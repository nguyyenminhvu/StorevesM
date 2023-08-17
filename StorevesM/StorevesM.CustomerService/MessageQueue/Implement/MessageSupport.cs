using Newtonsoft.Json;
using RabbitMQ.Client;
using StorevesM.CustomerService.Enum;
using StorevesM.CustomerService.MessageQueue.Interface;
using StorevesM.CustomerService.Model.Message;
using StorevesM.CustomerService.ProductExtension;
using StorevesM.CustomerService.Service;
using System.Text;

namespace StorevesM.CustomerService.MessageQueue.Implement
{
    public class MessageSupport : IMessageSupport, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
            _configuration = configuration;
            InitialBus();
        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        private void InitialBus()
        {
            try
            {
                ConnectionFactory _factory = new ConnectionFactory();
                _factory.HostName = _configuration["RabbitMQHost"];
                _factory.Port = Convert.ToInt32(_configuration["RabbitMQPort"]);
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at InitialBus: " + ex.Message);
            }
        }

        private void InitialBroker(MessageRaw raw)
        {
            if (_connection == null! || !_connection.IsOpen)
            {
                InitialBus();
            }
            _channel.ExchangeDeclare(raw.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(raw.QueueName, false, false, false, null!);
            _channel.QueueBind(raw.QueueName, raw.ExchangeName, raw.RoutingKey, null!);
        }

        private void SendRequest(MessageRaw raw)
        {
            InitialBroker(raw);
            var rawSerial = JsonConvert.SerializeObject(raw);
            var body = Encoding.UTF8.GetBytes(rawSerial);
            _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
        }

        public async Task ResponseGetCustomer(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                using (var _scope = _scopeFactory.CreateScope())
                {
                    var customerService = _scope.ServiceProvider.GetRequiredService<ICustomerService>();
                    var customer = await customerService.GetCustomer(Convert.ToInt32(raw.Message));

                    raw.QueueName = Queue.GetCustomerResponseQueue;
                    raw.ExchangeName = Exchange.GetCustomerDirect;
                    raw.RoutingKey = RoutingKey.GetCustomerResponse;
                    raw.Message = customer.SerializeMessageRaw();

                    SendRequest(raw);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at ResponseGetCustomer: " + ex.Message);
            }
            finally
            {
                Dispose();
            }
        }
    }
}

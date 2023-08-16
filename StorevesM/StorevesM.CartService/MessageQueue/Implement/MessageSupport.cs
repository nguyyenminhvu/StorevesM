using Newtonsoft.Json;
using RabbitMQ.Client;
using StorevesM.CartService.Enum;
using StorevesM.CartService.Model.Message;
using StorevesM.CartService.Service.Interface;
using StorevesM.ProductService.MessageQueue.Interface;
using System.Text;

namespace StorevesM.CartService.MessageQueue.Implement
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
            if (!_connection.IsOpen)
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

        public async Task ClearCartItem(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                using (var _scope = _scopeFactory.CreateScope())
                {
                    var cartService = _scope.ServiceProvider.GetRequiredService<ICartService>();
                    var clean = await cartService.ClearCartItem(Convert.ToInt32(raw.Message));

                    raw.QueueName = Queue.ClearCartItemResQueue;
                    raw.ExchangeName = Exchange.ClearCartItemDirect;
                    raw.RoutingKey = RoutingKey.ClearCartItemResponse;
                    raw.Message = clean ? "true" : "false";
                    SendRequest(raw);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at ClearCartItem: " + ex.Message);

            }
            finally
            {
                Dispose();
            }
        }
    }
}

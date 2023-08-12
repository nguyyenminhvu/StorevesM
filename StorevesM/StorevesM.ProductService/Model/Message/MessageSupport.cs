using Azure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.ProductService.Enum;
using System.Text;
using System.Threading.Channels;

namespace StorevesM.ProductService.Model.Message
{
    public class MessageSupport : IMessageSupport
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration)
        {
            _configuration = configuration;
            InitialBus();
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
            _channel.ExchangeDeclare(raw.ExchangeName, ExchangeType.Direct, false, false, null!);
            _channel.QueueDeclare(raw.QueueName, false, false, false, null!);
            _channel.QueueBind(raw.QueueName, raw.ExchangeName, raw.RoutingKey, null!);
        }

        private void SendRequest(MessageRaw raw)
        {
            InitialBroker(raw);
            var body = System.Text.Encoding.UTF8.GetBytes(raw.Message);
            _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
        }
        private void StartProcessingAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
        }
        public Task<bool> CheckCategoryExist(MessageRaw raw)
        {
            try
            {
                CancellationToken stoppingToken = new CancellationToken();
                SendRequest(raw);
                StartProcessingAsync(stoppingToken)
                var tcs = new TaskCompletionSource<bool>();
                raw.QueueName = Queue.GetCategoryResponseQueue;
                raw.RoutingKey = RoutingKey.GetCategoryResponse;
                InitialBroker(raw);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, ea) =>
                {
                    var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                    tcs.SetResult(response == "true");
                };
                _channel.BasicConsume(queue: raw.QueueName, autoAck: true, consumer: consumer);
                return tcs.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at CheckCategoryExist: " + ex.Message);
                return Task.FromResult(false);
            }
        }
    }
}

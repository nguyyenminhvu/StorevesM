using RabbitMQ.Client;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class GetProductsSubcribe : BackgroundService, IMessageSubcribe, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public GetProductsSubcribe(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void InititalBus(MessageChanel messageChanel)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = _configuration["RabbitMQHost"];
            factory.Port = Convert.ToInt32(_configuration["RabbitMQPort"]);

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(messageChanel.QueueName, false, false, false, null!);
            _channel.ExchangeDeclare(messageChanel.ExchangeName, ExchangeType.Direct);
            _channel.QueueBind(messageChanel.QueueName, messageChanel.ExchangeName, messageChanel.RoutingKey);
        }
        private void Disposed()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}

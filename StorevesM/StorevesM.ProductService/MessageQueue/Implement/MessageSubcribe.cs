using RabbitMQ.Client;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSubcribe : IMessageSubcribe
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageSubcribe(IConfiguration configuration)
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
        public Task<string> SubcribeMessageBroker(MessageChanel messageChanel, CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            InititalBus(messageChanel);
        }

    }
}

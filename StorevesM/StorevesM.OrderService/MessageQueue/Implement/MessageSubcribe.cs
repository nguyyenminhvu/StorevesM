using RabbitMQ.Client;
using StorevesM.OrderService.Model.Message;

namespace StorevesM.OrderService.MessageQueue.Implement
{
    public class MessageSubcribe :BackgroundService,IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageSubcribe(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
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


    }
}

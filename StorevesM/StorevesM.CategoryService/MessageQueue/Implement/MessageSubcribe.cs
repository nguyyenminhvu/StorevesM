using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CategoryService.Enum;
using StorevesM.CategoryService.MessageQueue.Interface;
using StorevesM.CategoryService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSubcribe : BackgroundService, IDisposable
    {
        private readonly IMessageFactory _messageFactory;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private MessageChanel _messageChanel = new();
        public MessageSubcribe(IConfiguration configuration, IMessageFactory messageFactory)
        {
            _messageFactory = messageFactory;
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageChanel.RoutingKey = RoutingKey.GetCategoryRequest;
            _messageChanel.ExchangeName = Exchange.GetCategoryDirect;
            _messageChanel.QueueName = Queue.GetCategoryRequestQueue;
            InititalBus(_messageChanel);
            var _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += async (sender, e) =>
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                await _messageFactory.ProcessMessage(body);
            };

            _channel.BasicConsume(queue: _messageChanel.QueueName, autoAck: true, consumer: _consumer);

            return Task.CompletedTask;
        }
    }
}

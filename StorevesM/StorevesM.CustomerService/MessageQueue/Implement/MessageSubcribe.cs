using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CustomerService.MessageQueue.Interface;
using StorevesM.CustomerService.Model.Message;
using StorevesM.OrderService.MessageQueue;

namespace StorevesM.CustomerService.MessageQueue.Implement
{
    public class MessageSubcribe : BackgroundService, IDisposable
    {
        private readonly IMessageFactory _messageFactory;
        private readonly IEnumerable<MessageChanel> _messageChannels;
        private readonly IConfiguration _configuration;
        private readonly MessageGetConnection _messageGetConnection;
        private IConnection _connection;
        private IModel _channel;

        public MessageSubcribe(IConfiguration configuration, IEnumerable<MessageChanel> messageChanels, IMessageFactory messageFactory)
        {
            _messageFactory = messageFactory;
            _messageChannels = messageChanels;
            _configuration = configuration;
            _messageGetConnection = new MessageGetConnection(_configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var item in _messageChannels)
            {
                if (item.RoutingKey == null || item.QueueName == null || item.ExchangeName == null)
                {
                    continue;
                }
                _channel = _messageGetConnection.InititalBus(item);
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += async (sender, args) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                    await _messageFactory.ProcessMessage(body);
                };
                _channel.BasicConsume(item.QueueName, true, consumer);
            }
            await Task.CompletedTask;
        }

    }
}

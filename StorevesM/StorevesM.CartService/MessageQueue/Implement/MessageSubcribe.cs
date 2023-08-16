using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CartService.MessageQueue.Interface;
using StorevesM.CartService.Model.Message;
using StorevesM.ProductService.MessageQueue;

namespace StorevesM.CartService.MessageQueue.Implement
{
    public class MessageSubcribe : BackgroundService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly MessageGetConnection _messageGetChannel;
        private readonly IMessageFactory _messageFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly IEnumerable<MessageChanel> _messageChannel;

        public MessageSubcribe(IConfiguration configuration, IMessageFactory messageFactory, IEnumerable<MessageChanel> messageChanels)
        {
            _configuration = configuration;
            _messageGetChannel = new MessageGetConnection(_configuration);
            _messageFactory = messageFactory;
            _messageChannel = messageChanels;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var item in _messageChannel)
            {
                if (item.RoutingKey == null! || item.QueueName == null || item.ExchangeName == null!)
                {
                    continue;
                }

                _channel = _messageGetChannel.InititalBus(item);
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

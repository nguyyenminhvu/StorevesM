using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CategoryService.MessageQueue.Interface;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSubcribe : BackgroundService, IMessageSubcribe
    {
        private readonly IMessageFactory _messageFactory;
        private readonly MessageGetConnection _getChannel;
        private IModel _channel;
        private readonly IEnumerable<MessageChanel> _messageChannels;

        public MessageSubcribe(IMessageFactory messageFactory, IConfiguration configuration, IEnumerable<MessageChanel> messageChanels)
        {
            _messageFactory = messageFactory;
            _getChannel = new MessageGetConnection(configuration);
            _messageChannels = messageChanels;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var item in _messageChannels)
            {
                if (item.RoutingKey == null || item.QueueName == null || item.ExchangeName == null)
                {
                    continue;
                }
                _channel = _getChannel.InititalBus(item);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (sender, ea) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                    await _messageFactory.ProcessMessage(body);
                };

                _channel.BasicConsume(item.QueueName, true, consumer);
            }
            await Task.CompletedTask;
        }
    }

}

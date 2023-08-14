using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CategoryService.Enum;
using StorevesM.CategoryService.Model.Message;
using StorevesM.CategoryService.Service;
using StorevesM.ProductService.MessageQueue.Interface;
using System.Text;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSupport : IMessageSupport
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration, ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
            _channel.ExchangeDeclare(raw.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(raw.QueueName, false, false, false, null!);
            _channel.QueueBind(raw.QueueName, raw.ExchangeName, raw.RoutingKey, null!);
        }

        private void SendRequest(MessageRaw raw)
        {
            InitialBroker(raw);
            var body = Encoding.UTF8.GetBytes(raw.Message);
            _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
        }

        public async Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                SendRequest(raw);

                cancellation.ThrowIfCancellationRequested();

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

                // Waiting 1 in 2 complete or 1 in 2 fail (take the other task without fail)=> waitResult is new Task complete from 2 Task inside WhenAny
                // tcs.Task waiting response
                // Task.Delay mean cancn
                var waitResult = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(3), cancellation));

                // waitResult = tcs.Task when tcs.Task complete(responsed)
                if (waitResult == tcs.Task)
                {
                    Disposed();
                    return tcs.Task.Result;
                }
                else
                {
                    Disposed();
                    Console.WriteLine("Timeout occurred while waiting for response.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at CheckCategoryExist: " + ex.Message);
                return false;
            }
        }

        private void Disposed()
        {
            if (_connection.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public async Task ResponseCheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                raw.Message = (await _categoryService.GetCategory(Convert.ToInt32(raw.Message))) != null ? "true" : "false";
                raw.RoutingKey = RoutingKey.GetCategoryResponse;
                raw.QueueName = Queue.GetCategoryResponseQueue;
                raw.ExchangeName = Exchange.GetCategoryDirect;
                InitialBroker(raw);
                SendRequest(raw);
                Disposed();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at CheckCategoryExist: " + ex.Message);
            }
        }
    }
}

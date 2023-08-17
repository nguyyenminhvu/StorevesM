using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.CustomerService.Enum;
using StorevesM.CustomerService.MessageQueue.Interface;
using StorevesM.CustomerService.Model.DTOMessage;
using StorevesM.CustomerService.Model.Message;
using StorevesM.CustomerService.ProductExtension;
using System.Text;

namespace StorevesM.OrderService.MessageQueue.Implement
{
    public class MessageSupport : IMessageSupport, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration)
        {
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
            if (_connection == null! || !_connection.IsOpen)
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
       
        public async Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();

                SendRequest(raw);
                raw.QueueName = Queue.GetCategoryResponseQueue;
                raw.RoutingKey = RoutingKey.GetCategoryResponse;
                raw.ExchangeName = Exchange.GetCategoryDirect;
                InitialBroker(raw);

                var taskcs = new TaskCompletionSource<CategoryDTO>();
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (sender, e) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                    taskcs.SetResult(body.DeserializeToCategoryDTO());
                };

                _channel.BasicConsume(queue: raw.QueueName, autoAck: true, consumer: consumer);

                // Waiting 1 in 2 complete or 1 in 2 fail (take the other task without fail)=> newTask is new Task complete from 2 Task inside WhenAny
                // tcs.Task waiting response
                // Task.Delay mean cancn
                var newTask = await Task.WhenAny(taskcs.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellation));

                // newTask = tcs.Task when tcs.Task complete(responsed)
                if (newTask == taskcs.Task)
                {
                    return taskcs.Task.Result;
                }
                else
                {
                    return null!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at GetCategory: " + ex.Message);
                return null!;
            }
        }

        public async Task<List<ProductDTO>> GetProducts(MessageRaw raw, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                SendRequest(raw);
                raw.ExchangeName = Exchange.GetProductsDirect;
                raw.QueueName = Queue.GetProductsResponseQueue;
                raw.RoutingKey = RoutingKey.GetProductsResponse;
                InitialBroker(raw);

                var taskCompletionSrc = new TaskCompletionSource<List<ProductDTO>>();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, args) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                    var messageRaw = body.DeserializeToMessageRaw();
                    taskCompletionSrc.SetResult(messageRaw.Message.DeserializeProductsDTO());
                };
                _channel.BasicConsume(raw.QueueName, true, consumer);

                var newTask = await Task.WhenAny(taskCompletionSrc.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellationToken));

                if (taskCompletionSrc.Task == newTask)
                {
                    var result = taskCompletionSrc.Task.Result;
                    Dispose();
                    return result;
                }
                else
                {
                    Dispose();
                    return null!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at GetProducts: " + ex.Message);
                Dispose();
                throw;
            }
        }

        public async Task<bool> UpdateQuantityProduct(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                SendRequest(raw);

                raw.RoutingKey = RoutingKey.UpdateQuantityResProduct;
                raw.QueueName = Queue.UpdateQuantityProductResQ;
                raw.ExchangeName = Exchange.UpdateQuantityProductDirect;
                InitialBroker(raw);

                var taskComletionSrc = new TaskCompletionSource<bool>();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (s, e) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                    var messageRaw = body.DeserializeToMessageRaw();
                    taskComletionSrc.SetResult(messageRaw.Message == "true");
                };

                _channel.BasicConsume(raw.QueueName, true, consumer);

                var newTask = await Task.WhenAny(taskComletionSrc.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellation));

                if (newTask == taskComletionSrc.Task)
                {
                    var result = taskComletionSrc.Task.Result;
                    Dispose();
                    return result;
                }
                else
                {
                    Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at UpdateQuantityProduct: " + ex.Message);
                Dispose();
                throw;
            }
        }

        public async Task<bool> ClearCartItem(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                SendRequest(raw);

                raw.ExchangeName = Exchange.ClearCartItemDirect;
                raw.QueueName = Queue.ClearCartItemResQueue;
                raw.RoutingKey = RoutingKey.ClearCartItemResponse;
                InitialBroker(raw);

                var taskCompletionSrc = new TaskCompletionSource<bool>();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (s, e) =>
                {
                    var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                    var messageRaw = body.DeserializeToMessageRaw();
                    taskCompletionSrc.SetResult(messageRaw.Message == "true");
                };

                _channel.BasicConsume(raw.QueueName, true, consumer);

                var newTask = await Task.WhenAny(taskCompletionSrc.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellation));

                if (newTask == taskCompletionSrc.Task)
                {
                    var result = taskCompletionSrc.Task.Result;
                    Dispose();
                    return result;
                }
                else
                {
                    Dispose();
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at ClearCartItem: " + ex.Message);
                Dispose();
                return false;
            }
        }
    }
}

using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StorevesM.ProductService.Enum;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.DTOMessage;
using StorevesM.ProductService.Model.Message;
using StorevesM.ProductService.ProductExtension;
using StorevesM.ProductService.Service;
using System.Text;
using System.Threading.Channels;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSupport : IMessageSupport, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceScopeFactory _scopeService;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration, IServiceScopeFactory serviceScope)
        {
            _scopeService = serviceScope;
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
            if (!_connection.IsOpen)
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

        private void ChangeExchangeListen(MessageRaw raw)
        {
            raw.QueueName = Queue.GetCategoryResponseQueue;
            raw.RoutingKey = RoutingKey.GetCategoryResponse;
            raw.ExchangeName = Exchange.GetCategoryDirect;
            InitialBroker(raw);
        }

        #region CheckCategoryExist
        //public async Task<bool> CheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default)
        //{
        //    try
        //    {
        //        cancellation.ThrowIfCancellationRequested();

        //        SendRequest(raw);
        //        ChangeExchangeListen(raw);

        //        var tcs = new TaskCompletionSource<bool>();

        //        var consumer = new EventingBasicConsumer(_channel);
        //        consumer.Received += (sender, ea) =>
        //        {
        //            var response = Encoding.UTF8.GetString(ea.Body.ToArray());
        //            var category = response.DeserializeToCategoryDTO();
        //            tcs.SetResult(category != null!);
        //        };

        //        _channel.BasicConsume(queue: raw.QueueName, autoAck: true, consumer: consumer);

        //        // Waiting 1 in 2 complete or 1 in 2 fail (take the other task without fail)=> waitResult is new Task complete from 2 Task inside WhenAny
        //        // tcs.Task waiting response
        //        // Task.Delay mean cancn
        //        var waitResult = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(10), cancellation));

        //        // waitResult = tcs.Task when tcs.Task complete(responsed)
        //        if (waitResult == tcs.Task)
        //        {
        //            return tcs.Task.Result;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Timeout occurred while waiting for response.");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error in MessageSupport at CheckCategoryExist: " + ex.Message);
        //        return false;
        //    }
        //}
        #endregion

        #region SendAndListenResponseGetCategory
        public async Task<CategoryDTO> GetCategory(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();

                SendRequest(raw);
                ChangeExchangeListen(raw);

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
        #endregion

        public async Task ResponseGetProducts(MessageRaw raw, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                using (var _scope = _scopeService.CreateScope())
                {
                    var productService = _scope.ServiceProvider.GetRequiredService<IProductService>();
                    var products = await productService.GetProducts();
                    raw.QueueName = Queue.GetProductsResponseQueue;
                    raw.ExchangeName = Exchange.GetProductsDirect;
                    raw.RoutingKey = RoutingKey.GetProductsResponse;
                    SendRequest(raw);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at ResponseGetProducts: " + ex.Message);
            }
        }

        public async Task ResponseUpdateQuantity(MessageRaw raw, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var _scope = _scopeService.CreateScope())
                {
                    var productService = _scope.ServiceProvider.GetRequiredService<IProductService>();
                    var updated = await productService.UpdateQuantityProduct(raw.Message.DeserializeCartDTO());
                    raw.Message = updated == true ? "true" : "false";
                    raw.QueueName = Queue.UpdateQuantityProductResQ;
                    raw.ExchangeName = Exchange.UpdateQuantityProduct;
                    raw.RoutingKey = RoutingKey.UpdateQuantityResProduct;
                    SendRequest(raw);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageSupport at  ResponseClearCartItem: " + ex.Message);
            }
        }

    }
}

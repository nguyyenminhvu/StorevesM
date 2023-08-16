using RabbitMQ.Client;
using StorevesM.CategoryService.Enum;
using StorevesM.CategoryService.Model.Message;
using StorevesM.CategoryService.Service;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.ProductExtension;
using System.Text;

namespace StorevesM.ProductService.MessageQueue.Implement
{
    public class MessageSupport : IMessageSupport, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private ICategoryService _categoryService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public MessageSupport(IConfiguration configuration, IServiceScopeFactory serviceScope)
        {
            _scopeFactory = serviceScope;
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
            InitialBus();
            InitialBroker(raw);
            var body = Encoding.UTF8.GetBytes(raw.Message);
            _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
        }
        public async Task ResponseCheckCategoryExist(MessageRaw raw, CancellationToken cancellation = default)
        {
            using (var _scope = _scopeFactory.CreateScope())
            {
                try
                {
                    _categoryService = _scope.ServiceProvider.GetRequiredService<ICategoryService>();
                    var category = await _categoryService.GetCategory(Convert.ToInt32(raw.Message));
                    raw.Message = category != null ? category.SerializeCategoryDtoToString() : null!;
                    raw.RoutingKey = RoutingKey.GetCategoryResponse;
                    raw.QueueName = Queue.GetCategoryResponseQueue;
                    raw.ExchangeName = Exchange.GetCategoryDirect;
                    SendRequest(raw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in MessageSupport at CheckCategoryExist: " + ex.Message);
                }
                finally
                {
                    Dispose();
                }
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}

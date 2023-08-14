using RabbitMQ.Client;
using StorevesM.CategoryService.Model.Message;
using StorevesM.ProductService.MessageQueue.Interface;

namespace StorevesM.CategoryService.MessageQueue.Implement
{
    public class MessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
            InitialBus();
        }

        // Initial connection
        private void InitialBus()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = _configuration["RabbitMQHost"];
                factory.Port = Convert.ToInt32(_configuration["RabbitMQPort"]);
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MessageBus at InitialBusL " + ex.Message);
            }
        }

        //Config exchange, queue, routing key
        private void ConfigurationBroker(MessageRaw raw)
        {
            _channel.ExchangeDeclare(raw.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(raw.QueueName, false, false, false, null);
            _channel.QueueBind(raw.QueueName, raw.ExchangeName, raw.RoutingKey, null);
        }

        // Send message
        public void PublicMessage(MessageRaw raw)
        {
            try
            {
                if (_connection.IsOpen)
                {
                    ConfigurationBroker(raw);
                    var body = System.Text.Encoding.UTF8.GetBytes(raw.Message);
                    _channel.BasicPublish(raw.ExchangeName, raw.RoutingKey, null!, body);
                    Disposed();
                    Console.WriteLine("Send success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PublicMessage at Message: " + ex.Message);
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
    }
}

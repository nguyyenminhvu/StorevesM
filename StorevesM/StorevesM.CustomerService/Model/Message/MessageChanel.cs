using StorevesM.CustomerService.Enum;

namespace StorevesM.CustomerService.Model.Message
{
    public class MessageChanel
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }


        public MessageChanel GetCustomer()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.GetCustomerRequestQueue;
            _mesageChanel.ExchangeName = Exchange.GetCustomerDirect;
            _mesageChanel.RoutingKey = CustomerService.Enum.RoutingKey.GetCustomerRequest;
            return _mesageChanel;
        }
      
    }
}

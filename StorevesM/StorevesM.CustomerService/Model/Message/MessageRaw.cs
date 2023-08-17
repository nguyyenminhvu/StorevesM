namespace StorevesM.CustomerService.Model.Message
{
    public class MessageRaw
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
        public string Message { get; set; }
    }
}

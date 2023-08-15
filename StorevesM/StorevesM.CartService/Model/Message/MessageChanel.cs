namespace StorevesM.CartService.Model.Message
{
    public class MessageChanel
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
    }
}

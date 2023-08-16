using StorevesM.ProductService.Enum;

namespace StorevesM.ProductService.Model.Message
{
    public class MessageChanel
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }

        public MessageChanel GetProducts()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.GetProductsRequestQueue;
            _mesageChanel.ExchangeName = Exchange.GetProductsDirect;
            _mesageChanel.RoutingKey = ProductService.Enum.RoutingKey.GetProductsRequest;
            return _mesageChanel;
        }
        public MessageChanel UpdateQuantityProduct()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.UpdateQuantityProductReqQ;
            _mesageChanel.ExchangeName = Exchange.UpdateQuantityProduct;
            _mesageChanel.RoutingKey = ProductService.Enum.RoutingKey.UpdateQuantityReqProduct;
            return _mesageChanel;
        }
        public MessageChanel ClearCartItem()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.ClearCartItemReqQueue;
            _mesageChanel.ExchangeName = Exchange.ClearCartItem;
            _mesageChanel.RoutingKey = ProductService.Enum.RoutingKey.ClearCartItem;
            return _mesageChanel;
        }
    }
}

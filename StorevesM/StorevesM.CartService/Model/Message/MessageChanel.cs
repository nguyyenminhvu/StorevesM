using StorevesM.CartService.Entity;
using StorevesM.CartService.Enum;

namespace StorevesM.CartService.Model.Message
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
            _mesageChanel.RoutingKey = CartService.Enum.RoutingKey.GetProductsRequest;
            return _mesageChanel;
        }
        public MessageChanel UpdateQuantityProduct()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.UpdateQuantityProductReqQ;
            _mesageChanel.ExchangeName = Exchange.UpdateQuantityProductDirect;
            _mesageChanel.RoutingKey = CartService.Enum.RoutingKey.UpdateQuantityReqProduct;
            return _mesageChanel;
        }
        public MessageChanel ClearCartItem()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.ClearCartItemReqQueue;
            _mesageChanel.ExchangeName = Exchange.ClearCartItemDirect;
            _mesageChanel.RoutingKey = CartService.Enum.RoutingKey.ClearCartItemRequest;
            return _mesageChanel;
        }

    }
}

using Newtonsoft.Json;
using StorevesM.CategoryService.MessageQueue.Interface;
using StorevesM.ProductService.Enum;
using StorevesM.ProductService.MessageQueue.Interface;
using StorevesM.ProductService.Model.Message;

namespace StorevesM.CategoryService.MessageQueue.Implement
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IMessageSupport _messageSupport;

        public MessageFactory(IMessageSupport messageSupport)
        {
            _messageSupport = messageSupport;
        }

        public async Task ProcessMessage(string messageRaw)
        {
            MessageRaw message = JsonConvert.DeserializeObject<MessageRaw>(messageRaw)!;
            switch (message.RoutingKey)
            {
                case RoutingKey.GetCategoryRequest: /*await _messageSupport.ResponseCheckCategoryExist(message);*/ break;
                case RoutingKey.UpdateQuantityReqProduct: await _messageSupport.ResponseUpdateQuantity(message); break;
                case RoutingKey.ClearCartItemRequest: /*await _messageSupport.ResponseCheckCategoryExist(message);*/ break;
                case RoutingKey.GetProductsRequest: await _messageSupport.ResponseGetProducts(message); break;
            }
        }
    }
}

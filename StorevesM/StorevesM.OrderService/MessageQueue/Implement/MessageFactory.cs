using Newtonsoft.Json;
using StorevesM.OrderService.Enum;
using StorevesM.OrderService.MessageQueue.Interface;
using StorevesM.OrderService.Model.Message;

namespace StorevesM.OrderService.MessageQueue.Implement
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
            }
        }
    }
}

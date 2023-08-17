using Newtonsoft.Json;
using StorevesM.CustomerService.Enum;
using StorevesM.CustomerService.MessageQueue.Interface;
using StorevesM.CustomerService.Model.Message;

namespace StorevesM.CustomerService.MessageQueue.Implement
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

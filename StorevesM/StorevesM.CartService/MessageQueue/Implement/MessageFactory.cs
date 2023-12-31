﻿using Newtonsoft.Json;
using StorevesM.CartService.Enum;
using StorevesM.CartService.MessageQueue.Interface;
using StorevesM.CartService.Model.Message;
using StorevesM.ProductService.MessageQueue.Interface;

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
                case RoutingKey.ClearCartItemRequest: await _messageSupport.ClearCartItem(message); break;
            }
        }
    }
}

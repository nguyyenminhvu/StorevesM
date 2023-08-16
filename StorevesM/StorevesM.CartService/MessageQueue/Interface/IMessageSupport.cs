﻿using StorevesM.CartService.Model.Message;

namespace StorevesM.ProductService.MessageQueue.Interface
{
    public interface IMessageSupport
    {
        Task ClearCartItem(MessageRaw raw, CancellationToken cancellation = default);
    }
}

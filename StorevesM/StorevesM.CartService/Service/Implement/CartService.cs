using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StorevesM.CartService.Entity;
using StorevesM.CartService.Enum;
using StorevesM.CartService.Model.DTOMessage;
using StorevesM.CartService.Model.Request;
using StorevesM.CartService.Model.View;
using StorevesM.CartService.Repository.Implement;
using StorevesM.CartService.Service.Interface;
using StorevesM.ProductService.MessageQueue.Interface;

namespace StorevesM.CartService.Service.Implement
{
    public class CartService : ICartService
    {
        private readonly IMessageSupport _messageSupport;
        private readonly IMapper _mapper;
        private readonly CartServiceDbcontext _context;
        private readonly Repository<Cart> _cartRepository;
        private readonly Repository<CartItem> _cartItemRepository;

        public CartService(CartServiceDbcontext cartServiceDbcontext, IMapper mapper, IMessageSupport messageSupport)
        {
            _messageSupport = messageSupport;
            _mapper = mapper;
            _context = cartServiceDbcontext;
            _cartRepository = new Repository.Implement.Repository<Cart>(_context);
            _cartItemRepository = new Repository.Implement.Repository<CartItem>(_context);
        }

        public async Task<bool> ClearCartItem(int cartId)
        {
            var cart = await _cartRepository.GetMany(x => x.Id == cartId).Include(x => x.CartItems).FirstOrDefaultAsync();
            if (cart != null)
            {
                var cartItem = cart.CartItems;
                _cartItemRepository.RemoveRangeAsync(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CustomerDTO> DemoCallCustomer(int id)
        {
            try
            {
                var customer = await _messageSupport.GetCustomer(new Model.Message.MessageRaw { ExchangeName = Exchange.GetCustomerDirect, QueueName = Queue.GetCustomerRequestQueue, RoutingKey = RoutingKey.GetCustomerRequest, Message = id.ToString() });
                return customer != null! ? customer : null!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public async Task<CartViewModel> GetCart(int customerId)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync(x => x.CustomerId == customerId, x => x.CartItems);
            if (cart == null)
            {
                await _cartRepository.AddAsync(new Cart { CustomerId = customerId });
                cart = await _cartRepository.FirstOrDefaultAsync(x => x.CustomerId == customerId, x => x.CartItems);
            }
            return cart != null ? _mapper.Map<CartViewModel>(cart) : null!;
        }

        public async Task<CartViewModel> UpdateCart(CartUpdateModel cum, int customerId)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync(x => x.Id == cum.CartId, x => x.CartItems);
            if (cart == null)
            {
                await _cartRepository.AddAsync(new Cart { CustomerId = customerId });
                cart = await _cartRepository.FirstOrDefaultAsync(x => x.CustomerId == customerId, x => x.CartItems);
            }
            else
            {
                var cartItem = _cartItemRepository.GetMany(x => x.CartId == cum.CartId);
                _cartItemRepository.RemoveRangeAsync(cartItem);
            }

            foreach (var item in cum.CartItems)
            {
                await _cartItemRepository.AddAsync(new CartItem { ProductId = item.ProductId, Quantity = item.Quantity, Price = item.Price, CartId = cum.CartId, CreateAt = DateTime.Now });
            }

            await _context.SaveChangesAsync();
            return await GetCart(customerId);
        }
    }
}

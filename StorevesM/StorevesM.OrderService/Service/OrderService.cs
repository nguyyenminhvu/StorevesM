using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorevesM.OrderService.Entity;
using StorevesM.OrderService.Enum;
using StorevesM.OrderService.MessageQueue.Interface;
using StorevesM.OrderService.Model.DTOMessage;
using StorevesM.OrderService.Model.Request;
using StorevesM.OrderService.Model.View;
using StorevesM.OrderService.ProductExtension;
using StorevesM.OrderService.Repository;

namespace StorevesM.OrderService.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMessageSupport _messageSupport;
        private readonly IMapper _mapper;
        private readonly OrderDbContext _context;
        private readonly Repository<Order> _orderRepository;
        private readonly Repository<OrderDetail> _orderDetailRepository;

        public OrderService(OrderDbContext orderDbContext, IMapper mapper, IMessageSupport messageSupport)
        {
            _messageSupport = messageSupport;
            _mapper = mapper;
            _context = orderDbContext;
            _orderRepository = new Repository<Order>(_context);
            _orderDetailRepository = new Repository<OrderDetail>(_context);
        }
        public async Task<OrderViewModel> CreateOrder(CartDTO cart)
        {
            // Get products from ProductService
            var products = await _messageSupport.GetProducts(new Model.Message.MessageRaw
            {
                ExchangeName = Exchange.GetProductsDirect,
                Message = "get",
                QueueName =
                Queue.GetProductsRequestQueue,
                RoutingKey = RoutingKey.GetProductsRequest
            });
            if (products == null)
            {
                return null!;
            }
            Order order = new Order();
            order.CustomerId = cart.CustomerId;
            order.Status = OrderStatus.Processing;
            await _orderRepository.AddAsync(order);
            await _context.SaveChangesAsync();
            foreach (var item in cart.CartItems)
            {
                if (item.Quantity > products.FirstOrDefault(x => x.Id == item.ProductId)!.Quantity)
                {
                    return null!;
                }
                await _orderDetailRepository.AddAsync(new OrderDetail { OrderId = order.Id, ProductId = item.ProductId, Price = item.Price, Quantity = item.Quantity, CreateAt = DateTime.Now });
            }

            // Update quantity product on ProductService (CartDTO)
            var updated = await _messageSupport.UpdateQuantityProduct(new Model.Message.MessageRaw { Message = cart.SerializeCartDTO(), QueueName = Queue.UpdateQuantityProductReqQ, ExchangeName = Exchange.UpdateQuantityProductDirect, RoutingKey = RoutingKey.UpdateQuantityReqProduct });

            if (!updated) { return null!; }

            // Clear cartitem on CartService (cartId)
            var cleaned = await _messageSupport.ClearCartItem(new Model.Message.MessageRaw { Message = cart.SerializeCartDTO(), QueueName = Queue.ClearCartItemReqQueue, ExchangeName = Exchange.ClearCartItemDirect, RoutingKey = RoutingKey.ClearCartItemRequest });

            if (!cleaned) { return null!; }

            await _context.SaveChangesAsync();
            return await GetOrder(order.Id);
        }

        public async Task<bool> DemoClearCartItem()
        {
            var cleaned = await _messageSupport.ClearCartItem(new Model.Message.MessageRaw { Message = 1.ToString(), QueueName = Queue.ClearCartItemReqQueue, ExchangeName = Exchange.ClearCartItemDirect, RoutingKey = RoutingKey.ClearCartItemRequest });
            return cleaned;
        }

        public async Task<List<ProductDTO>> DemoGetProduct()
        {
            var products = await _messageSupport.GetProducts(new Model.Message.MessageRaw
            {
                ExchangeName = Exchange.GetProductsDirect,
                Message = "get",
                QueueName =
              Queue.GetProductsRequestQueue,
                RoutingKey = RoutingKey.GetProductsRequest
            });
            return products;
        }

        public async Task<bool> DemoUpdateQuantityProduct()
        {
            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            cartItems.Add(new CartItemDTO { ProductId = 1, Quantity = 1 });
            cartItems.Add(new CartItemDTO { ProductId = 2, Quantity = 1 });
            CartDTO cartDTO = new();
            cartDTO.CartItems = cartItems;
            var updated = await _messageSupport.UpdateQuantityProduct(new Model.Message.MessageRaw { Message = cartDTO.SerializeCartDTO(), QueueName = Queue.UpdateQuantityProductReqQ, ExchangeName = Exchange.UpdateQuantityProductDirect, RoutingKey = RoutingKey.UpdateQuantityReqProduct });
            return updated;

        }

        public async Task<OrderViewModel> GetOrder(int orderId)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(x => x.Id == orderId);
            return order != null ? _mapper.Map<OrderViewModel>(order) : null!;
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var orders = _orderRepository.GetAll();
            return orders.Count() > 0 ? await orders.ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider).ToListAsync() : null!;
        }

        public async Task<IActionResult> UpdateOrder(OrderUpdateModel oum)
        {
            var order = await _orderRepository.GetMany(x => x.Id == oum.Id).Include(x => x.OrderDetails).FirstOrDefaultAsync();
            if (order != null)
            {
                switch (oum.Status)
                {
                    case OrderStatus.Cancelled:
                        if (order.Status.Contains(OrderStatus.Shipping))
                        {
                            // Can't cancel order status Shipping
                            return new StatusCodeResult(456);
                        }
                        else
                        {
                            order.Status = OrderStatus.Cancelled;
                            order.IsActive = false;
                        }
                        break;
                    case OrderStatus.Shipping: order.Status = OrderStatus.Shipping; break;
                    case OrderStatus.Done: order.Status = OrderStatus.Done; order.IsActive = false; break;
                }
                await _context.SaveChangesAsync();
                return new JsonResult(await GetOrder(oum.Id));
            }
            return new StatusCodeResult(404);
        }
    }
}

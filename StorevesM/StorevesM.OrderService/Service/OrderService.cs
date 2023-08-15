using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorevesM.OrderService.Entity;
using StorevesM.OrderService.Enum;
using StorevesM.OrderService.Model.DTOMessage;
using StorevesM.OrderService.Model.Request;
using StorevesM.OrderService.Model.View;
using StorevesM.OrderService.Repository;

namespace StorevesM.OrderService.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly OrderDbContext _context;
        private readonly Repository<Order> _orderRepository;
        private readonly Repository<OrderDetail> _orderDetailRepository;

        public OrderService(OrderDbContext orderDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _context = orderDbContext;
            _orderRepository = new Repository<Order>(_context);
            _orderDetailRepository = new Repository<OrderDetail>(_context);
        }
        public async Task<OrderViewModel> CreateOrder(CartDTO cart)
        {
            // Get products from ProductService
            var products = new List<ProductDTO>();

            Order order = new Order();
            order.CustomerId = cart.CustomerId;
            order.Status = OrderStatus.Processing;
            await _orderRepository.AddAsync(order);

            foreach (var item in cart.CartItems)
            {
                if (item.Quantity > products.FirstOrDefault(x => x.Id == item.ProductId)!.Quantity)
                {
                    return null!;
                }
                await _orderDetailRepository.AddAsync(new OrderDetail { OrderId = order.Id, ProductId = item.ProductId, Price = item.Price, Quantity = item.Quantity, CreateAt = DateTime.Now });
            }

            // Update quantity product on ProductService (CartDTO)
            // Clear cartItem on CartItem (CartDTO)

            return await GetOrder(order.Id);
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

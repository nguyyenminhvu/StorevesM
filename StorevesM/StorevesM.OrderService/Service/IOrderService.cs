using Microsoft.AspNetCore.Mvc;
using StorevesM.OrderService.Model.DTOMessage;
using StorevesM.OrderService.Model.Request;
using StorevesM.OrderService.Model.View;

namespace StorevesM.OrderService.Service
{
    public interface IOrderService
    {
        Task<OrderViewModel> GetOrder(int orderId);
        Task<List<OrderViewModel>> GetOrders();
        Task<OrderViewModel> CreateOrder(CartDTO cart);
        Task<IActionResult> UpdateOrder(OrderUpdateModel oum);
    }
}

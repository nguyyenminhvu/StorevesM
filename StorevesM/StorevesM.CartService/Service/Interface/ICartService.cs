using StorevesM.CartService.Model.DTOMessage;
using StorevesM.CartService.Model.Request;
using StorevesM.CartService.Model.View;

namespace StorevesM.CartService.Service.Interface
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart(int customerId);
        Task<CartViewModel> UpdateCart(CartUpdateModel cum, int customerId);
        Task<bool> ClearCartItem(int cartId);
        Task<CustomerDTO> DemoCallCustomer(int id);
    }
}

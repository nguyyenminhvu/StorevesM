using AutoMapper;
using StorevesM.CartService.Entity;
using StorevesM.CartService.Model.View;

namespace StorevesM.CartService.Profiles
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper()
        {
            CreateMap<Cart, CartViewModel>();
            CreateMap<CartItem, CartItemViewModel>();
        }
    }
}

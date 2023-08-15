using AutoMapper;
using StorevesM.OrderService.Entity;
using StorevesM.OrderService.Model.View;

namespace StorevesM.OrderService.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderDetail, OrderDetailViewModel>();
        }
    }
}

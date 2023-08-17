using AutoMapper;
using StorevesM.CustomerService.Entity;
using StorevesM.CustomerService.Model.View;

namespace StorevesM.CustomerService.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Customer, CustomerViewModel>();
        }
    }
}

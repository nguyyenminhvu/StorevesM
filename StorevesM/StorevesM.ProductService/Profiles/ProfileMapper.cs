using AutoMapper;
using StorevesM.ProductService.Entity;
using StorevesM.ProductService.Model.View;

namespace StorevesM.ProductService.Profiles
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper()
        {
            CreateMap<Product, ProductViewModel>();
        }
    }
}

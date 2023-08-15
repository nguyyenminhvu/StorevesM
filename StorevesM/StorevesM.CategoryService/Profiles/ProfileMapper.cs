using AutoMapper;
using StorevesM.CategoryService.Entity;
using StorevesM.CategoryService.Model.DTOMessage;
using StorevesM.CategoryService.Model.View;

namespace StorevesM.CategoryService.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}

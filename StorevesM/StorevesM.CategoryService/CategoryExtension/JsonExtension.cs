using Newtonsoft.Json;
using StorevesM.CategoryService.Model.DTOMessage;
using StorevesM.CategoryService.Model.View;

namespace StorevesM.ProductService.ProductExtension
{
    public static class JsonExtension
    {
        public static CategoryDTO DeserializeToCategoryDTO(this string categoryJson)
        {
            if (categoryJson != null)
            {
                return JsonConvert.DeserializeObject<CategoryDTO>(categoryJson)!;
            }
            return null!;
        }

        public static string SerializeCategoryDtoToString(this CategoryDTO categoryDTO)
        {
            return JsonConvert.SerializeObject(categoryDTO);
        }

        public static CategoryViewModel DeserializeToCategoryViewModel(this string categoryJson)
        {
            if (categoryJson != null)
            {
                return JsonConvert.DeserializeObject<CategoryViewModel>(categoryJson)!;
            }
            return null!;
        }

        public static string SerializeCategoryDtoToString(this CategoryViewModel categoryDTO)
        {
            return JsonConvert.SerializeObject(categoryDTO);
        }
    }
}

using Newtonsoft.Json;
using StorevesM.ProductService.Model.DTOMessage;
using StorevesM.ProductService.Model.View;

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

        public static CartDTO DeserializeCartDTO(this string cartJson)
        {
            if (cartJson != null)
            {
                return JsonConvert.DeserializeObject<CartDTO>(cartJson)!;
            }
            return null!;
        }
        public static string SerializeProducts(this List<ProductViewModel> productDtos)
        {
            if (productDtos != null)
            {
                return JsonConvert.SerializeObject(productDtos);
            }
            return null!;
        }
    }
}

using Newtonsoft.Json;
using StorevesM.OrderService.Model.DTOMessage;

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
        public static List<ProductDTO> DeserializeProductsDTO(this string productsJson)
        {
            if (productsJson != null)
            {
                return JsonConvert.DeserializeObject<List<ProductDTO>>(productsJson)!;
            }
            return null!;
        }

        public static string SerializeCartDTO(this CartDTO cart)
        {
            if (cart != null)
            {
                return JsonConvert.SerializeObject(cart);
            }
            return null!;
        }
    }
}

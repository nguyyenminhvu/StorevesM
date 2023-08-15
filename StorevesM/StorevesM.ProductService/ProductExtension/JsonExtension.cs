using Newtonsoft.Json;
using StorevesM.ProductService.Model.DTOMessage;

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
    }
}

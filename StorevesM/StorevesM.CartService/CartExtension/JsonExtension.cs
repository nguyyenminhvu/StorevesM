using Newtonsoft.Json;
using StorevesM.CartService.Model.DTOMessage;
using StorevesM.CartService.Model.Message;

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
        public static CustomerDTO DeserializeCustomerDTO(this string customerJson)
        {
            if (customerJson != null)
            {
                return JsonConvert.DeserializeObject<CustomerDTO>(customerJson)!;
            }
            return null!;
        }
        public static MessageRaw DeserializeToMessageRaw(this string messageRawJson)
        {
            if (messageRawJson != null)
            {
                return JsonConvert.DeserializeObject<MessageRaw>(messageRawJson)!;
            }
            return null!;
        }
    }
}

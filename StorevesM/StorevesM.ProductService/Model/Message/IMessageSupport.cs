namespace StorevesM.ProductService.Model.Message
{
    public interface IMessageSupport
    {
        Task<bool> CheckCategoryExist(MessageRaw raw);
    }
}

namespace StorevesM.ProductService.Enum
{
    public static class RoutingKey
    {
        public const string CategoryRouting = "category-routing";
        public const string ProductRouting = "product-routing";
        public const string CustomerRouting = "customer-routing";
    }
    public enum Exchange
    {
        CategoryExchangeDirect,
        ProductExchangeDirect
    }
}

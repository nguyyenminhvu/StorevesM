namespace StorevesM.ProductService.Enum
{
    public static class RoutingKey
    {
        public const string GetCategoryRequest = "get-category-id-request";
        public const string GetCategoryResponse = "get-category-id-response";
        public const string GetCategories = "get-categories";
    }

    public static class Exchange
    {
        public const string GetCategoryDirect = "GetCategoryDirect";
        public const string GetCategoriesDirect = "GetCategoriesDirect";
    }

    public static class Queue
    {
        public const string GetCategoryRequestQueue = "GetCategoryRequestQueue";
        public const string GetCategoryResponseQueue = "GetCategoryResponseQueue";
        public const string GetCategoriesQueue = "GetCategoriesQueue";
    }
}

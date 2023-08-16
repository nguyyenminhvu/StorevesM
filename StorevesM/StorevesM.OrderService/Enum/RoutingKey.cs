namespace StorevesM.OrderService.Enum
{
    public static class RoutingKey
    {
        public const string GetCategoryRequest = "get-category-id-request";
        public const string GetCategoryResponse = "get-category-id-response";
        public const string GetCategories = "get-categories";

        public const string GetProductsRequest = "get-products-request";
        public const string GetProductsResponse = "get-products-response";

        public const string GetProductRequest = "get-product-id-request";
        public const string GetProductResponse = "get-product-id-response";

        public const string UpdateQuantityResProduct = "update-quantity-res-product";
        public const string UpdateQuantityReqProduct = "update-quantity-req-product";

        public const string ClearCartItemRequest = "clear-cart-item-request";
        public const string ClearCartItemResponse = "clear-cart-item-response";

    }

    public static class Exchange
    {
        public const string GetCategoryDirect = "GetCategoryDirect";
        public const string GetCategoriesDirect = "GetCategoriesDirect";

        public const string GetProductDirect = "GetProductDirect";
        public const string GetProductsDirect = "GetProductsDirect";
        public const string UpdateQuantityProductDirect = "UpdateQuantityProductDirect";

        public const string ClearCartItemDirect = "ClearCartItem";
    }

    public static class Queue
    {
        public const string GetCategoryRequestQueue = "GetCategoryRequestQueue";
        public const string GetCategoryResponseQueue = "GetCategoryResponseQueue";
        public const string GetCategoriesQueue = "GetCategoriesQueue";

        public const string GetProductRequestQueue = "GetProductRequestQueue";
        public const string GetProductResponseQueue = "GetProductResponseQueue";

        public const string GetProductsRequestQueue = "GetProductsRequestQueue";
        public const string GetProductsResponseQueue = "GetProductsResponseQueue";

        public const string UpdateQuantityProductResQ = "UpdateQuantityProductResQ";
        public const string UpdateQuantityProductReqQ = "UpdateQuantityProductReqQ";

        public const string ClearCartItemResQueue = "ClearCartItemResQueue";
        public const string ClearCartItemReqQueue = "ClearCartItemReqQueue";

    }
}

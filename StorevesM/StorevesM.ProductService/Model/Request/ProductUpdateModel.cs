namespace StorevesM.ProductService.Model.Request
{
    public class ProductUpdateModel
    {
        public string? Name { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public string? Describe { get; set; }

        public int? CategoryId { get; set; }
    }
}

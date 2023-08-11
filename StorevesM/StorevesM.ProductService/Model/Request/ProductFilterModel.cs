namespace StorevesM.ProductService.Model.Request
{
    public class ProductFilterModel
    {
        public string? Name { get; set; }

        public double? PriceFrom { get; set; }

        public double? PriceTo { get; set; }

        public int? CategoryId { get; set; }

    }
}

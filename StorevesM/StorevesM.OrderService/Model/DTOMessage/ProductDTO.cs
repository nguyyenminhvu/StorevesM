namespace StorevesM.OrderService.Model.DTOMessage
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Describe { get; set; }

        public CategoryDTO Category { get; set; }
    }
}

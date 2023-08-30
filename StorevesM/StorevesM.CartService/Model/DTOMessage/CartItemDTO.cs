namespace StorevesM.CartService.Model.DTOMessage
{
    public class CartItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }
    }
}

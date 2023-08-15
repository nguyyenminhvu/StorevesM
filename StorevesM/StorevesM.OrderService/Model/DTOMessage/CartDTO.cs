namespace StorevesM.OrderService.Model.DTOMessage
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }
}

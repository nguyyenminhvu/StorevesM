using StorevesM.CartService.Model.DTOMessage;

namespace StorevesM.CartService.Model.View
{
    public class CartItemViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }
    }
}

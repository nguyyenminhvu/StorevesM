namespace StorevesM.CartService.Model.View
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

    }
}

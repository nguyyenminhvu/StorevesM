namespace StorevesM.CartService.Model.Request
{
    public class CartUpdateModel
    {
        public int CartId { get; set; }
        public List<CartItemUpdateModel> CartItems { get; set; }
    }
}

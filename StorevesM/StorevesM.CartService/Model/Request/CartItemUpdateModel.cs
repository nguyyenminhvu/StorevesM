namespace StorevesM.CartService.Model.Request
{
    public class CartItemUpdateModel
    {
        public int ProductId { get; set; }

        public double  Price { get; set; }

        public int Quantity { get; set; }

    }
}

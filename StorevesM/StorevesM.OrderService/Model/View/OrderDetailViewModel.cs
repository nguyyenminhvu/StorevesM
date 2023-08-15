namespace StorevesM.OrderService.Model.View
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreateAt { get; set; }
    }
}

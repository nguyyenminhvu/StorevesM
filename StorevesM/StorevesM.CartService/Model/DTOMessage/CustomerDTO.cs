namespace StorevesM.CartService.Model.DTOMessage
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string? Address { get; set; }

        public DateTime CreateAt { get; set; }
    }
}

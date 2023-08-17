namespace StorevesM.CustomerService.Model.Request
{
    public class CustomerCreateModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string? Address { get; set; }

    }
}

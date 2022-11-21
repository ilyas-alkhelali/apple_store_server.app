namespace Apple_Store_Db_Server.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string About { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime DateOfAdd { get; set; } = DateTime.Now;

        public decimal? Rating { get; set; }
        public int NumberOfRated { get; set; }
        public decimal? SumOfRates { get; set; }
        
        public List<Order>? Orders { get; set; } = new List<Order>();


    }
}

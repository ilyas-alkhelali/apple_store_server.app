using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public string About { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTime DateOfAdd { get; set; } = DateTime.Now;

        [Range(0, 5, ErrorMessage = "Invalid rating")]
        public double Rating { get; set; } = 4.5;

        public int NumberOfRated { get; set; }
        public double SumOfRates { get; set; }
        
        public List<Order>? Orders { get; set; } = new List<Order>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.Models
{
    public class BoughtProduct
    {
        [Key]
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public int Capacity { get; set; }

        public Product Product { get; set; }

        public Order Order { get; set; }
        public Guid OrderId { get; set; }
    }
}

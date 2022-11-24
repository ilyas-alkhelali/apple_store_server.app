using Apple_Store_Db_Server.Dto;
using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "field required")]
        public double Cost { get; set; }

        public User? User { get; set; }
        public Guid? UserId { get; set; }

        public List<BoughtProduct> Products { get; set; } = new List<BoughtProduct>();
    }
}

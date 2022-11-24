using Apple_Store_Db_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apple_Store_Db_Server.Dto
{
    [NotMapped]
    public class OrderDto
    {
        [Required(ErrorMessage = "field required")]
        public Product Product{ get; set; }

        [Required(ErrorMessage = "field required")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "field required")]
        public int Capacity { get; set; }
    }
}

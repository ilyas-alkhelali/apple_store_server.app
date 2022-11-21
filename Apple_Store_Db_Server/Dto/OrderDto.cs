using Apple_Store_Db_Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apple_Store_Db_Server.Dto
{
    [NotMapped]
    public class OrderDto
    {
        public Product Product{ get; set; } 
        public int Amount { get; set; }
        public int Capacity { get; set; }
    }
}

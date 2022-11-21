using Apple_Store_Db_Server.Dto;

namespace Apple_Store_Db_Server.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public decimal Cost { get; set; } = decimal.Zero;

        public User? User { get; set; }
        public Guid? UserId { get; set; }

        public List<OrderDto> Products { get; set; } = new List<OrderDto>();
    }
}

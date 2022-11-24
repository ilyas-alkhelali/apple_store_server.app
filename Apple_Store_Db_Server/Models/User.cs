using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }

        public List<Order>? Orders { get; set; } = new List<Order>();
    }
}

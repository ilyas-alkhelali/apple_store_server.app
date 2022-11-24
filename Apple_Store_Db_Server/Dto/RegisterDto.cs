using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.ViewModel
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "field required")]
        [Range(1, 150, ErrorMessage = "Invalid age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "field required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "minimum 3 symbols and maximun 20")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        [EmailAddress(ErrorMessage = "InCorrect Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "minimum 8 symbols and maximun 50")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty; 
    }
}

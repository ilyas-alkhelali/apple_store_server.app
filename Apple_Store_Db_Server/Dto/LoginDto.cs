using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.ViewModel
{
    public class LoginDto
    {
        [Required(ErrorMessage = "field required")]
        [EmailAddress(ErrorMessage = "InCorrect Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "minimum 8 symbols and maximun 50")]
        public string Password { get; set; } = string.Empty;
    }
}

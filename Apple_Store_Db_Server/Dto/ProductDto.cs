using System.ComponentModel.DataAnnotations;

namespace Apple_Store_Db_Server.ViewModels
{
    public class ProductDto
    {
        [Required(ErrorMessage = "field required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "minimum 2 symbols and maximun 50")]
        public string TypeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "minimum 2 symbols and maximun 50")]
        public string Version { get; set; } = string.Empty;

        [Required(ErrorMessage = "field required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "field required")]
        public int Amount { get; set; } = 0;

        [Required(ErrorMessage = "field required")]
        public string About { get; set; } = string.Empty;
       
    }
}

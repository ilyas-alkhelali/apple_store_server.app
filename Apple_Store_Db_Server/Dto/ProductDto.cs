namespace Apple_Store_Db_Server.ViewModels
{
    public class ProductDto
    {
        public string TypeName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public int Amount { get; set; } = 0;
        public string About { get; set; } = string.Empty;
       
    }
}

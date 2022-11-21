namespace Apple_Store_Db_Server.ViewModel
{
    public class RegisterDto
    {
        public int Age { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty; 
    }
}

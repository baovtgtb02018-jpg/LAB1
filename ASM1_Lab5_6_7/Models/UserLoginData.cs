namespace ASM.Models
{
    public class UserLoginData
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool ThanhCong { get; set; }
        public string GhiChu { get; set; } = string.Empty;
    }
}

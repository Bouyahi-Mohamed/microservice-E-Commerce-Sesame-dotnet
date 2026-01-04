namespace identity_service.DTOs
{
    public class LoginRequestDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class SignupRequestDto
    {
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string token { get; set; } = string.Empty;
        public int userId { get; set; }
        public int customerId { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }
}

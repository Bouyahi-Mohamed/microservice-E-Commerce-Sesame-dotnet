using System;

namespace projet_API.DTOs
{
    public class LoginRequestDto
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class SignupRequestDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }

    public class AuthResponseDto
    {
        public string token { get; set; }
        public int userId { get; set; }
        public int customerId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}

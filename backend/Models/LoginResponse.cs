using System;

namespace projet_1.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public int NameIdentifier { get; set; }
        public Role Role { get; set; }
    }
}

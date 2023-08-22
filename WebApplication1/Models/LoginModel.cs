using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class LoginModel
    {
        public List<LoginInfo>? allAccts {  get; set; }
    }

    public class LoginInfo
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Active { get; set; }
    }
}

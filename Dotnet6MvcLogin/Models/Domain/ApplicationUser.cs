using Microsoft.AspNetCore.Identity;

namespace Dotnet6MvcLogin.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserStatus { get; set; }
        public DateTime RegistrationTime { get; set; }

        public DateTime? LoginTime { get; set; }
    }
}

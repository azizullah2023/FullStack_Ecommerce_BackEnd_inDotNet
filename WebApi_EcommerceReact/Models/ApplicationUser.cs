using Microsoft.AspNetCore.Identity;

namespace WebApi_EcommerceReact.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}

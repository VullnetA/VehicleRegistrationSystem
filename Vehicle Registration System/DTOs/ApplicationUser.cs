using Microsoft.AspNetCore.Identity;

namespace Vehicle_Registration_System.DTOs
{
    public class ApplicationUser : IdentityUser
    {
        public int? OwnerId { get; set; }
    }
}

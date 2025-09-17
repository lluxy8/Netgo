using Microsoft.AspNetCore.Identity;

namespace Netgo.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactInfo { get; set; }
        public string Location { get; set; }
        public bool VerifiedSeller { get; set; } = false;
        public string ProfilePictureURL { get; set; }
    }
}

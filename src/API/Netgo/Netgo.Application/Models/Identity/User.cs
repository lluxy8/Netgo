namespace Netgo.Application.Models.Identity
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NormalizedUserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string ContactInfo { get; set; }
        public string Location { get; set; }
        public bool VerifiedSeller { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}

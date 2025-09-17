namespace Netgo.Application.Models.Identity
{
    public class RegistrationRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ContactInfo { get; set; }
        public required string Location { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string ProfilePicture { get; set; } = string.Empty;
    }
}

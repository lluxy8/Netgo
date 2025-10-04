using Microsoft.AspNetCore.Http;

namespace Netgo.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ContactInfo { get; set; }
        public required IFormFile ProfilePicture { get; set; }
    }
}

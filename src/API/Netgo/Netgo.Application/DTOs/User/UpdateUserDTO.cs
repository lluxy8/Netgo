using Microsoft.AspNetCore.Http;

namespace Netgo.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactInfo { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}

namespace Netgo.Application.DTOs.User
{
    public class ResetUserPasswordDTO
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

namespace Netgo.Application.DTOs.User
{
    public class ResetUserPasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

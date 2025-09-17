namespace Netgo.Application.Models.Identity
{
    public class ConfirmEmailRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}

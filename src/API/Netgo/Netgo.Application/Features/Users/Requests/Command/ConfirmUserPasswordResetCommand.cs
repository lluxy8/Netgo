using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.User;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class ConfirmUserPasswordResetCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}

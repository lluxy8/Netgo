using MediatR;
using Netgo.Application.Common;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class LogoutUserCommand : IRequest<Result>
    {
        public required string UserId { get; set; }
    }
}

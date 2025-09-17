using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class LoginUserCommand : IRequest<Result<AuthResponse>>
    {
        public AuthRequest AuthRequest { get; set; }
    }
}

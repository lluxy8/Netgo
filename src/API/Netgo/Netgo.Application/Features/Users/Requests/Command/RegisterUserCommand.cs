using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class RegisterUserCommand : IRequest<Result<RegistrationResponse>>
    {
        public required RegistrationRequest RegistrationRequest { get; set; }
    }
}

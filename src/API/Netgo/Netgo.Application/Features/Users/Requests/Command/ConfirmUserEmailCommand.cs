using MediatR;
using Netgo.Application.Common;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class ConfirmUserEmailCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}

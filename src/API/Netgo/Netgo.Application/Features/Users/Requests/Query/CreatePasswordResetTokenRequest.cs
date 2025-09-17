using MediatR;
using Netgo.Application.Common;

namespace Netgo.Application.Features.Users.Requests.Query
{
    public class CreatePasswordResetTokenQuery : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
}

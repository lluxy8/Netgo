using MediatR;
using Netgo.Application.Common;

namespace Netgo.Application.Features.Users.Requests.Query
{
    public class CreateUserEmailTokenQuery : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }
}

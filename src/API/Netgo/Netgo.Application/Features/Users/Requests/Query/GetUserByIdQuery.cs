using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Requests.Query
{
    public class GetUserByIdQuery : IRequest<Result<User>>
    {
        public Guid Id { get; set; }
    }
}

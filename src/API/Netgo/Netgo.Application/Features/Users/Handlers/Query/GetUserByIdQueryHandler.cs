using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Users.Requests.Query;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Handlers.Query
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
    {
        private readonly IUserService _userService;

        public GetUserByIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUser(request.Id.ToString());
            return Result<User>.Success(user);
        }
    }
}

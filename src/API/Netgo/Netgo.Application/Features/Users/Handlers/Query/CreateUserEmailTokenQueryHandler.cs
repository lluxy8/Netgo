using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Features.Users.Requests.Query;

namespace Netgo.Application.Features.Users.Handlers.Query
{
    public class CreateUserEmailTokenQueryHandler : IRequestHandler<CreateUserEmailTokenQuery, Result<string>>
    {
        private readonly IUserService _userService;
        public CreateUserEmailTokenQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<string>> Handle(CreateUserEmailTokenQuery request, CancellationToken cancellationToken)
        {
            var token =  await _userService.GenerateEmailConfirmationToken(request.Id.ToString());
            return Result<string>.Success(token);
        }
    }
}

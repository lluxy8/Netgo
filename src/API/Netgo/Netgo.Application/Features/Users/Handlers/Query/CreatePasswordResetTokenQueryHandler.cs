using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Features.Users.Requests.Query;

namespace Netgo.Application.Features.Users.Handlers.Query
{
    internal class CreatePasswordResetTokenQueryHandler : IRequestHandler<CreatePasswordResetTokenQuery, Result<string>>
    {
        private readonly IUserService _userService;

        public CreatePasswordResetTokenQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<string>> Handle(CreatePasswordResetTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await _userService.GeneratePasswordResetToken(request.Id.ToString(), request.Password);
            var res = Result<string>.Success(token, 200);
            return res;
        }
    }
}

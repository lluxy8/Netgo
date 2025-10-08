using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Features.Users.Requests.Command;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class LogOutUserCommandHandler : IRequestHandler<LogoutUserCommand, Result>
    {
        private readonly IUserService _userService;

        public LogOutUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.Logout(request.UserId);
            return Result.Success();
        }
    }
}

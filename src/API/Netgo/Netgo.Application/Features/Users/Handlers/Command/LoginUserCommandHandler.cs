using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Features.Users.Requests.Command;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
    {
        private readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {

            var result = await _authService.Login(request.AuthRequest);
            return Result<AuthResponse>.Success(result);
        }
    }
}

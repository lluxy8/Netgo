using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Features.Users.Requests.Command;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class ConfirmUserPasswordResetCommandHandler
        : IRequestHandler<ConfirmUserPasswordResetCommand, Result>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public ConfirmUserPasswordResetCommandHandler(
            IUserService userService, 
            IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(ConfirmUserPasswordResetCommand request, CancellationToken cancellationToken)
        {
            await _userService.ConfirmPasswordReset(
                request.UserId.ToString(),
                request.NewPassword,
                request.Token);

            var email = await _userService.GetUserEmail(request.UserId.ToString());
            await _emailService.Send(new Models.Email
            {
                To = email,
                Body = """
                    Your password has been successfully reset. If this wasn’t you please contact us: support@netgo.com
                """,               
                Subject = "Password Reset Successful"
            });

            return Result.Success();
        }
    }
}

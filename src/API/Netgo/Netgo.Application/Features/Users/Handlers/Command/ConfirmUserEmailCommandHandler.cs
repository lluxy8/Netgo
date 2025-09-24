using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Users.Requests.Command;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class ConfirmUserEmailCommandHandler : IRequestHandler<ConfirmUserEmailCommand, Result>
    {
        private readonly IUserService userService;

        public ConfirmUserEmailCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<Result> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var isVerified = await userService.ConfirmEmail(request.UserId.ToString(), request.Token);

            if(!isVerified)
                throw new BadRequestException("Email confirmation failed");

            return Result.Success();
        }

    }
}

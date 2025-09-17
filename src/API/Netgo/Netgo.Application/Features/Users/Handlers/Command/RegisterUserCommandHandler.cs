using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.DTOs.Product.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Users.Requests.Command;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegistrationResponse>>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<Result<RegistrationResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegistrationRequestValidator();

            var validationResult = await validator
                .ValidateAsync(request.RegistrationRequest,
                cancellationToken);

            if (validationResult.IsValid)
                throw new ValidationException(validationResult);

            var result = await _authService.Register(request.RegistrationRequest);
            return Result<RegistrationResponse>.Success(result);
        }
    }
}

using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.DTOs.User.Validators;
using Netgo.Application.Exceptions;
using Netgo.Application.Features.Users.Requests.Command;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Features.Users.Handlers.Command
{
    public class UpdateUsertCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;   

        public UpdateUsertCommandHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserDTOValidator();

            var validationResult = await validator.ValidateAsync(
                request.UpdateUsertDTO, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var updatedUser = _mapper.Map<User>(request.UpdateUsertDTO);
            await _userService.UpdateUser(updatedUser);

            return Result.Success();
        }
    }
}

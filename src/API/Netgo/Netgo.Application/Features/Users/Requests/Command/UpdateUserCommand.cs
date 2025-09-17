using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Product;
using Netgo.Application.DTOs.User;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public UpdateUserDTO UpdateUsertDTO { get; set; }
    }
}

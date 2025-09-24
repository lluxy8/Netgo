using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Netgo.Application.Common;
using Netgo.Application.DTOs.User;
using Netgo.Application.Features.Users.Requests.Command;
using Netgo.Application.Features.Users.Requests.Query;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [EnableRateLimiting("sliding")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var request = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO user)
        {
            var request = new UpdateUserCommand { UpdateUsertDTO = user };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Netgo.Application.DTOs.Auth;
using Netgo.Application.Features.Users.Requests.Command;
using Netgo.Application.Features.Users.Requests.Query;
using Netgo.Application.Models.Identity;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableRateLimiting("sliding")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest auth)
        {
            var request = new LoginUserCommand { AuthRequest = auth };
            var result = await _mediator.Send(request);

            if(result.IsSuccess)
            {
                SetAuthCookie(result.Data.Token);

                return Ok(new AuthResponseDTO
                {
                    Id = result.Data.Id,
                    Token = result.Data.Token
                });
            }

            return new ResultActionResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest register)
        {
            var request = new RegisterUserCommand { RegistrationRequest = register };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPost("passwordresettoken/{id}")]
        public async Task<IActionResult> ResetPasswordToken(Guid id, [FromBody] string oldPassword)
        {
            var request = new CreatePasswordResetTokenQuery { Id = id, Password = oldPassword };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpPost("passwordreset/{id}")]
        public async Task<IActionResult> ResetPasswordConfirm(
            Guid id,
            [FromQuery] string token,
            [FromBody] string newPassword)
        {
            var request = new ConfirmUserPasswordResetCommand { UserId = id, NewPassword = newPassword, Token = token };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }


        [Authorize]
        [HttpPost("emailconfirmtoken/{id}")]
        public async Task<IActionResult> EmailConfirmToken(Guid id)
        {
            var request = new CreateUserEmailTokenQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [Authorize]
        [HttpPost("emailconfirm/{id}")]
        public async Task<IActionResult> Emailconfirm(Guid id, [FromQuery] string token)
        {
            var request = new ConfirmUserEmailCommand { UserId = id, Token = token, };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(24),
                Path = "/"
            };

            Response.Cookies.Append("authToken", token, cookieOptions);
        }
    }
}

using Netgo.Application.Models.Identity;

namespace Netgo.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest request);
        string? GetUserIdByToken(string token);
        Task<RegistrationResponse> Register(RegistrationRequest request);
    }
}
    
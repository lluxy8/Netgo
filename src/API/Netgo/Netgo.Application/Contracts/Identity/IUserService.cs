using Netgo.Application.DTOs.User;
using Netgo.Application.Models.Identity;

namespace Netgo.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(string userId);
        Task<bool> UserExists(string userId);
        Task ConfirmPasswordReset(string userId, string newPassword, string token);
        Task<bool> CheckPassword(string userId, string password);
        Task<string> GeneratePasswordResetToken(string userId, string oldPassword);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string userId);
        Task<bool> UpdateVerifiedUserSellerState(string userId, bool verified);
        Task<bool>ConfirmEmail(string userId, string token);
        Task<string> GenerateEmailConfirmationToken(string userId);
        Task<string> GetUserEmail(string userId);
    }
}

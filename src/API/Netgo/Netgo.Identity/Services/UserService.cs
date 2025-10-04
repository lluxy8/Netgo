using Microsoft.AspNetCore.Identity;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Exceptions;
using Netgo.Application.Models.Identity;

namespace Netgo.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> GetUser(string userId)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            return MapUser(user);
        }

        public Task<List<User>> GetUsers()
        {
            var users = _userManager.Users.ToList();
            if (users == null || users.Count == 0)
                throw new NotFoundException("Users", string.Empty);

            return Task.FromResult(users.Select(MapUser).ToList());
        }

        public async Task<bool> UserExists(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await GetUserOrThrowIfNotFound(user.Id);

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.ContactInfo = user.ContactInfo;
            existingUser.Location = user.Location;
            existingUser.ProfilePictureURL = user.ProfilePictureURL;

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await GetUserOrThrowIfNotFound(userId);

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateVerifiedUserSellerState(string userId, bool verified)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            user.VerifiedSeller = verified;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }

        public async Task<string> GenerateEmailConfirmationToken(string userId)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task ConfirmPasswordReset(string userId, string newPassword, string token)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if(!result.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<string> GeneratePasswordResetToken(string userId, string oldPassword)
        {
            var user = await GetUserOrThrowIfNotFound(userId);

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, oldPassword);

            if (!isOldPasswordValid)
                throw new BadRequestException("Invalid credentials");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }


        public async Task<bool> CheckPassword(string userId, string password)
        {
            var user = await GetUserOrThrowIfNotFound(userId);

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<string> GetUserEmail(string userId)
            => (await GetUser(userId)).Email;

        public async Task Logout(string userId)
        {
            var user = await GetUserOrThrowIfNotFound(userId);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        private static User MapUser(ApplicationUser user)
            => new User
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ContactInfo = user.ContactInfo,
                VerifiedSeller = user.VerifiedSeller,
                EmailConfirmed = user.EmailConfirmed,
                Location = user.Location,
                NormalizedUserName = user.NormalizedUserName,
                ProfilePictureURL = user.ProfilePictureURL
            };

        private async Task<ApplicationUser> GetUserOrThrowIfNotFound(string userId)
            => await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);
    }
}

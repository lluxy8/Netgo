using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Exceptions;
using Netgo.Application.Models.Identity;
using Netgo.Identity.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Netgo.Identity.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<
            ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) 
                ?? throw new BadRequestException("Invalid email or password");

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Invalid email or password");
            }


            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            AuthResponse response = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            return response;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            string username = GetUsername(request.FirstName, request.LastName);
            var existingUser = await _userManager.FindByNameAsync(username);

            while (existingUser != null)
            {
                username = GetUsername(request.FirstName, request.LastName);
                existingUser = await _userManager.FindByNameAsync(username);
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                ContactInfo = request.ContactInfo,
                Location = request.Location,
                LastName = request.LastName,
                UserName = username,
                ProfilePictureURL = request.ProfilePicture,
                EmailConfirmed = true
            };

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return new RegistrationResponse() { UserId = user.Id };
                }
                else
                {
                    throw new BadRequestException(
                        string.Join(", ", result.Errors.Select(e => e.Description)));      
                }
            }
            else
            {
                throw new BadRequestException($"Email {request.Email} already exists.");
            }
        }

        private static string GetUsername(string firstName, string lastName)
        {
            string usrname = $"{firstName}{lastName}{RandomDigitGenerator.Generate(12)}";
            return usrname;
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

    }
}

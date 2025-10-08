using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Exceptions;
using Netgo.Application.Models.Identity;
using Netgo.Identity.Common;
using Newtonsoft.Json.Linq;
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
        private readonly IFileService _fileService;

        public AuthService(UserManager<
            ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IFileService fileService            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _fileService = fileService;
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

            await _userManager.UpdateSecurityStampAsync(user);

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            return new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
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
                ProfilePictureURL = "https://static.vecteezy.com/system/resources/thumbnails/004/511/281/small_2x/default-avatar-photo-placeholder-profile-picture-vector.jpg",
                EmailConfirmed = true
            };

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);

                    var pp = await _fileService.SaveFileAsync(user.Id, request.ProfilePicture);
                    user.ProfilePictureURL = pp;
                    await _userManager.UpdateAsync(user);

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
            string usrname = $"{firstName}{lastName}{RandomDigitGenerator.Generate(5)}";
            return usrname;
        }

        public string? GetUserIdByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                //ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = key
            };

            var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
            var claims = principal.Claims;
            var idClaim = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Uid);
            return idClaim?.Value;
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

            var securityStamp = await _userManager.GetSecurityStampAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id),
                new Claim(CustomClaimTypes.SecurityStamp, securityStamp) 
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

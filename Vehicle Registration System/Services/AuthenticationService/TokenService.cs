using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vehicle_Registration_System.Models;
using Microsoft.Extensions.Configuration;
using Vehicle_Registration_System.DTOs;

namespace Vehicle_Registration_System.Services.AuthenticationService
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private const int ExpirationMinutes = 60;

        public async Task<string> CreateToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var roles = await _userManager.GetRolesAsync(user);

            var token = CreateJwtToken(
                CreateClaims(user, roles),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
            new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: expiration,
                claims: claims,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(ApplicationUser user, IList<string> roles)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("OwnerId", user.OwnerId.ToString() ?? "")
            }.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))).ToList();
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            return new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}

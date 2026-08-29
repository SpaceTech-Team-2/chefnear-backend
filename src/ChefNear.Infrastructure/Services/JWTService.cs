using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChefNear.Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        private readonly JwtSettings _jwtSettings;

        public JWTService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtSecurityToken> CreateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.Role, user.Role.ToString()),
    new Claim("FName", user.FirstName ?? string.Empty),
    new Claim("LName", user.LastName ?? string.Empty),
    new Claim("PhoneNumber", user.PhoneNumber ?? string.Empty)
};

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds);
        }
    }
}
using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChefNear.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ChefNearDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(
            ChefNearDbContext context,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshTokenString();

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = HashToken(refreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays > 0
                    ? _jwtSettings.RefreshTokenDurationInDays
                    : 7) // Default 7 days
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null)
                return null;

            if (!storedToken.IsActive)
                return null;

            return storedToken.User;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null)
                return;

            storedToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private string GenerateRefreshTokenString()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
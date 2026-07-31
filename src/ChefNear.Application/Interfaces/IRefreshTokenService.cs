using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(User user);

        Task<User?> ValidateRefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}

using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories;

internal class DeviceTokenRepo : GenericRepository<DeviceToken>, IDeviceTokenRepo
{
    private readonly ChefNearDbContext _db;

    public DeviceTokenRepo(ChefNearDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public async Task<IReadOnlyList<string>> GetByUserIdAsync(string userId)
    {
        return await _db.DeviceTokens
            .Where(t => t.UserId == userId && t.IsActive)
            .Select(t => t.Token)
            .ToListAsync();
    }

    public async Task<DeviceToken> GetByTokenAsync(string token)
    {
        return await _db.DeviceTokens
            .FirstOrDefaultAsync(t => t.Token == token);   
    }
}

using ChefNear.Domain.Entities;

namespace ChefNear.Application.Common.Persistence.Interfaces;

public interface IDeviceTokenRepo : IGenericRepository<DeviceToken>
{
    Task<DeviceToken> GetByTokenAsync(string token);
    Task<IReadOnlyList<string>> GetByUserIdAsync(string userId);
}

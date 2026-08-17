using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface INotificationRepo : IGenericRepository<Notification>
    {
        Task<IReadOnlyList<Notification>> GetUserNotificationsPaginatedAsync(
            string userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}

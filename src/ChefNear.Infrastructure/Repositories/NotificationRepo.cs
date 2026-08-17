using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Infrastructure.Repositories
{
    public class NotificationRepo : GenericRepository<Notification>, INotificationRepo
    {
        private readonly ChefNearDbContext _dbContext;

        public NotificationRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Notification>> GetUserNotificationsPaginatedAsync(
            string userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}

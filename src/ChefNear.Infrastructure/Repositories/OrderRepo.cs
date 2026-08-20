using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories
{
    public class OrderRepo : GenericRepository<Order>, IOrderRepo
    {
        private readonly ChefNearDbContext _db;

        public OrderRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<Order?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _db.Orders
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetByIdWithTrackingAsync(Guid id)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Dish)
                .Include(o => o.Chef)
                .Include(o => o.Client)
                .Include(o => o.DeliveryAddress)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersWithDetails(string chefId, int pageNo, int pageSize, bool active = true)
        {
            return await _db.Orders
                .Include(o => o.Client)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Dish)
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize)
                .Where(o => o.Status < OrderStatus.Delivered && o.DeliveredAt == null && o.CanceledAt == null)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}

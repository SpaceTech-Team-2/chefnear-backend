using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
                .Include(o => o.Dish)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}

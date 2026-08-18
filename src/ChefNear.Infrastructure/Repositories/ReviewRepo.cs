using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Repositories
{
    public class ReviewRepo : GenericRepository<Review>, IReviewRepo
    {
        private readonly ChefNearDbContext dbContext;
        public ReviewRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Review>> GetByDishIdAsync(
    Guid dishId,
    CancellationToken cancellationToken)
        {
            return await dbContext.Reviews
                .Where(x => x.DishId == dishId)
                .ToListAsync(cancellationToken);
        }
    }
}

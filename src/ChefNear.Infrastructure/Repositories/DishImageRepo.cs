using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Repositories
{
    public class DishImageRepo : GenericRepository<DishImage>, IDishImageRepo
    {
        public DishImageRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}

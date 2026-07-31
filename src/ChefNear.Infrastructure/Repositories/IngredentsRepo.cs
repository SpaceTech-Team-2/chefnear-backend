using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Repositories
{
    public class IngredentsRepo : GenericRepository<Ingredient>, IIngredientsRepo
    {
        public IngredentsRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}

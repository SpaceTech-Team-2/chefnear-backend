using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Repositories
{
    public class DisputeRepo : GenericRepository<Dispute>, IDisputeRepo
    {
        public DisputeRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}

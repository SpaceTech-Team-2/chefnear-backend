using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface IReviewRepo : IGenericRepository<Review>
    {
        Task<List<Review>> GetByDishIdAsync(
       Guid dishId,
       CancellationToken cancellationToken);
    }
}

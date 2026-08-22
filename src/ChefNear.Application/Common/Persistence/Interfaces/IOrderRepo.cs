using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface IOrderRepo : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithDetailsAsync(Guid id);
        Task<Order> GetByIdWithTrackingAsync(Guid id);
        Task<IReadOnlyList<Order>> GetOrdersWithDetails(string chefId, int pageNo, int pageSize, bool active = true);
    }
}

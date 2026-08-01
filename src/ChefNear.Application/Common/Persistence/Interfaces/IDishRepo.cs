using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface IDishRepo : IGenericRepository<Dish>
    {
        Task<List<Dish>> GetNearbyDishesAsync();
        Task<Dish?> GetDishDetailsAsync(Guid id);
    }
}

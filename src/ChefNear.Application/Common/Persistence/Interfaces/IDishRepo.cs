using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface IDishRepo : IGenericRepository<Dish>
    {
        Task<List<Dish>> GetNearbyDishesAsync();
        Task<Domain.Entities.Dish?> GetByIdWithDetailsAsync(Guid id);
        Task<List<Dish>> GetDishesByChefId(string chefId);
        Task<Dish?> GetDishDetailsAsync(Guid id);
    }
}

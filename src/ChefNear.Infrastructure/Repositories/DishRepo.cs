using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories;

public class DishRepo : GenericRepository<Dish>, IDishRepo
{
    private readonly ChefNearDbContext _context;

    public DishRepo(ChefNearDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Dish>> GetDishesByChefId(string chefId)
    {
        return await _context.Dishes
            .Include(d => d.Chef)
            .Include(d => d.Images)
            .Where(d => d.ChefId == chefId)
            .ToListAsync();
    }

    public async Task<List<Dish>> GetNearbyDishesAsync()
    {
        return await _context.Dishes
            .Include(d => d.Images)
            .Include(d => d.Chef)
                .ThenInclude(c => c.KitchenAddress)
            .Where(d => !d.IsDeleted)
            .ToListAsync();
    }
    public async Task<Domain.Entities.Dish?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Dishes
            .Include(d => d.Category)
            .Include(d => d.Chef)
            .Include(d => d.Images)
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<Dish?> GetDishDetailsAsync(Guid id)
    {
        return await _context.Dishes
            .Include(d => d.Category)
            .Include(d => d.Chef)
            .Include(d => d.Images)
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
    }
}
using System.Collections.Concurrent;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChefNearDbContext _dbContext;
    private bool _disposed;

    private IUserRepo? userRepo;
    private IAdressRepo? adressRepo;
    private ICategoryRepo? categoryRepo;
    private IDishRepo? dishRepo;
    private IDishImageRepo? dishImageRepo;
    private IDisputeRepo? disputeRepo;
    private IIngredientsRepo? ingredientsRepo;
    private INotificationRepo? notificationRepo;
    private IOrderRepo? orderRepo;
    private IReviewRepo? reviewRepo;
    private IPaymentRepo? paymentRepo;

    public UnitOfWork(ChefNearDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUserRepo Users => userRepo ??= new UserRepo(_dbContext);
    public IAdressRepo adresses => adressRepo ??= new AddressRepo(_dbContext);
    public ICategoryRepo categories => categoryRepo ??= new CategoryRepo(_dbContext);
    public IDishRepo dishes => dishRepo ??= new DishRepo(_dbContext);
    public IDishImageRepo dishImages => dishImageRepo ??= new DishImageRepo(_dbContext);
    public IDisputeRepo disputes => disputeRepo ??= new DisputeRepo(_dbContext);
    public IIngredientsRepo ingredients => ingredientsRepo ??= new IngredentsRepo(_dbContext);
    public INotificationRepo notifications => notificationRepo ??= new NotificationRepo(_dbContext);
    public IOrderRepo Orders => orderRepo ??= new OrderRepo(_dbContext);
    public IReviewRepo Reviews => reviewRepo ??= new ReviewRepo(_dbContext);
    public IPaymentRepo Payments => paymentRepo ??= new PaymentRepo(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            _disposed = true;
        }
    }
}
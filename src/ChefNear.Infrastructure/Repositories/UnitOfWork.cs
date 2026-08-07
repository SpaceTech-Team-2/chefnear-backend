using System.Collections.Concurrent;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChefNear.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChefNearDbContext _dbContext;
    private bool _disposed;

    private IUserRepo? userRepo;
    private IChefRepo? chefRepo;
    private IClientRepo? clientRepo;
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
    private IWalletRepo? walletRepo;
    private IWalletTransactionRepo? walletTransactionRepo;

    public UnitOfWork(ChefNearDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUserRepo Users => userRepo ??= new UserRepo(_dbContext);
    public IChefRepo Chefs => chefRepo ??= new ChefRepo(_dbContext);
    public IClientRepo Clients => clientRepo ??= new ClientRepo(_dbContext);
    public IAdressRepo Adresses => adressRepo ??= new AddressRepo(_dbContext);
    public ICategoryRepo Categories => categoryRepo ??= new CategoryRepo(_dbContext);
    public IDishRepo Dishes => dishRepo ??= new DishRepo(_dbContext);
    public IDishImageRepo DishImages => dishImageRepo ??= new DishImageRepo(_dbContext);
    public IDisputeRepo Disputes => disputeRepo ??= new DisputeRepo(_dbContext);
    public IIngredientsRepo Ingredients => ingredientsRepo ??= new IngredentsRepo(_dbContext);
    public INotificationRepo Notifications => notificationRepo ??= new NotificationRepo(_dbContext);
    public IOrderRepo Orders => orderRepo ??= new OrderRepo(_dbContext);
    public IReviewRepo Reviews => reviewRepo ??= new ReviewRepo(_dbContext);
    public IPaymentRepo Payments => paymentRepo ??= new PaymentRepo(_dbContext);
    public IWalletRepo Wallets { get => walletRepo ??= new WalletRepo(_dbContext); set => walletRepo = value; }
    public IWalletTransactionRepo Transactions { get => walletTransactionRepo ??= new WalletTransactionRepo(_dbContext); set => walletTransactionRepo = value; }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

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
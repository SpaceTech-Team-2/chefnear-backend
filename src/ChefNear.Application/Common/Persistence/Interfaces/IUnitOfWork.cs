using ChefNear.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChefNear.Application.Common.Persistence.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IAdressRepo Adresses { get; }
    ICategoryRepo Categories { get; }
    IDishImageRepo DishImages { get; }
    IDishRepo Dishes { get; }
    IDisputeRepo Disputes { get; }
    IIngredientsRepo Ingredients  { get; }
    IOrderRepo Orders  { get; }
    IReviewRepo Reviews { get; }
    IDeviceTokenRepo DeviceTokens { get; }
    IUserRepo Users { get; }
    IChefRepo Chefs { get; }
    IClientRepo Clients { get; }
    IPaymentRepo Payments { get; }
    INotificationRepo Notifications  { get; }
    IWalletRepo Wallets { get; set; }
    IWalletTransactionRepo Transactions { get; set; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
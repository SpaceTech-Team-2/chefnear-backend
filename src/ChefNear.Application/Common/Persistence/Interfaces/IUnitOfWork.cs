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
    IUserRepo Users { get; }
    IPaymentRepo Payments { get; }
    INotificationRepo Notifications  { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
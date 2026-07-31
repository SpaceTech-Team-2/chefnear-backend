using ChefNear.Domain.Common;

namespace ChefNear.Application.Common.Persistence.Interfaces
{

public interface IUnitOfWork : IDisposable
{
        IAdressRepo adresses { get; }
        ICategoryRepo categories { get; }
        IDishImageRepo dishImages { get; }
        IDishRepo dishes { get; }
        IDisputeRepo disputes { get; }
        IIngredientsRepo ingredients  { get; }
        IOrderRepo Orders  { get; }
        IReviewRepo Reviews { get; }
        IUserRepo Users { get; }
        IPaymentRepo Payments { get; }
        INotificationRepo notifications  { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

}
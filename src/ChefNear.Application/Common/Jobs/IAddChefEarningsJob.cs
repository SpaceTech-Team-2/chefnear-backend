namespace ChefNear.Application.Common.Jobs;

public interface IAddChefEarningsJob
{
    Task ExecuteAsync(Guid paymentId, string chefId);
}

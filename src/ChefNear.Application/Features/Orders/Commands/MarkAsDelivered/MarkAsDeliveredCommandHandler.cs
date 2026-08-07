using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using Hangfire;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsDelivered;

public class MarkAsDeliveredCommandHandler(
    IUnitOfWork unitOfWork,
    IBackgroundJobClient backgroundJobClient
,
    IAddChefEarningsJob addChefEarningsJob) : IRequestHandler<MarkAsDeliveredCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IBackgroundJobClient backgroundJobClient = backgroundJobClient;
    private readonly IAddChefEarningsJob addChefEarningsJob = addChefEarningsJob;

    public async Task<Result> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders
            .GetAsync(o => o.Id == request.OrderId && o.ClientId == request.Client.Id, nameof(Order.Payment));

        var payment = order.Payment;

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.ReadyForDelivery)
            return Result.Failure(DomainErrors.Order.OrderMustBeReady);

        if (payment == null)
            return Result.Failure(Error.Failure("OrderNotPaid", "This order not paid yet to mark it as Delivered."));

        order.MarkAsDelivered();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        backgroundJobClient.Enqueue(() => addChefEarningsJob.ExecuteAsync(payment.Id, order.ChefId));
        // TODO: add subsequent job after `addChefEarningsJob` to Notify Chef.

        // TODO: Push Notification 

        return Result.Success();
    }
}

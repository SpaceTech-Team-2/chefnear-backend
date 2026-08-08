using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.MarkAsReady;

public class MarkOrderAsReadyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<MarkOrderAsReadyCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result> Handle(MarkOrderAsReadyCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null) 
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Preparing)
            return Result.Failure(DomainErrors.Order.OrderMustBePreparing);

        order.MarkAsReady(request.DeliveryFee, request.EstimatedDeliveryTime);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Publish an event or notification that the order is ready for delivery
        
        return Result.Success();
    }
}

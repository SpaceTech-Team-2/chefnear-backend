using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.StartPreparing;

public class StartOrderPreparingCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<StartOrderPreparingCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result> Handle(StartOrderPreparingCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Accepted)
            return Result.Failure(DomainErrors.Order.OrderMustBeAccepted);

        order.Status = OrderStatus.Preparing;
        order.EstimatedCookingTime = request.EstimatedCookingTime;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Publish an event or notification that the order preparation has started
        return Result.Success();
    }
}

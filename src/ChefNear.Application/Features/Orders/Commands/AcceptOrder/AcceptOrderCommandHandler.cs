using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.AcceptOrder;

public class AcceptOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AcceptOrderCommand, Result>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetAsync(o => o.Id == request.OrderId && o.ChefId == request.Chef.Id);

        if (order == null)
            return Result.Failure(DomainErrors.Order.OrderNotFound);

        if (order.Status != OrderStatus.Confirmed)
            return Result.Failure(DomainErrors.Order.OrderMustBeConfirmed);

        order.Status = OrderStatus.Accepted;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Publish an event or notification that the order has been accepted
        return Result.Success();
    }
}

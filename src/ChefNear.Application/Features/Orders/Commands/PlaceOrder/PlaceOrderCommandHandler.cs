using AutoMapper;
using ChefNear.Application.Common.Payments;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Domain.Exceptions;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPaymentGatewayFactory paymentGatewayFactory,
    ILogger<PlaceOrderCommandHandler> logger) : IRequestHandler<PlaceOrderCommand, Result<PlaceOrderResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGatewayFactory _paymentGatewayFactory = paymentGatewayFactory;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlaceOrderCommandHandler> _logger = logger;

    public async Task<Result<PlaceOrderResponse>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var existingPayment = await _unitOfWork.Payments.GetAsync(p => p.IdempotencyKey == request.IdempotencyKey.ToString());

        if (existingPayment != null)
            return DomainErrors.Payment.IdempotencyKeyAlreadyExists;

        if (request.DeliveryAddressId == null && request.DeliveryAddress == null)
            return DomainErrors.Order.DeliveryAddressNotProvided;

        var dishIds = request.Items.Select(i => i.DishId).Distinct().ToList();
        var dishes = await _unitOfWork.Dishes.FindAsync(d => dishIds.Contains(d.Id));

        if (dishes.Count != dishIds.Count)
            return DomainErrors.Dish.DishNotFound;

        if (dishes.Any(d => d.Status != DishStatus.Available))
            return DomainErrors.Dish.DishUnavailable;

        var chefIds = dishes.Select(d => d.ChefId).Distinct().ToList();
        if (chefIds.Count > 1)
            return DomainErrors.Order.MultipleChefsNotAllowed;

        decimal totalAmount = 0;
        var orderItems = new List<OrderItem>();
        var itemSummaries = new List<OrderItemSummary>();

        foreach (var item in request.Items)
        {
            var dish = dishes.First(d => d.Id == item.DishId);
            totalAmount += dish.Price * item.Quantity;

            orderItems.Add(new OrderItem
            {
                DishId = dish.Id,
                Quantity = item.Quantity
            });

            itemSummaries.Add(new OrderItemSummary
            {
                DishName = dish.Name,
                UnitPrice = dish.Price,
                Quantity = item.Quantity
            });
        }

        var payment = new Payment
        {
            IdempotencyKey = request.IdempotencyKey.ToString(),
            Status = PaymentStatus.Pending,
            Amount = totalAmount,
        };

        var order = new Order
        {
            ClientId = request.Client.Id,
            Notes = request.Notes,
            Status = OrderStatus.Pending,
            Payment = payment,
            OrderItems = orderItems,
            ChefId = chefIds.First()
        };

        if (request.DeliveryAddressId == null)
        {
            var address = _mapper.Map<Domain.Entities.Address>(request.DeliveryAddress);
            address.ClientId = order.ClientId;
            order.DeliveryAddress = address;
        }
        else
        {
            var deliveryAddress = await _unitOfWork.Adresses.GetByIdAsync(request.DeliveryAddressId.Value);

            if (deliveryAddress == null)
                return DomainErrors.Address.AddressNotFound;

            order.DeliveryAddressId = deliveryAddress.Id;
        }

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            var paymentGateway = _paymentGatewayFactory.GetGateway(request.PaymentGateway);

            var orderSummary = new OrderSummary
            {
                OrderId = order.Id,
                PaymentId = payment.Id,
                ClientEmail = request.Client.Email,
                ClientFirstName = request.Client.FirstName,
                ClientLastName = request.Client.LastName,
                ClientPhone = request.Client.PhoneNumber,
                Items = itemSummaries,
                TotalAmount = payment.Amount,
            };

            var paymentIntent = await paymentGateway.CreatePaymentIntentAsync(orderSummary);
            
            payment.PaymentIntentId = paymentIntent.Id;

            var response = new PlaceOrderResponse
            {
                OrderId = order.Id,
                ClientSecret = paymentIntent.ClientSecret,
            };

            return Result.Success(response);
        }
        catch (PaymentGatewayException ex)
        {
            _logger.LogError(ex,
                "Failed to create payment intent. OrderId: {OrderId}, PaymentId: {PaymentId}, ClientId: {ClientId}, ChefId: {ChefId}, TotalAmount: {TotalAmount}",
                order.Id,
                payment.Id,
                order.ClientId,
                chefIds.FirstOrDefault(),
                payment.Amount
            );

            order.IsDeleted = true;
            order.DeletedAt = DateTime.UtcNow;
            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = $"Payment Intent creation failed: {ex.Message}";

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            throw;
        }
    }
}

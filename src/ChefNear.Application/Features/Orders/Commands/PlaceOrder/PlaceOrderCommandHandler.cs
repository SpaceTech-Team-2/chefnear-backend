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
        var existingPayment = await _unitOfWork.Payments.FindFirstAsync(p => p.IdempotencyKey == request.IdempotencyKey.ToString());

        // Check if a payment with the same idempotency key already exists
        if (existingPayment != null)
            return DomainErrors.Payment.IdempotencyKeyAlreadyExists;

        var dish = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);

        if (dish == null)
            return DomainErrors.Dish.DishNotFound;

        if (dish.Status != DishStatus.Available)
            return DomainErrors.Dish.DishUnavailable;

        if (request.DeliveryAddressId == null && request.DeliveryAddress == null)
            return DomainErrors.Order.DeliveryAddressNotProvided;

        var payment = new Payment
        {
            IdempotencyKey = request.IdempotencyKey.ToString(),
            Status = PaymentStatus.Pending,
            Amount = dish.Price * request.Quantity,
        };

        var order = new Order
        {
            ClientId = request.Client.Id,
            DishId = request.DishId,
            Quantity = request.Quantity,
            Notes = request.Notes,
            Status = OrderStatus.Pending,
            Payment = payment
        };

        if (request.DeliveryAddressId == null)
        {
            var address = _mapper.Map<Address>(request.DeliveryAddress);
            address.UserId = order.ClientId;
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
                DishName = dish.Name,
                Quantity = request.Quantity,
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
                "Failed to create payment intent. OrderId: {OrderId}, PaymentId: {PaymentId}, ClientId: {ClientId}, ChefId: {ChefId}, DishId: {DishId}, Quantity: {Quantity}, TotalAmount: {TotalAmount}",
                order.Id,
                payment.Id,
                order.ClientId,
                dish.ChefId,
                order.DishId,
                order.Quantity,
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

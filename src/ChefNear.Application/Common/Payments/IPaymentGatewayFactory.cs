using ChefNear.Domain.Enums;

namespace ChefNear.Application.Common.Payments;

public interface IPaymentGatewayFactory
{
    IPaymentGateway GetGateway(PaymentGateway gateway);
}

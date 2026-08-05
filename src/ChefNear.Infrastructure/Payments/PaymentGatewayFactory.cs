using ChefNear.Application.Common.Payments;
using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Domain.Enums;
using ChefNear.Infrastructure.Payments.PaymentGateways;
using Microsoft.Extensions.DependencyInjection;

namespace ChefNear.Infrastructure.Payments;

internal class PaymentGatewayFactory(IServiceProvider serviceProvider) : IPaymentGatewayFactory
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public IPaymentGateway GetGateway(PaymentGateway gateway)
    {
        return gateway switch
        {
            PaymentGateway.Paymob => serviceProvider.GetRequiredService<IPaymobService>(),
            _ => throw new NotImplementedException($"Payment gateway {gateway} is not implemented.")
        };
    }
}

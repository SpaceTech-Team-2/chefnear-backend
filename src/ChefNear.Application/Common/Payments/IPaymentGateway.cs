using Microsoft.AspNetCore.Http;

namespace ChefNear.Application.Common.Payments;

public interface IPaymentGateway
{
    Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(OrderSummary orderSummary);  
    Task VerifyWebhookAsync(HttpRequest request);
    Task RefundAsync(string transactionId);
}

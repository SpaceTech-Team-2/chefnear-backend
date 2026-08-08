namespace ChefNear.Application.Common.Payments.Paymob;

public interface IPaymobService : IPaymentGateway
{
    Task<string> Payout(decimal amount, string chennel);
}

namespace ChefNear.Application.Common.Payments;

public class CreatePaymentIntentResult
{
    public string Id { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
    public string PublicKey { get; set; } = default!;
}

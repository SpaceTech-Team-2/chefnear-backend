using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Infrastructure.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ChefNear.API.Filters;

public class VerifyHmacAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var paymobSettings = context.HttpContext.RequestServices.GetRequiredService<IOptions<PaymobSettings>>().Value;
        var logger = context.HttpContext.RequestServices.GetService<ILogger<VerifyHmacAttribute>>();

        // 1. Get received HMAC from query parameter
        if (!context.HttpContext.Request.Query.TryGetValue("hmac", out var receivedHmac) || string.IsNullOrEmpty(receivedHmac))
        {
            logger?.LogWarning("HMAC verification failed: Missing 'hmac' query parameter.");
            context.Result = new BadRequestResult();
            return;
        }

        // 2. Get bound PaymobWebhook model from ActionArguments
        var request = context.ActionArguments.Values
            .OfType<PaymobWebhook>()
            .FirstOrDefault();

        if (request?.Transaction == null)
        {
            logger?.LogWarning("HMAC verification failed: PaymobWebhook payload is missing or un-bound.");
            context.Result = new BadRequestResult();
            return;
        }

        var obj = request.Transaction;

        // 3. Concatenate fields in Paymob's required lexicographical order
        var concatenated = string.Concat(
            obj.AmountCents,
            obj.CreatedAt,
            obj.Currency,
            FormatBool(obj.ErrorOccured),
            FormatBool(obj.HasParentTransaction),
            obj.TransactionId,
            obj.IntegrationId,
            FormatBool(obj.Is3DSecure),
            FormatBool(obj.IsAuth),
            FormatBool(obj.IsCapture),
            FormatBool(obj.IsRefunded),
            FormatBool(obj.IsStandalonePayment),
            FormatBool(obj.IsVoided),
            obj.Order?.OrderId,
            obj.Owner,
            FormatBool(obj.Pending),
            obj.SourceData?.Pan ?? string.Empty,
            obj.SourceData?.SubType ?? string.Empty,
            obj.SourceData?.Type ?? string.Empty,
            FormatBool(obj.Success)
        );

        // 4. Compute SHA-512 HMAC
        using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(paymobSettings.HMAC));
        var hash = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(concatenated));
        var calculatedHmac = Convert.ToHexString(hash).ToLowerInvariant();
        var receivedHmacStr = receivedHmac.ToString().Trim().ToLowerInvariant();

        // 5. Securely compare calculated vs received HMAC
        var calculatedBytes = Encoding.UTF8.GetBytes(calculatedHmac);
        var receivedBytes = Encoding.UTF8.GetBytes(receivedHmacStr);

        var isValid = calculatedBytes.Length == receivedBytes.Length &&
                      CryptographicOperations.FixedTimeEquals(calculatedBytes, receivedBytes);

        if (!isValid)
        {
            logger?.LogWarning("HMAC verification mismatch.\n  Concatenated: {Concatenated}\n  Calculated:   {Calculated}\n  Received:     {Received}",
                concatenated, calculatedHmac, receivedHmacStr);
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    private static string FormatBool(bool value) => value ? "true" : "false";
}

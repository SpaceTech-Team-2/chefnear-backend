using ChefNear.Application.Common.Payments;
using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Application.Model;
using ChefNear.Domain.Exceptions;
using ChefNear.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ChefNear.Infrastructure.Payments.PaymentGateways;

internal class PaymobService(
    IHttpClientFactory httpClientFactory,
    IOptions<PaymobSettings> paymobSettings,
    IOptions<FrontendSettings> frontendSettings,
    IOptions<AppUrlSettings> appUrlSettings,
    ILogger<PaymobService> logger) : IPaymobService
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly PaymobSettings paymobSettings = paymobSettings.Value;
    private readonly FrontendSettings frontendSettings = frontendSettings.Value;
    private readonly AppUrlSettings appUrlSettings = appUrlSettings.Value;
    private readonly ILogger<PaymobService> logger = logger;

    public async Task<CreatePaymentIntentResult> CreatePaymentIntentAsync(OrderSummary orderSummary)
    {
        using var client = httpClientFactory.CreateClient();

        var url = $"{paymobSettings.BaseUrl}/{paymobSettings.Endpoints.Intention}";

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Token", paymobSettings.SecretKey);

        var requestBody = new
        {
            amount = orderSummary.TotalAmount * 100,
            currency = "EGP",
            payment_methods = new[]
            {
                5817122,
                5772602
            },
            items = new[]
            {
                new
                {
                    name = orderSummary.DishName,
                    amount = orderSummary.TotalAmount * 100,
                    description = "Home chef order",
                    quantity = orderSummary.Quantity
                }
            },
            billing_data = new
            {
                first_name = orderSummary.ClientFirstName,
                last_name = orderSummary.ClientLastName,
                email = orderSummary.ClientEmail,
                phone_number = orderSummary.ClientPhone
            },
            special_reference = orderSummary.PaymentId.ToString(),
            notification_url = $"{appUrlSettings.ApiBaseUrl}/{paymobSettings.WebhookRoute}",
            redirection_url = $"{frontendSettings.BaseUrl}/{frontendSettings.Routes.PaymentResultUrl}"
        };

        request.Content = JsonContent.Create(requestBody);

        try
        {
            logger.LogInformation(
                "Creating Paymob payment intent. PaymentId: {PaymentId}, Amount: {Amount}",
                orderSummary.PaymentId,
                orderSummary.TotalAmount);

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError(
                    "Paymob returned an error. StatusCode: {StatusCode}, PaymentId: {PaymentId}, Response: {Response}",
                    response.StatusCode,
                    orderSummary.PaymentId,
                    responseContent);

                throw new PaymentGatewayException(
                    $"Paymob returned {(int)response.StatusCode} ({response.StatusCode}).",
                    (int)response.StatusCode);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var result = JsonSerializer.Deserialize<CreatePaymentIntentResult>(
                responseContent,
                options);

            if (result is null)
            {
                logger.LogError(
                    "Failed to deserialize Paymob response. PaymentId: {PaymentId}",
                    orderSummary.PaymentId
                );

                throw new PaymentGatewayException(
                    "Invalid response returned from Paymob.",
                    (int)response.StatusCode);
            }

            logger.LogInformation(
                "Paymob payment intent created successfully. PaymentId: {PaymentId}, IntentId: {IntentId}",
                orderSummary.PaymentId,
                result.Id
            );

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Network error while calling Paymob. PaymentId: {PaymentId}",
                orderSummary.PaymentId
            );

            throw new PaymentGatewayException(
                "Unable to communicate with the payment gateway.",
                (int)HttpStatusCode.ServiceUnavailable,
                ex
            );
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(
                ex,
                "Paymob request timed out. PaymentId: {PaymentId}",
                orderSummary.PaymentId
            );

            throw new PaymentGatewayException(
                "Payment gateway request timed out.",
                (int)HttpStatusCode.ServiceUnavailable,
                ex
            );
        }
        catch (JsonException ex)
        {
            logger.LogError(
                ex,
                "Invalid JSON returned from Paymob. PaymentId: {PaymentId}",
                orderSummary.PaymentId
            );

            throw new PaymentGatewayException(
                "Payment gateway returned an invalid response.",
                (int)HttpStatusCode.BadGateway,
                ex
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error while creating Paymob payment intent. PaymentId: {PaymentId}",
                orderSummary.PaymentId
            );

            throw;
        }
    }

    public Task RefundAsync(string transactionId)
    {
        throw new NotImplementedException();
    }

    public Task VerifyWebhookAsync(HttpRequest request)
    {
        throw new NotImplementedException();
    }
}

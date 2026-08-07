using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ChefNear.Application.Common.Payments;
using ChefNear.Application.Model;
using ChefNear.Domain.Exceptions;
using ChefNear.Infrastructure.Payments.PaymentGateways;
using ChefNear.Infrastructure.Settings;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace ChefNear.Infrastructure.Tests.Payments;

public class PaymobServiceTests
{
    private const string SecretKey = "test-secret-key";
    private const string BaseUrl = "https://accept.paymob.com";
    private const string IntentionEndpoint = "v1/intention/";
    private const string RefundEndpoint = "api/acceptance/void_refund/refund";

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenSuccessful_ReturnsPaymentIntentResult()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderSummary = CreateOrderSummary(paymentId);
        var responseJson = """
            {
              "id": "intent_123",
              "client_secret": "secret_abc"
            }
            """;

        var handler = CreateHandler(HttpStatusCode.OK, responseJson);
        var sut = CreateSut(handler);

        // Act
        var result = await sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        result.Id.Should().Be("intent_123");
        result.ClientSecret.Should().Be("secret_abc");
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenSuccessful_SendsAuthorizedPostToIntentionEndpoint()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderSummary = CreateOrderSummary(paymentId);
        HttpRequestMessage? capturedRequest = null;

        var handler = CreateHandler(
            HttpStatusCode.OK,
            """{"id":"intent_123","client_secret":"secret_abc"}""",
            request =>
            {
                capturedRequest = request;
                return Task.CompletedTask;
            });

        var sut = CreateSut(handler);

        // Act
        await sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri!.ToString().Should().Be($"{BaseUrl}/{IntentionEndpoint}");
        capturedRequest.Headers.Authorization.Should().BeEquivalentTo(
            new AuthenticationHeaderValue("Token", SecretKey));
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenPaymobReturnsError_ThrowsPaymentGatewayException()
    {
        // Arrange
        var orderSummary = CreateOrderSummary(Guid.NewGuid());
        var handler = CreateHandler(HttpStatusCode.BadRequest, """{"error":"invalid request"}""");
        var sut = CreateSut(handler);

        // Act
        var act = () => sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        exception.Which.Message.Should().Contain("400");
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenResponseIsNull_ThrowsPaymentGatewayException()
    {
        // Arrange
        var orderSummary = CreateOrderSummary(Guid.NewGuid());
        var handler = CreateHandler(HttpStatusCode.OK, "null");
        var sut = CreateSut(handler);

        // Act
        var act = () => sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.Message.Should().Be("Invalid response returned from Paymob.");
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenResponseIsInvalidJson_ThrowsPaymentGatewayException()
    {
        // Arrange
        var orderSummary = CreateOrderSummary(Guid.NewGuid());
        var handler = CreateHandler(HttpStatusCode.OK, "{invalid-json");
        var sut = CreateSut(handler);

        // Act
        var act = () => sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.StatusCode.Should().Be((int)HttpStatusCode.BadGateway);
        exception.Which.Message.Should().Be("Payment gateway returned an invalid response.");
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WhenNetworkFails_ThrowsPaymentGatewayException()
    {
        // Arrange
        var orderSummary = CreateOrderSummary(Guid.NewGuid());
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network failure"));

        var sut = CreateSut(handlerMock.Object);

        // Act
        var act = () => sut.CreatePaymentIntentAsync(orderSummary);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);
        exception.Which.Message.Should().Be("Unable to communicate with the payment gateway.");
    }

    [Fact]
    public async Task RefundAsync_WhenSuccessful_ReturnsRefundTransactionId()
    {
        // Arrange
        const string transactionId = "txn_123";
        const decimal amount = 75.50M;
        var handler = CreateHandler(HttpStatusCode.OK, """{"id":987654321}""");
        var sut = CreateSut(handler);

        // Act
        var refundTransactionId = await sut.RefundAsync(transactionId, amount);

        // Assert
        refundTransactionId.Should().Be("987654321");
    }

    [Fact]
    public async Task RefundAsync_WhenSuccessful_SendsAuthorizedPostToRefundEndpoint()
    {
        // Arrange
        const string transactionId = "txn_123";
        const decimal amount = 75.50M;
        HttpRequestMessage? capturedRequest = null;

        var handler = CreateHandler(
            HttpStatusCode.OK,
            """{"id":987654321}""",
            request =>
            {
                capturedRequest = request;
                return Task.CompletedTask;
            });

        var sut = CreateSut(handler);

        // Act
        await sut.RefundAsync(transactionId, amount);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri!.ToString().Should().Be($"{BaseUrl}/{RefundEndpoint}");
        capturedRequest.Headers.Authorization.Should().BeEquivalentTo(
            new AuthenticationHeaderValue("Token", SecretKey));
    }

    [Fact]
    public async Task RefundAsync_WhenPaymobReturnsError_ThrowsPaymentGatewayException()
    {
        // Arrange
        var handler = CreateHandler(HttpStatusCode.UnprocessableEntity, """{"error":"refund failed"}""");
        var sut = CreateSut(handler);

        // Act
        var act = () => sut.RefundAsync("txn_123", 10M);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.StatusCode.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        exception.Which.Message.Should().Contain("422");
    }

    [Fact]
    public async Task RefundAsync_WhenNetworkFails_ThrowsPaymentGatewayException()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network failure"));

        var sut = CreateSut(handlerMock.Object);

        // Act
        var act = () => sut.RefundAsync("txn_123", 10M);

        // Assert
        var exception = await act.Should().ThrowAsync<PaymentGatewayException>();
        exception.Which.StatusCode.Should().Be((int)HttpStatusCode.ServiceUnavailable);
        exception.Which.Message.Should().Be("Unable to communicate with the payment gateway.");
    }

    [Fact]
    public async Task RefundAsync_WhenRefundEndpointNotConfigured_UsesDefaultEndpoint()
    {
        // Arrange
        HttpRequestMessage? capturedRequest = null;
        var handler = CreateHandler(
            HttpStatusCode.OK,
            """{"id":12345}""",
            request =>
            {
                capturedRequest = request;
                return Task.CompletedTask;
            });

        var paymobSettings = CreatePaymobSettings();
        paymobSettings.Endpoints.Refund = string.Empty;

        var sut = CreateSut(handler, paymobSettings);

        // Act
        await sut.RefundAsync("txn_123", 10M);

        // Assert
        capturedRequest!.RequestUri!.ToString().Should().Be($"{BaseUrl}/{RefundEndpoint}");
    }

    [Fact]
    public async Task VerifyWebhookAsync_ThrowsNotImplementedException()
    {
        // Arrange
        var handler = CreateHandler(HttpStatusCode.OK, "{}");
        var sut = CreateSut(handler);

        // Act
        var act = () => sut.VerifyWebhookAsync(new DefaultHttpContext().Request);

        // Assert
        await act.Should().ThrowAsync<NotImplementedException>();
    }

    private static OrderSummary CreateOrderSummary(Guid paymentId)
    {
        return new OrderSummary
        {
            OrderId = Guid.NewGuid(),
            PaymentId = paymentId,
            TotalAmount = 100M,
            ClientFirstName = "John",
            ClientLastName = "Doe",
            ClientEmail = "john.doe@example.com",
            ClientPhone = "+201234567890",
            Items =
            [
                new OrderItemSummary
                {
                    DishName = "Koshari",
                    UnitPrice = 50M,
                    Quantity = 2
                }
            ]
        };
    }

    private static PaymobSettings CreatePaymobSettings()
    {
        return new PaymobSettings
        {
            BaseUrl = BaseUrl,
            SecretKey = SecretKey,
            WebhookRoute = "api/payments/paymob/webhook",
            Endpoints = new Endpoints
            {
                Intention = IntentionEndpoint,
                Refund = RefundEndpoint
            }
        };
    }

    private static PaymobService CreateSut(
        HttpMessageHandler handler,
        PaymobSettings? paymobSettings = null)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient(handler));

        paymobSettings ??= CreatePaymobSettings();

        var frontendSettings = Options.Create(new FrontendSettings
        {
            BaseUrl = "https://frontend.example.com",
            Routes = new FrontendRoutes
            {
                PaymentResultUrl = "payment/result"
            }
        });

        var appUrlSettings = Options.Create(new AppUrlSettings
        {
            ApiBaseUrl = "https://api.example.com"
        });

        return new PaymobService(
            httpClientFactory.Object,
            Options.Create(paymobSettings),
            frontendSettings,
            appUrlSettings,
            NullLogger<PaymobService>.Instance);
    }

    private static HttpMessageHandler CreateHandler(
        HttpStatusCode statusCode,
        string responseContent,
        Func<HttpRequestMessage, Task>? onRequest = null)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>(async (request, _) =>
            {
                if (onRequest is not null)
                {
                    await onRequest(request);
                }

                return new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                };
            });

        return handlerMock.Object;
    }
}

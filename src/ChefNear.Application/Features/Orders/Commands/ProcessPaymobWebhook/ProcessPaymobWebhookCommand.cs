using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Commands.ProcessPaymobWebhook;

public record ProcessPaymobWebhookCommand(PaymobWebhook Webhook) : IRequest<Result>;

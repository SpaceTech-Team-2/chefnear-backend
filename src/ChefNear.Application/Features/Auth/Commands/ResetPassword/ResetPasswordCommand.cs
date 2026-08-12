using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword
) : IRequest<Result>;

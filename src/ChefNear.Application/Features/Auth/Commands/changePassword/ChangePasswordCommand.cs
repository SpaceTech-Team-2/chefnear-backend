using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordComand(
    string OLdPassword,
    string NewPassword,
    string ConfirmPassword
) : IRequest<Result<ChangePasswordResponse>>;

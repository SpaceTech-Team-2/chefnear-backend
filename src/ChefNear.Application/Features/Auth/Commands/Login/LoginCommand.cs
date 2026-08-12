using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password,
    bool RememberMe = false
) : IRequest<Result<LoginResponse>>;

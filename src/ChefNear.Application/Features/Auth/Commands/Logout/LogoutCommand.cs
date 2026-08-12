using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Logout;

public record LogoutRequest(string? RefreshToken);

public record LogoutCommand(
    string UserId,
    string? RefreshToken
) : IRequest<Result<LogoutResponse>>;

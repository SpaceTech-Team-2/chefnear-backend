using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<Result<RefreshTokenResponse>>;

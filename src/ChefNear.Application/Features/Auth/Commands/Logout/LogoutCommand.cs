using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Result<LogoutResponse>>
    {
        public string UserId { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}

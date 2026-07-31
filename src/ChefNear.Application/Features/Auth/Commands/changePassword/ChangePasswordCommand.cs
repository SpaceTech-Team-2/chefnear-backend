using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.changePassword
{
    public class ChangePasswordComand : IRequest<Result<ChangePasswordResponse>>
    {
        public string OLdPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}

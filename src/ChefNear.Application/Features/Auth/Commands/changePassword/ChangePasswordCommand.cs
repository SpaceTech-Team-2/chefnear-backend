using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordComand : IRequest<Result<ChangePasswordResponse>>
    {
        public string OLdPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}

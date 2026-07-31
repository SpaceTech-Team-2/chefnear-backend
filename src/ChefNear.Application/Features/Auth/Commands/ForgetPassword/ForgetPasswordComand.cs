using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordComand : IRequest<Result>
    {
        public string Email { get; set; } = default!;
    }
}

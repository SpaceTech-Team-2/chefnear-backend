using ChefNear.Application.Responce;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordComand : IRequest<AuthResponse>
    {
        public string OLdPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}

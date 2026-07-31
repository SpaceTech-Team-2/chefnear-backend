using ChefNear.Application.Responce;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}

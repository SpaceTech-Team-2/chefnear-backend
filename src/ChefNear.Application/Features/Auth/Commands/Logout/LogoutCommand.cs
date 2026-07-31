using ChefNear.Application.Responce;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<AuthResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}

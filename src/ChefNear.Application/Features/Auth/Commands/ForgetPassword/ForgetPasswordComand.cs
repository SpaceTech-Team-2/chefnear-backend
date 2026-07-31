using ChefNear.Application.Responce;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordComand : IRequest<BaseCommandResponse>
    {
        public string Email { get; set; } = default!;
    }
}

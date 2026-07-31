using ChefNear.Application.Responce;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<BaseCommandResponse>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}

using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Admin.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result<bool>>
    {
        public string UserId { get; set; }
        public DeleteUserCommand(string userId) => UserId = userId;
    }
}

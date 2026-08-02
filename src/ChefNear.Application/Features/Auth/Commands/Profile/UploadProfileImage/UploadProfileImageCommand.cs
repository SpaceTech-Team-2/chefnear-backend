using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Profile.UploadProfileImage
{
   public class UploadProfileImageCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public byte[] FileBytes { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}

using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Profile.Commands.UploadProfileImage;

public record UploadProfileImageCommand(
    Guid UserId,
    byte[] FileBytes,
    string FileName
) : IRequest<Result>;

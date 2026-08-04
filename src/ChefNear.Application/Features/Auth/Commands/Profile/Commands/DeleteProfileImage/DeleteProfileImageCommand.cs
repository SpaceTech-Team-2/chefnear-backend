using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Profile.Commands.DeleteProfileImage;

public class DeleteProfileImageCommand : IRequest<Result>
{
    public string UserId { get; set; } = string.Empty;
}
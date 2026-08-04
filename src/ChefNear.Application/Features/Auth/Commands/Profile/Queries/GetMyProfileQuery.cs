using ChefNear.Application.Features.Auth.Commands.Profile.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Queries.Profile.GetMyProfile;

public class GetMyProfileQuery : IRequest<Result<ProfileDto>>
{
    public string UserId { get; set; } = string.Empty;
}
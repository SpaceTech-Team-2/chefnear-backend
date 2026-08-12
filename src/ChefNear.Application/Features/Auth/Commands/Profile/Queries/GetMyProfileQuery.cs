using ChefNear.Application.Features.Auth.Commands.Profile.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Queries.Profile.GetMyProfile;

public record GetMyProfileQuery(string UserId) : IRequest<Result<ProfileDto>>;
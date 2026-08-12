using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Profile.Commands.DeleteProfileImage;

public record DeleteProfileImageCommand(string UserId) : IRequest<Result>;
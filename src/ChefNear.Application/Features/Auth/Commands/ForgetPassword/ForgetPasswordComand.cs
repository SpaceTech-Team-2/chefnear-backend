using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.ForgetPassword;

public record ForgetPasswordComand(string Email) : IRequest<Result>;

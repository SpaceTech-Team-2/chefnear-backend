using ChefNear.Shared.Constants;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class CreateAdminCommand : IRequest<Result<string>>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}


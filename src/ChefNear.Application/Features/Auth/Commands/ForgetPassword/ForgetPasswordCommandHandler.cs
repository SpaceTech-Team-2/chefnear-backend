using ChefNear.Application.Features.Auth.Commands.ForgetPassword;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordComand, BaseCommandResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly AppUrlSettings _appUrlSettings;

    public ForgetPasswordCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService,
        IOptions<AppUrlSettings> appUrlSettings)
    {
        _userManager = userManager;
        _emailService = emailService;
        _appUrlSettings = appUrlSettings.Value;
    }

    public async Task<BaseCommandResponse> Handle(ForgetPasswordComand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new BaseCommandResponse
            {
                Success = true,
                Message = "If an account exists, a reset link has been sent."
            };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);
        var encodedEmail = Uri.EscapeDataString(user.Email!);

        var resetLink = $"{_appUrlSettings.FrontendBaseUrl}/{_appUrlSettings.ResetPasswordPath}?email={encodedEmail}&token={encodedToken}";

        await _emailService.SendEmailAsync(
            user.Email!,
            "Reset Password - ChefNear",
            $"Click here to reset your password: {resetLink}");

        return new BaseCommandResponse
        {
            Success = true,
            Message = "If an account exists, a reset link has been sent."
        };
    }
}
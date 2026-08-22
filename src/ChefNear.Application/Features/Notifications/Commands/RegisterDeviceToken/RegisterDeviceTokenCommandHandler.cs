using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Notifications.Commands.RegisterDeviceToken;

public class RegisterDeviceTokenCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RegisterDeviceTokenCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RegisterDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        // case 1 : token not exists before
        // then just insert it into the DeviceTokens DB with UserId FK
        // case 2 : token exists (thats mean user register again with another account)
        // re-assign that token with the current registered user if already the current user differ from the token.UserId

        var deviceToken = await _unitOfWork.DeviceTokens.GetByTokenAsync(request.Token);

        if (deviceToken != null && request.UserId == deviceToken.UserId)
            return Result.Success();

        if(deviceToken == null)
        {
            var newDeviceToken = DeviceToken.CreateToken(request.Token, request.UserId);
            await _unitOfWork.DeviceTokens.AddAsync(newDeviceToken);
        }
        else
        {
            deviceToken.AssignToUser(request.UserId);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAddressCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _unitOfWork.adresses.GetByIdAsync(request.AddressId);

        if (address == null)
        {
            return Result.Failure(
                Error.NotFound("Address.NotFound", "Address not found.")
            );
        }

        if (address.UserId != request.UserId.ToString())
        {
            return Result.Failure(
                Error.Forbidden("Address.Forbidden", "You cannot update this address.")
            );
        }

        if (request.IsDefault)
        {
            var addresses = await _unitOfWork.adresses.GetAllAsync();

            foreach (var add in addresses.Where(a => a.UserId == request.UserId.ToString() && a.IsDefault))
            {
                add.IsDefault = false;
            }
        }

        address.Label = request.Label;
        address.City = request.City;
        address.Details = request.Details;
        address.Latitude = request.Latitude;
        address.Longitude = request.Longitude;
        address.IsDefault = request.IsDefault;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
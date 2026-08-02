using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Application.Features.Addresses.Commands;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public class CreateAddressCommandHandler
    : IRequestHandler<CreateAddressCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAddressCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        if (request.IsDefault)
        {
            var addresses = await _unitOfWork.adresses.GetAllAsync();

            foreach (var add in addresses.Where(a => a.UserId == request.UserId.ToString() && a.IsDefault))
            {
                add.IsDefault = false;
            }
        }

        var address = new Domain.Entities.Address
        {
            UserId = request.UserId.ToString(),
            Label = request.Label,
            City = request.City,
            Details = request.Details,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsDefault = request.IsDefault
        };

        await _unitOfWork.adresses.AddAsync(address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(address.Id);
    }
}
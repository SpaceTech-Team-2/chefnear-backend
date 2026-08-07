using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAddressCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _unitOfWork.Adresses.GetByIdAsync(request.AddressId);

        if (address == null)
        {
            return Result.Failure( Error.NotFound("Address.NotFound", "Address not found."));
         
        }

        if (address.ClientId != request.UserId.ToString())
        {
            return Result.Failure( Error.Forbidden("Address.Forbidden", "You cannot delete this address."));
        }

        await _unitOfWork.Adresses.DeleteAsync(address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
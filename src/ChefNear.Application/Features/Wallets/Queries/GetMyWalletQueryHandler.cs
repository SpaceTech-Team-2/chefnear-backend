using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Wallets.DTOs;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Wallets.Queries;

public class GetMyWalletQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetMyWalletQuery, Result<GetMyWalletDto>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<Result<GetMyWalletDto>> Handle(GetMyWalletQuery request, CancellationToken cancellationToken)
    {
        var wallet = await unitOfWork.Wallets
            .GetAsync(w => w.ChefId == request.Chef.Id, nameof(Wallet.Transactions));

        if (wallet == null)
            return DomainErrors.Wallet.WalletNotFound;

        var dto = mapper.Map<GetMyWalletDto>(wallet);

        return Result.Success(dto);
    }
}

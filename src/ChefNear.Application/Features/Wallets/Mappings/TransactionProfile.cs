using AutoMapper;
using ChefNear.Application.Features.Wallets.Commands.Withdraw;
using ChefNear.Application.Features.Wallets.DTOs;
using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Wallets.Mappings;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<WalletTransaction, WalletWithdrawalResponse>()
            .ForMember(x => x.TransactionId, opt => opt.MapFrom(src => src.Id));

        CreateMap<WalletTransaction, WalletTransactionDto>();
        CreateMap<Wallet, GetMyWalletDto>();
    }
}

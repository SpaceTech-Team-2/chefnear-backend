using ChefNear.Application.Features.Wallets.DTOs;
using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Wallets.Queries;

public record GetMyWalletQuery(
    CurrentUser Chef
): IRequest<Result<GetMyWalletDto>>;

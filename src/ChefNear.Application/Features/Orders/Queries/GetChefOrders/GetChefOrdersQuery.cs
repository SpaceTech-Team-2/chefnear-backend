using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Orders.Queries.GetChefOrders;

public record GetChefOrdersRequest(
    bool IsActive = true,
    int PageNumber = 1,
    int PageSize = 10
);

public record GetChefOrdersQuery(
    CurrentUser Chef,
    bool IsActive = true,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<List<ChefOrderDto>>>;

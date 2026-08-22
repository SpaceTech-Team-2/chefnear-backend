using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Orders.Queries.GetChefOrders;

public class GetChefOrdersQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<GetChefOrdersQuery, Result<List<ChefOrderDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<ChefOrderDto>>> Handle(GetChefOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.Orders
            .GetOrdersWithDetails(request.Chef.Id, request.PageNumber, request.PageSize, request.IsActive);

        var dto = _mapper.Map<List<ChefOrderDto>>(orders);
        return Result.Success(dto);
    }
}

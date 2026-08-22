using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dish.Queries.GetChefDishes;

public class GetChefDishesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetChefDishesQuery, Result<List<DishSummaryDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<DishSummaryDto>>> Handle(GetChefDishesQuery request, CancellationToken cancellationToken)
    {
        var chefDishes = await _unitOfWork.Dishes.GetDishesByChefId(request.ChefId);

        var dto = _mapper.Map<List<DishSummaryDto>>(chefDishes);
        return Result.Success(dto);
    }
}

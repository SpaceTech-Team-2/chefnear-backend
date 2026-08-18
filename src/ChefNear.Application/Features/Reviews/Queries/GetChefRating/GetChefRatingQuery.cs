using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetChefRating
{
    public class GetChefRatingQuery : IRequest<Result<ChefRatingDto>>
    {
        public string ChefId { get; set; } = string.Empty;
    }
}

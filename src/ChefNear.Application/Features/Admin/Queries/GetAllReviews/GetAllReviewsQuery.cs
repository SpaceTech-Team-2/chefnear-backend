using ChefNear.Application.Features.Admin.Queries.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.GetAllReviews
{
    public class GetAllReviewsQuery : IRequest<Result<List<AdminReviewDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

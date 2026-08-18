using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQuery : IRequest<Result<ReviewDto>>
    {
        public Guid ReviewId { get; set; }
    }
}

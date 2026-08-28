using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Command.AdminDeleteReview
{
    public class AdminDeleteReviewCommand : IRequest<Result<bool>>
    {
        public Guid ReviewId { get; set; }
        public AdminDeleteReviewCommand(Guid reviewId) => ReviewId = reviewId;
    }
}

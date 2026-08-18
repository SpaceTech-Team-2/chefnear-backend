using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand :IRequest<Result>
    {
        public Guid ReviewId { get; set; }
        [JsonIgnore]
        public string ClientId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

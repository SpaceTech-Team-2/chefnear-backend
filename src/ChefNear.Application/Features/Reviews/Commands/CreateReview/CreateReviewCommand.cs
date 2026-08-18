using AutoMapper.Configuration.Annotations;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Reviews.Commands.AddReview
{
    public class CreateReviewCommand :IRequest<Result<Guid>>
    {
        public Guid OrderId { get; set; }
        public Guid DishId { get; set; }
        [JsonIgnore]
        public string ClientId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

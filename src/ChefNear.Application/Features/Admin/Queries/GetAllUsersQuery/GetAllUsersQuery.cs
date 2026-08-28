using ChefNear.Application.Features.Admin.Queries.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.GetAllUsersQuery
{
    public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Role { get; set; }
        public string? Search { get; set; }
    }

}

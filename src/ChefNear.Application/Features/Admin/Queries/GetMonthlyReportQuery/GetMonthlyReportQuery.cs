using ChefNear.Application.Features.Admin.Queries.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.GetMonthlyReportQuery
{
    public class GetMonthlyReportQuery : IRequest<Result<MonthlyReportDto>>
    {
    }
}

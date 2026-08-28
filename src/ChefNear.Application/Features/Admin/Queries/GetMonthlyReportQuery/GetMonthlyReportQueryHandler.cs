using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Admin.Queries.DTOs;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.GetMonthlyReportQuery
{
    public class GetMonthlyReportQueryHandler : IRequestHandler<GetMonthlyReportQuery, Result<MonthlyReportDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> _userManager;

        public GetMonthlyReportQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<MonthlyReportDto>> Handle(GetMonthlyReportQuery request, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var ordersThisMonth = await unitOfWork.Orders
     .GetQueryable()
     .Where(o => o.CreatedAt >= startOfMonth && o.CreatedAt < startOfNextMonth)
     .ToListAsync(ct);

            var totalOrders = ordersThisMonth.Count;

            var cancelledOrders = ordersThisMonth
                .Count(o => o.Status == OrderStatus.Cancelled);

            
            var cancellationRate = totalOrders == 0
                ? 0
                : Math.Round((double)cancelledOrders / totalOrders * 100, 1);

            var dto = new MonthlyReportDto
            {
                TotalOrders = totalOrders,
                CancelledOrders = cancelledOrders,
                CancellationRate = cancellationRate
            };

            return Result<MonthlyReportDto>.Success(dto);
        }
    }
}

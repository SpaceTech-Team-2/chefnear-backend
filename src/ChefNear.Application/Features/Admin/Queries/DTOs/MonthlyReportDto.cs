using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Queries.DTOs
{
    public class MonthlyReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewUsers { get; set; }
        public int CancelledOrders { get; set; }
        public double CancellationRate { get; set; }
    }
}

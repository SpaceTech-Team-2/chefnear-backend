using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
   
    public class Dispute : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string FiledByUserId { get; set; } = string.Empty; 
        public User FiledBy { get; set; } = null!;

        public string? ResolvedByAdminId { get; set; }
        public User? ResolvedByAdmin { get; set; }

        public DisputeType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DisputeStatus Status { get; set; } = DisputeStatus.Open;
        public string? Resolution { get; set; }
    }

}

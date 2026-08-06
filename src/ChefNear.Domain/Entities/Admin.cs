using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class Admin : User
    {
        public ICollection<Dispute> ResolvedDisputes { get; set; } = new List<Dispute>();
    }
}

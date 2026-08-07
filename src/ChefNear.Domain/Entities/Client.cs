using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class Client : User
    {
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

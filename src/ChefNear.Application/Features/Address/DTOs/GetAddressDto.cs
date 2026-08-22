using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Address.DTOs
{
    public class GetAddressDto
    {
        public Guid Id { get; set; }

        public string? Label { get; set; }

        public string City { get; set; } = string.Empty;

        public string? Details { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsDefault { get; set; }
    }
}

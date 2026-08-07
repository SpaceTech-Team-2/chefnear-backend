using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Profile.DTOs
{
    public class ProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}

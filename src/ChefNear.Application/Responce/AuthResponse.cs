using System;
using System.Collections.Generic;

namespace ChefNear.Application.Responce
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Role { get; set; }
        public List<string>? Roles { get; set; }

        public bool OnboardingCompleted { get; set; }
        public int CurrentStep { get; set; }

    public string? Token { get; set; }                  
        public string? AccessToken { get; set; }             
        public DateTime? TokenExpiration { get; set; }        
        public string? RefreshToken { get; set; }             
        public DateTime? RefreshTokenExpiration { get; set; } 
        public string? TokenType { get; set; } = "Bearer";   

        // === Constructor ===
        public AuthResponse()
        {
            Roles = new List<string>();
            Errors = new List<string>();
        }
    }
}
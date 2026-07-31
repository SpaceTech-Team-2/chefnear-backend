using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Model
{
    public class AppUrlSettings
    {
        public string FrontendBaseUrl { get; set; } = "http://localhost:4200";
        public string ApiBaseUrl { get; set; } = "https://localhost:5001";
        public string ResetPasswordPath { get; set; } = "auth/reset-password";
        public string ConfirmEmailPath { get; set; } = string.Empty;

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}

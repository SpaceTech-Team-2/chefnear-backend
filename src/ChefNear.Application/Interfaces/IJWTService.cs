using ChefNear.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ChefNear.Application.Interfaces
{
  
        public interface IJWTService
        {
            Task<JwtSecurityToken> CreateJwtToken(User user, IList<string> roles);
        }
    
}

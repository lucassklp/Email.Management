using System;
using System.Linq;
using System.Security.Claims;
using Backend.Domain;
using Backend.Persistence;
using Email.Management.Dtos;
using Microsoft.AspNetCore.Http;

namespace Email.Management.Providers
{
    public class AuthenticatedUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DaoContext context;
        public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor, DaoContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public User GetAuthenticatedUser()
        {
            var tokenId = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (tokenId != null)
            {
                var id = Convert.ToInt32(tokenId);
                return context.Set<User>().FirstOrDefault(x => x.Id == id);
            }

            return null;
        }

        public JwtUserDto GetAuthenticatedJwtUser()
        {
            var tokenId = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var tokenEmail = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (tokenId != null)
            {
                var id = Convert.ToInt32(tokenId);
                return new JwtUserDto(id, tokenEmail);
            }

            return new JwtUserDto(0, null);
        }
    }
}

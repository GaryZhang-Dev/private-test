using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using AuthClient.Models;
using IdentityModel;
using Api.infrastructure;

namespace AuthClient
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            claims.AddRange(context.IssuedClaims);
            context.IssuedClaims = claims;
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var role = context.Subject.Claims?.SingleOrDefault(x => x.Type == JwtClaimTypes.Role)?.Value;

            if (role == null)
            {
                return Task.CompletedTask;
            }
            if (context.Client.ClientId == ClientIds.AuthWebClient ||
                context.Client.ClientId == ClientIds.AuthWebClientForMobile)
            {
                if (role != "user")
                {
                    context.IsActive = false;
                }
            }
            return Task.CompletedTask;
        }
    }
}

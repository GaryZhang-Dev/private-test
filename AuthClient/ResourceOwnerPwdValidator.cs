using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthClient
{
    public class ResourceOwnerPwdValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "test" && context.Password == "test")
            {
                context.Result = new GrantValidationResult(subject: context.UserName, authenticationMethod: OidcConstants.AuthenticationMethods.Password);
            }
            else {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null,
                    new Dictionary<string, object> { {"code","500" },{"message","用户名或密码错误" } });
            }

            return Task.FromResult(0);
        }
    }
}

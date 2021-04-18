using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using IdentityResource = IdentityServer4.Models.IdentityResource;

namespace AuthClient
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource> { new ApiResource("api", "this is a api") };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone()
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser
                {
                    SubjectId = "123",
                    Username = "Mr",
                    Password = "123456",
                    Claims = new Claim[] {
                        new Claim( ClaimTypes.Role, "管理员" )
                    }
                },
                new TestUser
                {
                    SubjectId = "456",
                    Username = "Mr",
                    Password = "123456",
                    Claims = new Claim[] {
                        new Claim( ClaimTypes.Role, "阅读者" )
                    }
                },
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="mvc_imp",
                    ClientName="Mvc_Name",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    //设置是否要授权
                    //RequireConsent=false,
                    
                    //指定允许令牌或授权码返回的地址（URL）
                    RedirectUris={ "http://www.b.net:5001/signin-oidc","http://www.a.cn:5002/signin-oidc" },
                    //指定允许注销后返回的地址(URL)，这里写两个客户端
                    PostLogoutRedirectUris={ "http://www.b.net:5001/signout-callback-oidc","http://www.a.cn:5002/signout-callback-oidc" },
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                }
             };
        }
    }

    
}

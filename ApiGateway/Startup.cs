using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Api.infrastructure;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ApiGateWay
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
            services.AddStackExchangeRedisCache(cache =>
            {
                cache.Configuration = Configuration["RedisConfiguration"];
            });
            services.AddDataProtection(s => { s.ApplicationDiscriminator = "PrivateTest"; })
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(Configuration["RedisConfiguration"]), "DP-Keys");
            services.AddOcelot(Configuration).AddPolly();
            //services.AddAuthentication()
                //.AddIdentityServerAuthentication("PrivateTestKey", option =>
                //{
                //    option.Authority = "localhost://8800";
                //    option.ApiName = "private";
                //    option.RequireHttpsMetadata = false;
                //    option.SupportedTokens = SupportedTokens.Both;
                //});
            services.AddAuthentication()
                .AddIdentityServerAuthentication("OpenApiServiceKey", option =>
                {
                    option.Authority = Configuration["IdentityServerUri"];
                    option.ApiName = "openapi-service";
                    option.RequireHttpsMetadata = false;
                    option.SupportedTokens = SupportedTokens.Both;
                });
            services.AddAuthentication()
                .AddIdentityServerAuthentication("OAuthKey", option =>
                {
                    option.Authority = Configuration["IdentityServerUri"];
                    option.ApiName = "oauth";
                    option.RequireHttpsMetadata = false;
                    option.SupportedTokens = SupportedTokens.Both;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                    ForwardedHeaders.XForwardedProto
            });
            app.UseCors("CorsPolicy");

            app.UseWebSockets();
            app.UseOcelot(cfg =>
            {
                cfg.PreQueryStringBuilderMiddleware = PreQueryStringBuilderMiddleware;
                cfg.PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    //Console.WriteLine(JsonConvert.SerializeObject(ctx.HttpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value)));

                    await next.Invoke();
                };
            }).Wait();
            //app.UseMvcWithDefaultRoute();
        }

        private Task PreQueryStringBuilderMiddleware(HttpContext context, Func<Task> next)
        {
            var includeClaims = new string[]
            {
                "sub",
                "role",
                "client_id",
                "client_role"
            };

            if (context.User?.Claims?.Any() ?? false)
            {
                foreach (var claim in context.User.Claims)
                {
                    if (includeClaims.Contains(claim.Type))
                    {
                        context.Request.Headers.Add("claims_" + claim.Type, claim.Value);
                    }
                }
            }

            return next();
        }
    }
}

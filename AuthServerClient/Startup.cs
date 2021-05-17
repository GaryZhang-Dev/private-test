using AccountService.Domain;
using Api.infrastructure;
using AuthServerClient.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AuthServerClient
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var authConnect = Configuration["AuthServerDb"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddCors();
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
            services.AddMvc();

            var redisConnect = ConnectionMultiplexer.Connect(Configuration["RedisConfiguration"]);// "");q
            services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "MyPrivateTest"; })
                .PersistKeysToStackExchangeRedis(redisConnect, "DP-Keys");

            var identityServer = services.AddIdentityServer(identity =>
            {
                identity.IssuerUri = Configuration["IdentityServerUri"];
                identity.UserInteraction.LoginUrl = Configuration["AuthClientUrl"] + "/user/login";
                identity.UserInteraction.LogoutUrl = Configuration["AuthClientUrl"] + "/user/logout";
            });

            //X509certificate
            if (_env.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                identityServer.AddSigningCredential(
                    new X509Certificate2(Path.Combine(_env.ContentRootPath, "idsrv.pfx"), "Wcnrg987."));
            }
            identityServer.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builer =>
                builer.UseSqlServer(authConnect, sql => sql.MigrationsAssembly(migrationsAssembly));
            }).AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(authConnect, sql => sql.MigrationsAssembly(migrationsAssembly));

                options.EnableTokenCleanup = true;
            }).AddRedisCaching(cache =>
            {
                cache.RedisConnectionMultiplexer = redisConnect;
                cache.KeyPrefix = "idsrv";
            }).AddProfileService<ProfileService>()
            .AddClientStoreCache<IdentityServer4.EntityFramework.Stores.ClientStore>()
            .AddResourceStoreCache<IdentityServer4.EntityFramework.Stores.ResourceStore>()
            .AddCorsPolicyCache<IdentityServer4.EntityFramework.Services.CorsPolicyService>()
            .AddResourceOwnerValidator<ResourceOwnerPwdValidator>();
            //services.AddDbContext<AccountDbContext>(context => context.UseSqlServer(Configuration["AccountCenterDb"]));
            return new AutofacServiceProvider(ConfigureContainer(services));
        }
        private static IContainer ConfigureContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            //builder.RegisterAssemblyModules(typeof(RepositoryModule).Assembly);
            //builder.RegisterAssemblyModules(typeof(MessagingModule).Assembly);
            //builder.RegisterAssemblyModules(typeof(DomainServicesModule).Assembly);
            //builder.RegisterAssemblyModules(typeof(ExternalServicesModule).Assembly);

            //builder.RegisterDefaultRebus("auth-server", (cfg, ctx) =>
            //    cfg.Routing(r =>
            //    {
            //        r.TypeBased()
            //            .MapAssemblyExternalCommandsOf<SendSmsCommand>(Infrastructure.Common.Shared.EndPointSpec
            //                .ApiQueueName);
            //    }).Timeouts(t => { t.StoreInSqlServer(ctx.Resolve<IConfiguration>()["RebusDb"], "rebus_timeouts"); }));


            return builder.Build();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            //app.UseAuthentication();

            app.UseMvc(); //使用默认路由
        }


        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.IdentityResources.Any())
                {
                    context.IdentityResources.Add(new IdentityResources.OpenId().ToEntity());
                    context.IdentityResources.Add(new IdentityResources.Profile().ToEntity());
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any())
                {
                    context.ApiResources.Add(new ApiResource("account-service", "账户").ToEntity());
                }
                if (!context.Clients.Any(x => x.ClientId == ClientIds.AuthWebClient))
                {
                    context.Clients.Add(new Client
                    {
                        ClientId = ClientIds.AuthWebClient,
                        ClientName = "PrivateTestMVVM",
                        ClientSecrets = new[]
                            {new IdentityServer4.Models.Secret("th!s!s@very$trong$ecretf0rantcloudplatform".Sha256())},
                        AllowedGrantTypes = GrantTypes.Code,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,
                        RequireClientSecret = false,
                        AccessTokenLifetime = 3600 * 24,
                        RedirectUris =
                        {
                        "http://localhost:8080/signin-callback/",
                        },
                        PostLogoutRedirectUris =
                        {
                        "http://localhost:8080/signout-callback/",
                        },
                        AllowedCorsOrigins =
                        {
                        "http://localhost:8080",
                        },
                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "account-service",
                        }
                    }.ToEntity());
                    context.SaveChanges();
                }
                if (!context.Clients.Any(x => x.ClientId == ClientIds.AuthWebClientForMobile))
                {
                    context.Clients.Add(new Client
                    {
                        ClientId = ClientIds.AuthWebClientForMobile,
                        ClientName = "PrivateTestMVVM.mobile",
                        ClientSecrets = new[]
                        {
                            new IdentityServer4.Models.Secret(
                                "th!s!s@very$trong$ecretf0rantcloudplatformmobile".Sha256())
                        },
                        AllowedGrantTypes = GrantTypes.Code,
                        RequireConsent = false,
                        AllowAccessTokensViaBrowser = true,
                        RequireClientSecret = false,
                        AccessTokenLifetime = 3600 * 24,
                        RedirectUris =
                        {
                            "http://localhost:8086/signin-callback/",
                        },
                        PostLogoutRedirectUris =
                        {
                            "http://localhost:8086/signout-callback/",
                        },
                        AllowedCorsOrigins =
                        {
                            "http://localhost:8086",
                        },
                        AllowedScopes = new[]
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "account-service",
                        }
                    }.ToEntity());
                    context.SaveChanges();
                }
            }
        }
    }
}

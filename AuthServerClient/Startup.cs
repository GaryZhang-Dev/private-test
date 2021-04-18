using AccountService.Domain;
using Api.infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IHostingEnvironment _env;

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
            services.AddMvcCore();

            var redisConnect = ConnectionMultiplexer.Connect(Configuration["RedisConfiguration"]);// "");q
            services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "MyPrivateTest"; })
                .PersistKeysToStackExchangeRedis(redisConnect, "DataProtction-Key");

            var identityServer = services.AddIdentityServer(identity =>
            {
                identity.IssuerUri = Configuration["IdentityServerUri"];
                identity.UserInteraction.LoginUrl = Configuration["AuthClientUrl"] + "/user/login";
                identity.UserInteraction.LogoutUrl = Configuration["AuthClientUrl"] + "/user/logout";
            });

            //X509certificate
            if (_env.IsDevelopment())
            {
                //identityServer.AddDeveloperSigningCredential();
                identityServer.AddSigningCredential(
                    new X509Certificate2(Path.Combine(_env.ContentRootPath, "idsrv.pfx"), "Wcnrg987."));
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
                    builder.UseSqlServer(authConnect,
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                options.EnableTokenCleanup = true;
            }).AddRedisCaching(cache =>
            {
                cache.RedisConnectionMultiplexer = redisConnect;
                cache.KeyPrefix = "IdServer4";
            }).AddProfileService<ProfileService>()
            .AddClientStoreCache<IdentityServer4.EntityFramework.Stores.ClientStore>()
            .AddResourceStoreCache<IdentityServer4.EntityFramework.Stores.ResourceStore>()
            .AddCorsPolicyCache<IdentityServer4.EntityFramework.Services.CorsPolicyService>()
            .AddResourceOwnerValidator<ResourceOwnerPwdValidator>();

            services.AddDbContext<AccountDbContext>(context => context.UseSqlServer(Configuration["AccountCenterDb"]));
            return new AutofacServiceProvider(ConfigureContainer(services));
        }
        private static IContainer ConfigureContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            //builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            //app.UseMvc(); //使用默认路由
        }
    }
}

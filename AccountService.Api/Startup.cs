using AccountService.Domain;
using Api.infrastructure;
using Api.infrastructure.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Reflection;

namespace AccountService.Api
{
    public class Startup
    {

        public readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            //ApplicationBootstrapper
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //var connectionString = Configuration["AuthServerDb"];
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = true,
                            ProcessExtensionDataNames = true
                        }
                    };
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //options.SerializerSettings.Converters.Add(new TrimmingConverter());
                });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisConfiguration"];
            });
            services.AddDbContext<AccountDbContext>(options =>
                options.UseSqlServer(Configuration["AccountDbServer"]));

            services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "private-test"; })
                .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(Configuration["RedisConfiguration"]),
                    "DP-Keys");
            this.ApplicationContainer = ConfigureContainer(services);
            return new AutofacServiceProvider(this.ApplicationContainer).RunStartupTasks();
        }
        private IContainer ConfigureContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            AutofacRegistration.Populate(builder, services);
            builder.Register(k => k.Resolve<AccountDbContext>())
               .As<DbContext>()
               .InstancePerLifetimeScope();
            return builder.Build();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseAuthentication();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AccountDbContext>();
                context.Database.EnsureCreated();
                //                 Seed the database.                                    
            }
        }
    }
}

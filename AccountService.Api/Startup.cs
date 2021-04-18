using AccountService.Domain;
using Api.infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AccountService.Api
{
    public class Startup
    {

        public readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            //ApplicationBootstrapper
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration["AuthorizationServer"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<AccountDbContext>(options =>
                options.UseSqlServer(Configuration["AccountDbServer"]));

            return new AutofacServiceProvider(this.ApplicationContainer);
        }
    }
}

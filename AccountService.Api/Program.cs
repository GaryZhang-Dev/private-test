using Api.infrastructure.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                 .UseUrlsForDevelopment("http://*:8955")
                 //.UseSerilog((hostingContext, loggerConfiguration) =>
                 //{
                 //    ApplicationBootstrapper.ConfigureLogging(hostingContext.Configuration, loggerConfiguration);
                 //})
                 .UseStartup<Startup>().Build().Run();
        }
    }
}

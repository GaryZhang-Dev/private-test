using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ApiGateWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            bool isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDev)
            {
                return WebHost.CreateDefaultBuilder(args)
                    .UseUrls("Http://*:8800")
                    .UseStartup<Startup>().ConfigureAppConfiguration((hosting, builder) => {
                        var oclotFilePath = hosting.HostingEnvironment.IsDevelopment() ? "Ocelot.json" : "Oclot.k8s.json";
                        builder.AddJsonFile(oclotFilePath);
                    });
            }
            else
            {
                return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration((hostingContext, builder) =>
                    {
                        var ocelotFilePath = hostingContext.HostingEnvironment.IsDevelopment() ? "Ocelot.json" : "Ocelot.k8s.json";
                        builder.AddJsonFile(ocelotFilePath, false, true);
                    });
            }
        }
    }
}

using Foundation.Core;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.infrastructure.Extensions
{
    public static class AppBuildExgtensions
    {
        
    }
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseUrlsForDevelopment(this IWebHostBuilder builder, params string[] urls)
        {
            return Environments.IsDevelopment() ? builder.UseUrls(urls) : builder;
         }
    }
}


using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.infrastructure.Extensions
{
    public static class ServiceProviderExtension
    {
        public static TServiceProvider RunStartupTasks<TServiceProvider>(this TServiceProvider serviceProvider) where TServiceProvider : IServiceProvider
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();
                foreach (var startupTask in startupTasks)
                {
                    startupTask.Run();
                }
            }
            return serviceProvider;
        }
    }
}

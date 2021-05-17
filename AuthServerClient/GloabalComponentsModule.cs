using Autofac;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerClient
{
    public class GloabalComponentsModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReturnUrlParser>().As<IReturnUrlParser>().InstancePerLifetimeScope();
        }
    }
}

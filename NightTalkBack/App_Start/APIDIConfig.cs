using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace NightTalkBack
{
    public class APIDIConfig : BaseDIConfig
    {
        public static System.Web.Http.Dependencies.IDependencyResolver apiResolver;
        public static void RegisterComponents()
        {
            ContainerBuilder builder = GetBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();

            apiResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configuration.DependencyResolver = apiResolver;
        }
    }
}
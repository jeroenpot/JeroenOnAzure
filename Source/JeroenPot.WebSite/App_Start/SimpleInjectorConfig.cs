using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JeroenPot.Website.Controllers;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace JeroenPot.Website
{
    public class SimpleInjectorConfig
    {
        public static Container Bootstrap(Container container)
        {
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            Website.Configuration.SimpleInjectorBootstrapper.ScanAssembly(container);
            Common.Configuration.SimpleInjectorBootstrapper.ScanAssembly(container);
            Twitter.Configuration.SimpleInjectorBootstrapper.ScanAssembly(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }
    }
}

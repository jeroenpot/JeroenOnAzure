using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;

namespace JeroenPot.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Container Container = new Container();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SimpleInjectorConfig.Bootstrap(Container);
        }
    }
}

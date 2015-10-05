using SimpleInjector;

namespace JeroenPot.Common.Configuration
{
    public static class SimpleInjectorBootstrapper
    {
        public static void ScanAssembly(Container container)
        {
            JeroenPot.SimpleInjector.AssemblyScanner.DependencyRegistration.Register(container, typeof(SimpleInjectorBootstrapper).Assembly);
        }
    }
}

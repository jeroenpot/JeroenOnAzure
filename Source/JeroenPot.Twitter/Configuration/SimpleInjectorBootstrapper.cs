using SimpleInjector;

namespace JeroenPot.Twitter.Configuration
{
    public static class SimpleInjectorBootstrapper
    {
        public static void ScanAssembly(Container container)
        {
            JeroenPot.SimpleInjector.AssemblyScanner.DependencyRegistration.Register(container, typeof(SimpleInjectorBootstrapper).Assembly);
        }
    }
}

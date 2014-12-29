namespace BlogEngine.Infrastructure
{
    using System;
    using Domain;
    using StructureMap;
    using StructureMap.Graph;

    public static class IoC
    {
        private static Lazy<IContainer> _container = new Lazy<IContainer>(InitializeContainer);

        private static IContainer InitializeContainer()
        {
            return new Container(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<Entity>();
                    scan.LookForRegistries();
                    scan.WithDefaultConventions();
                });
            });
        }

        public static IContainer Container { get { return _container.Value; } }
    }
}
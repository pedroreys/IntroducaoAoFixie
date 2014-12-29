namespace BlogEngine.Infrastructure
{
    using System.Data.Entity;
    using StructureMap.Configuration.DSL;

    public class BlogEngineRegistry : Registry
    {
        public BlogEngineRegistry()
        {
            For<DbContext>().Use<BlogEngineContext>();
        }
    }
}
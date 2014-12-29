namespace BlogEngineTests.IntegrationTests
{
    using System;
    using System.Configuration;
    using BlogEngine.Infrastructure;
    using Fixie;
    using Infrastructure;

    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        public void Execute(Fixture context, Action next)
        {
            DeleteAllData();
            next();
        }

        private static void DeleteAllData()
        {
            using (var container = IoC.Container.CreateChildContainer())
            using (var dbContext = new BlogEngineContext(ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString))
            {
                DatabaseDeleter.Instance.DeleteAllData(dbContext);
            }
        }

        public void Execute(Class context, Action next)
        {
            DeleteAllData();

            next();
        }
    }
}
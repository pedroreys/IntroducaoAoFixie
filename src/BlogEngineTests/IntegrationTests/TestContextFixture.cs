namespace BlogEngineTests.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using BlogEngine.Domain;
    using BlogEngine.Infrastructure;
    using Infrastructure;
    using StructureMap;

    public class TestContextFixture
    {
        private readonly IContainer _container;

        public TestContextFixture(IContainer container)
        {
            _container = container;
        }

        public void SetUp()
        {
            var dbContext = CreateNewDbContext();

            _container.Configure(cfg =>
            {
                cfg.For<DbContext>().Use(dbContext);
                cfg.For<BlogEngineContext>().Use(dbContext);
            });

            DatabaseDeleter.Initialize(dbContext);
        }

        public void SaveAll(params object[] entities)
        {
            Do(dbContext =>
            {
                foreach (var entity in entities)
                {
                    var entry = dbContext.ChangeTracker.Entries().FirstOrDefault(entityEntry => entityEntry.Entity == entity);
                    if (entry == null)
                    {
                        dbContext.Set(entity.GetType()).Add(entity);
                    }
                }
            });
        }

        public void Reload<TPersistentObject>(ref TPersistentObject entity)
            where TPersistentObject : Entity
        {
            if (entity == null)
                return;

            var dbContext = CreateNewDbContext();

            entity = dbContext.Set<TPersistentObject>().Find(entity.Id);
        }

        public void Delete(Entity entity)
        {
            Do(dbContext => dbContext.Set(entity.GetType()).Remove(entity));
        }

        public void Do(Action action)
        {
            var dbContext = _container.GetInstance<BlogEngineContext>();
            try
            {
                dbContext.BeginTransaction();
                action();
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        public void Do(Action<DbContext> action)
        {
            var dbContext = _container.GetInstance<BlogEngineContext>();
            try
            {
                dbContext.BeginTransaction();
                action(dbContext);
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        public void DoClean(Action<DbContext> action)
        {
            var dbContext = CreateNewDbContext();

            try
            {
                dbContext.BeginTransaction();
                action(dbContext);
                dbContext.CloseTransaction();
            }
            catch (Exception e)
            {
                dbContext.CloseTransaction(e);
                throw;
            }
        }

        private static BlogEngineContext CreateNewDbContext()
        {
            return new BlogEngineContext(ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString);
        }
    }
}
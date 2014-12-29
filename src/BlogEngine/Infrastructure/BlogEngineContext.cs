namespace BlogEngine.Infrastructure
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Domain;

    public class BlogEngineContext : DbContext
    {
        private DbContextTransaction _currentTransaction;

        public BlogEngineContext(string connectionString)
            : base(connectionString)
        {
            
        }

        public override int SaveChanges()
        {
            var entitiesBeingAdded = ChangeTracker
                                .Entries<Entity>()
                                .Where(x => x.State == EntityState.Added)
                                .Select(x => x.Entity)
                                .ToArray();
            foreach (var entity in entitiesBeingAdded)
            {
                entity.Timestamp = SystemTime.Now;
            }

            
            return base.SaveChanges();
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction();
        }

        public void CloseTransaction()
        {
            CloseTransaction(null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                if (_currentTransaction != null)
                {
                    _currentTransaction.Commit();
                }

            }
            catch (Exception)
            {
                if (_currentTransaction != null && _currentTransaction.UnderlyingTransaction.Connection != null)
                {
                    _currentTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BlogEngineContext>(null);
            modelBuilder.Configurations.AddFromAssembly(typeof(BlogEngineContext).Assembly);

            modelBuilder.Conventions.Add<ForeignKeyNamingConvention>();
        }
    }
}
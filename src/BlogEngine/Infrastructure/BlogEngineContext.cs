namespace BlogEngine.Infrastructure
{
    using System.Data.Entity;
    using System.Linq;
    using Domain;

    public class BlogEngineContext : DbContext
    {
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BlogEngineContext>(null);
            modelBuilder.Configurations.AddFromAssembly(typeof(BlogEngineContext).Assembly);
        }
    }
}
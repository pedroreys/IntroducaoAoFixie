namespace BlogEngine.Infrastructure
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    public abstract class EntityMapping<T> : EntityTypeConfiguration<T> where T : Entity
    {
        public EntityMapping()
        {
            Property(x => x.Timestamp);
        }
    }

    public class AuthorMapping : EntityMapping<Author>
    {
        
    }

    public class BlogMapping : EntityMapping<Blog>
    {
        
    }

    public class BlogPostMapping : EntityMapping<BlogPost>
    {
        
    }

    public class CommentMapping : EntityMapping<Comment>
    {
        
    }
    
}
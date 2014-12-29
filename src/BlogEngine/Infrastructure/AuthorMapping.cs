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
        public BlogPostMapping()
        {
            Property(x => x.Content).HasColumnName("PostContent");
        }
    }

    public class CommentMapping : EntityMapping<Comment>
    {
        public CommentMapping()
        {
            Property(x => x.Content).HasColumnName("CommentContent");
        }
    }
    
}
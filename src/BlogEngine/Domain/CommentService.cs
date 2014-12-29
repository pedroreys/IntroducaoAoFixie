namespace BlogEngine.Domain
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class CommentService : ICommentService
    {
        private readonly DbContext _db;

        public CommentService(DbContext db)
        {
            _db = db;
        }

        public List<Comment> GetAllCommentsFor(BlogPost post)
        {
            var comments = _db.Set<Comment>()
                .Where(c => c.BlogPost.Id == post.Id)
                .ToList();

            return comments;
        }
    }

    public interface ICommentService
    {
        List<Comment> GetAllCommentsFor(BlogPost post);
    }
}
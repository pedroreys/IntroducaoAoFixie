namespace BlogEngine.Domain
{
    using System.Collections.Generic;

    public class BlogPost : Entity
    {
        public BlogPost()
        {
            Comments = new List<Comment>();
        }

        public BlogPost(Blog blog, Author author, string title, string description, string content)
            : this()
        {
            Blog = blog;
            Author = author;
            Title = title;
            Description = description;
            Content = content;
        }

        public virtual Blog Blog { get; set; }
        public virtual Author Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public virtual List<Comment> Comments { get; protected set; }

        public Comment AddComment(Author author, string content)
        {
            var comment = new Comment(author, this, content);
            Comments.Add(comment);
            return comment;
        }
    }
}
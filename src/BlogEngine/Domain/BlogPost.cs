namespace BlogEngine.Domain
{
    public class BlogPost : Entity
    {
        protected BlogPost()
        {
            //For EF only
        }

        public BlogPost(Blog blog, Author author, string title, string description, string content)
        {
            Blog = blog;
            Author = author;
            Title = title;
            Description = description;
            Content = content;
        }

        public virtual Blog Blog { get; protected set; }
        public virtual Author Author { get; protected set; }
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public string Content { get; protected set; }
    }
}
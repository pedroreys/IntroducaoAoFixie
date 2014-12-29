namespace BlogEngine.Domain
{
    public class Comment : Entity
    {
        public Comment()
        {
            
        }

        public Comment(Author author, BlogPost post, string content)
        {
            Author = author;
            BlogPost = post;
            Content = content;
        }

        public virtual Author Author { get; set; }
        public virtual BlogPost BlogPost { get; set; }
        public string Content { get; set; }
    }
}
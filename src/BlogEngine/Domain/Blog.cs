namespace BlogEngine.Domain
{
    using System.Collections.Generic;
    using System.Security.Cryptography;

    public class Blog : Entity
    {
        public Blog()
        {
            Posts = new List<BlogPost>();
        }

        public string Name { get; set; }

        public virtual List<BlogPost> Posts { get; protected set; }

        public BlogPost CreateBlogPost(Author author, string title, string description, string content)
        {
            var post = new BlogPost(this, author, title, description, content);
            Posts.Add(post);
            return post;
        }
    }
}
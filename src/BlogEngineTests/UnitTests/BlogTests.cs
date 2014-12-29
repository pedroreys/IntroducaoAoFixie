namespace BlogEngineTests.UnitTests
{
    using BlogEngine.Domain;
    using Shouldly;

    public class BlogTests
    {
        public void Should_add_blogpost_to_blog(Blog blog, Author author)
        {
            const string title = "Blog post Title";
            const string description = "Blog Post Description";
            const string content = "This is my blog content";
            var post = blog.CreateBlogPost(author, title, description, content);

            blog.Posts.Count.ShouldBe(1);
            blog.Posts.ShouldContain(post);
            post.Author.ShouldBe(author);
            post.Title.ShouldBe(title);
            post.Description.ShouldBe(description);
            post.Content.ShouldBe(content);
        } 
    }
}
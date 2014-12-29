namespace BlogEngineTests.IntegrationTests.Domain
{
    using System.Collections.Generic;
    using BlogEngine.Domain;
    using Shouldly;

    public class CommentServiceTests
    {
        private readonly Author _author1;
        private readonly Author _author2;
        private List<Comment> _loadedComments;

        public CommentServiceTests(TestContextFixture fixture, BlogPost blogPost, Author author1, Author author2, ICommentService commentService)
        {
            _author1 = author1;
            _author2 = author2;
            blogPost.AddComment(author1, "Meu comentario");
            blogPost.AddComment(author2, "Outro comentario");
            fixture.SaveAll(blogPost);

            _loadedComments = commentService.GetAllCommentsFor(blogPost);
        }

        public void Should_retrieve_all_comments()
        {
            _loadedComments.Count.ShouldBe(2);
        }

        public void Should_retrieve_comment_from_author_one()
        {
            _loadedComments.ShouldContain(c => c.Author == _author1);
        }

        public void SHould_retrieve_comment_from_author_two()
        {
            _loadedComments.ShouldContain(c => c.Author == _author2);
        }

    }
}
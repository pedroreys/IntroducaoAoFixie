namespace BlogEngineTests.Infrastructure
{
    using System;
    using BlogEngine.Domain;
    using BlogEngine.Infrastructure;
    using Shouldly;

    public class AuthorMappingTests
    {
        public void Should_persist_author()
        {
            var context = new BlogEngineContext("Server=localhost;Database=FixieBlogEngine;Trusted_Connection=True");

            var originalAuthor = new Author()
            {
                Name = "Homer Simpson"
            };
            var timestamp = new DateTimeOffset(2014, 12, 29, 19, 0, 0, TimeSpan.Zero);

            using (SystemTime.WithNowAs(timestamp))
            {
                context.Set<Author>().Add(originalAuthor);
                context.SaveChanges();
            }

            var newContext = new BlogEngineContext("Server=localhost;Database=FixieBlogEngine;Trusted_Connection=True");

            var persistedAuthor = newContext.Set<Author>().Find(originalAuthor.Id);

            persistedAuthor.IsPersistent.ShouldBe(true);
            persistedAuthor.Name.ShouldBe(originalAuthor.Name);
            persistedAuthor.Timestamp.ShouldBe(timestamp);
        }
    }
}
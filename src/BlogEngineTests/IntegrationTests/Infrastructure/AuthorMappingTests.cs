namespace BlogEngineTests.IntegrationTests.Infrastructure
{
    using System;
    using BlogEngine.Domain;
    using BlogEngine.Infrastructure;
    using Shouldly;

    public class AuthorMappingTests
    {
        public void Should_persist_author(TestContextFixture fixture, Author author)
        {
            var timestamp = new DateTimeOffset(2014, 12, 29, 19, 0, 0, TimeSpan.Zero);

            using (SystemTime.WithNowAs(timestamp))
            {
                fixture.SaveAll(author);
            }

            fixture.DoClean(db =>
            {
                var persistedAuthor = db.Set<Author>().Find(author.Id);

                persistedAuthor.IsPersistent.ShouldBe(true);
                persistedAuthor.Name.ShouldBe(author.Name);
                persistedAuthor.Timestamp.ShouldBe(timestamp);
            });
        }
    }
}
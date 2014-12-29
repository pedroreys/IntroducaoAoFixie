namespace BlogEngineTests.UnitTests.Testing
{
    using BlogEngineTests.Testing;
    using Ploeh.AutoFixture;

    public class UnitTestFixtureCustomization : AutoFixtureCustomization
    {
        protected override void CustomizeFixture(IFixture fixture)
        {
            // no-op
        }
    }
}
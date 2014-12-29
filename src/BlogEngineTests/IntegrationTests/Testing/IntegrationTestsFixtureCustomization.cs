namespace BlogEngineTests.IntegrationTests.Testing
{
    using BlogEngine.Infrastructure;
    using BlogEngineTests.Testing;
    using Ploeh.AutoFixture;

    public class IntegrationTestsFixtureCustomization : AutoFixtureCustomization
    {
        protected override void CustomizeFixture(IFixture fixture)
        {
            var container = IoC.Container;

            var contextFixture = new TestContextFixture(container);
            contextFixture.SetUp();
            fixture.Customizations.Add(new TestContextFixtureBuilder(contextFixture));
            fixture.Customizations.Add(new ContainerBuilder(container)); // always last
        }
    }
}
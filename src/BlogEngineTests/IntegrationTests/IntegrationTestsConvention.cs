namespace BlogEngineTests.IntegrationTests
{
    using Fixie;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;
    using Testing;

    public abstract class IntegrationTestsConvention : Convention
    {
        public IntegrationTestsConvention()
        {
            var fixture = InitializeAutoFixture();
            Classes
                .InTheSameNamespaceAs<IntegrationTestsConvention>();

            ClassExecution
                .UsingFactory(t => new SpecimenContext(fixture).Resolve(t));

            Parameters
                .Add(method => method.ResolveParametersWith(fixture));
        }

        private IFixture InitializeAutoFixture()
        {
            return new Ploeh.AutoFixture.Fixture().Customize(new IntegrationTestsFixtureCustomization());
        }
    }
}
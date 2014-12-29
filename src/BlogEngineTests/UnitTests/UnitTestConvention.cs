namespace BlogEngineTests.UnitTests
{
    using Fixie;
    using Ploeh.AutoFixture;
    using Testing;
    using Fixture = Ploeh.AutoFixture.Fixture;

    public class UnitTestConvention : Convention
    {
        public UnitTestConvention()
        {
            var fixture = InitializeAutoFixture();
            Classes
                .InTheSameNamespaceAs<UnitTestConvention>();

            ClassExecution
                .CreateInstancePerCase();

            Parameters
                .Add(method => method.ResolveParametersWith(fixture));
        }

        private IFixture InitializeAutoFixture()
        {
            return new Fixture().Customize(new UnitTestFixtureCustomization());
        }
    }
}
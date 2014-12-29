namespace BlogEngineTests.IntegrationTests.Testing
{
    using System;
    using Ploeh.AutoFixture.Kernel;

    public class TestContextFixtureBuilder : ISpecimenBuilder
    {
        private readonly TestContextFixture _fixture;

        public TestContextFixtureBuilder(TestContextFixture fixture)
        {
            _fixture = fixture;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;

            if (t == null || t != typeof(TestContextFixture))
                return new NoSpecimen(request);

            return _fixture;
        }
    }
}
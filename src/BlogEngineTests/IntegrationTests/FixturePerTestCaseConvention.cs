namespace BlogEngineTests.IntegrationTests
{
    public class FixturePerTestCaseConvention : IntegrationTestsConvention
    {
        public FixturePerTestCaseConvention()
        {
            Classes
                .ConstructorDoesntHaveArguments();

            ClassExecution
                .CreateInstancePerCase();

            FixtureExecution
                .Wrap<DeleteData>();

        }
    }
}
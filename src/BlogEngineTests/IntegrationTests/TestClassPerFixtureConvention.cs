namespace BlogEngineTests.IntegrationTests
{
    public class TestClassPerFixtureConvention : IntegrationTestsConvention
    {
        public TestClassPerFixtureConvention()
        {
            Classes
                .ConstructorHasArguments();

            ClassExecution
                .CreateInstancePerClass()
                .Wrap<DeleteData>();
        }
    }
}
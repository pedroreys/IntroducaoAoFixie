namespace BlogEngineTests.Testing
{
    using System.Reflection;
    using Ploeh.AutoFixture.Kernel;

    public class BoolAlwaysTrueBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var property = request as PropertyInfo;

            if (property != null && property.PropertyType == typeof(bool))
                return true;

            return new NoSpecimen(request);
        }
    }
}
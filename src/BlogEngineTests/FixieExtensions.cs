namespace BlogEngineTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fixie;
    using Fixie.Conventions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public static class FixieExtensions
    {
        public static ClassExpression InTheSameNamespaceAs<TTargetType>(this ClassExpression filter)
        {
            return filter.InTheSameNamespaceAs(typeof (TTargetType));
        }

        public static ClassExpression InTheSameNamespaceAs(this ClassExpression filter, params Type[] targetTypes)
        {
            return filter.Where(type => targetTypes.Any(targetType => type.IsInNamespace(targetType.Namespace)));
        }

        public static IEnumerable<object[]> ResolveParametersWith(this MethodInfo methodInfo, IFixture fixture)
        {
            return Enumerable.Repeat(methodInfo
                                        .GetParameters()
                                        .Select(parameterInfo => new SpecimenContext(fixture).Resolve(parameterInfo))
                                        .ToArray()
                                , 1);
        }

        public static ClassExpression ConstructorHasArguments(this ClassExpression filter)
        {
            return filter.Where(t => t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(x => x.GetParameters().Any()));
        }

        public static ClassExpression ConstructorDoesntHaveArguments(this ClassExpression filter)
        {
            return filter.Where(t => t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .All(x => x.GetParameters().Length == 0)
            );
        }
    }
}
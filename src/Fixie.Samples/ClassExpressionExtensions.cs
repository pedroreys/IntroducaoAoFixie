namespace Fixie.Samples
{
    using System;
    using System.Linq;
    using Conventions;

    public static class ClassExpressionExtensions
    {
        public static ClassExpression InTheSameNamespaceAs<TTargetType>(this ClassExpression filter)
        {
            return filter.InTheSameNamespaceAs(typeof (TTargetType));
        }

        public static ClassExpression InTheSameNamespaceAs(this ClassExpression filter, params Type[] targetTypes)
        {
            return filter.Where(type => targetTypes.Any(targetType => type.IsInNamespace(targetType.Namespace)));
        }
    }
}
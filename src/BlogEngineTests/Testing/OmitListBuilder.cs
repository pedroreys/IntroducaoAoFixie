﻿namespace BlogEngineTests.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ploeh.AutoFixture.Kernel;

    public class OmitListBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi != null)
            {
                var isIEnumerable = pi.PropertyType
                    .GetInterfaces()
                    .Any(t => t.IsGenericType
                              && t.GetGenericTypeDefinition() == typeof(IList<>));

                if (isIEnumerable)
                {
                    var genericArguments = pi.PropertyType.GetGenericArguments();
                    var concreteType = typeof(List<>).MakeGenericType(genericArguments);
                    var instance = Activator.CreateInstance(concreteType);
                    return instance;
                }
            }

            return new NoSpecimen(request);
        }
    }
}
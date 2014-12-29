﻿namespace BlogEngineTests.IntegrationTests.Testing
{
    using System;
    using Ploeh.AutoFixture.Kernel;
    using StructureMap;

    public class ContainerBuilder : ISpecimenBuilder
    {
        private readonly IContainer _container;

        public ContainerBuilder(IContainer container)
        {
            _container = container;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;

            if (type == null || type.IsPrimitive)
            {
                return new NoSpecimen(request);
            }

            var service = _container.TryGetInstance(type);

            return service ?? new NoSpecimen(request);
        }
    }
}
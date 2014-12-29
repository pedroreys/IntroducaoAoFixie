﻿using System;

namespace Fixie.Samples.IoC
{
    public class CustomConvention : Convention
    {
        readonly IoCContainer container;

        public CustomConvention()
        {
            container = InitContainerForIntegrationTests();

            Classes
                .InTheSameNamespaceAs(typeof(CustomConvention))
                .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid());

            ClassExecution
                .CreateInstancePerClass()
                .UsingFactory(GetFromContainer)
                .SortCases((caseA, caseB) => String.Compare(caseA.Name, caseB.Name, StringComparison.Ordinal));
        }

        static IoCContainer InitContainerForIntegrationTests()
        {
            var container = new IoCContainer();
            container.Add(typeof(IDatabase), new RealDatabase());
            container.Add(typeof(IThirdPartyService), new FakeThirdPartyService());
            return container;
        }

        object GetFromContainer(Type testClass)
        {
            return container.Get(testClass);
        }
    }
}
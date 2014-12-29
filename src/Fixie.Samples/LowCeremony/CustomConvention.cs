﻿using System;
using System.Linq;
using System.Reflection;

namespace Fixie.Samples.LowCeremony
{
    public class CustomConvention : Convention
    {
        static readonly string[] LifecycleMethods = { "FixtureSetUp", "FixtureTearDown", "SetUp", "TearDown" };

        public CustomConvention()
        {
            Classes
                .InTheSameNamespaceAs(typeof(CustomConvention))
                .NameEndsWith("Tests");

            Methods
                .Where(method => method.IsVoid())
                .Where(method => LifecycleMethods.All(x => x != method.Name));

            ClassExecution
                .CreateInstancePerClass()
                .SortCases((caseA, caseB) => String.Compare(caseA.Name, caseB.Name, StringComparison.Ordinal));

            FixtureExecution
                .Wrap<CallFixtureSetUpTearDownMethodsByName>();

            CaseExecution
                .Wrap<CallSetUpTearDownMethodsByName>();
        }

        class CallSetUpTearDownMethodsByName : CaseBehavior
        {
            public void Execute(Case @case, Action next)
            {
                @case.Class.TryInvoke("SetUp", @case.Fixture.Instance);
                next();
                @case.Class.TryInvoke("TearDown", @case.Fixture.Instance);
            }
        }

        class CallFixtureSetUpTearDownMethodsByName : FixtureBehavior
        {
            public void Execute(Fixture fixture, Action next)
            {
                fixture.Class.Type.TryInvoke("FixtureSetUp", fixture.Instance);
                next();
                fixture.Class.Type.TryInvoke("FixtureTearDown", fixture.Instance);
            }
        }
    }

    public static class BehaviorBuilderExtensions
    {
        public static void TryInvoke(this Type type, string method, object instance)
        {
            var lifecycleMethod =
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .SingleOrDefault(x => x.HasSignature(typeof(void), method));

            if (lifecycleMethod == null)
                return;

            try
            {
                lifecycleMethod.Invoke(instance, null);
            }
            catch (TargetInvocationException exception)
            {
                throw new PreservedException(exception.InnerException);
            }
        }
    }
}
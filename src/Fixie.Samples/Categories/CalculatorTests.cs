using Should;

namespace Fixie.Samples.Categories
{
    using System;

    public class CalculatorTests
    {
        readonly Calculator calculator;

        public CalculatorTests()
        {
            calculator = new Calculator();
        }

        [CategoryA]
        public void ShouldAdd()
        {
            Console.WriteLine("Running Category A test");
            calculator.Add(2, 3).ShouldEqual(5);
        }

        [CategoryB]
        public void ShouldSubtract()
        {
            Console.WriteLine("Running Category B test");
            calculator.Add(2, 3).ShouldEqual(5);
        }
    }
}
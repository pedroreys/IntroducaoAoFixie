namespace FixieCalculator.Tests
{
    using System;
    using System.Threading.Tasks;
    using Shouldly;

    public class CalculatorTests
    {
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            _calculator = new Calculator();
        }

        public void Should_add_two_numbers()
        {
            var result = _calculator.Add(1, 2);
            result.ShouldBe(3);
        }

        public void Should_subract_two_numbers()
        {
            var result = _calculator.Subtract(10, 4);
            result.ShouldBe(6);
        }

        public void Should_multiply_two_numbers()
        {
            var result = _calculator.Multiply(2, 4);
            result.ShouldBe(8);
        }

        public void Should_divide_two_numbers()
        {
            var result = _calculator.Divide(10,2);
            result.ShouldBe(5);
        }

        public async Task Should_add_two_numbers_async()
        {
            var result = await _calculator.AddAsync(4, 5);
            result.ShouldBe(9);
        }

        private void Should_not_run_private_methods()
        {
            throw new Exception("Fixie Shouldn't run private methods");
        }

        private int Should_not_run_non_void_methods()
        {
            throw new Exception("Fixie shouldn't run non-void methods");
        }
    }

    public class WrongTestFixtureName
    {
        public WrongTestFixtureName()
        {
            throw new Exception("Fixie shouldn't instanciate classes that it doesn't match the test class name convention");
        }

        public void Should_not_run_this_test()
        {
            throw new Exception("Fixie shouldn't run this test");
        }
    }
}

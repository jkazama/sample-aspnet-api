using Xunit;
using Sample.Utils;
using System;

namespace Sample.Test.Utils
{
    public class CalculatorTest
    {
        [Fact]
        public void PassingTest()
        {
			var calc = Calculator.Init();
            Console.WriteLine(calc.Add(3).Subtract(2).Multiply(8).DivideBy(2).DecimalValue());
			Assert.True(true);
		}
	}
}
using Xunit;
using Sample.Utils;
using System;

namespace Sample.Test.Utils
{
    public class CalculatorTest
    {
        [Fact]
        public void Calculate()
        {
			var calc = Calculator.Init();
            Assert.Equal(4, calc.Add(3).Subtract(2).Multiply(8).DivideBy(2).DecimalValue());
		}
	}
}
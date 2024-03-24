using Calculator_V3421048.ViewModel;
using NUnit.Framework;

namespace Calculator_V3421048.Test
{
	public class CalculationTests
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		private CalculationHandler calculator;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		[SetUp]
		public void Setup()
		{
			calculator = new CalculationHandler();
		}

		[Test]
		public void AdditionTest ()
		{
			double a = 30;
			double b = 20;

			double expectedResult = 50;

			double result = calculator.Addition(a,b);

			Assert.Equals(expectedResult, result);
		}


		[Test]
		public void SubstractionTest ()
		{
			double a = 30;
			double b = 20;

			double expectedResult = 10;

			double result = calculator.Subtraction(a, b);

			Assert.Equals(expectedResult, result);
		}


		[Test]
		public void MultiplyTest ()
		{
			double a = 30;
			double b = 20;

			double expectedResult = 600;

			double result = calculator.Multiplication(a, b);

			Assert.Equals(expectedResult, result);
		}


		[Test]
		public void DivisionTest ()
		{
			double a = 25;
			double b = 5;

			double expectedResult = 5;

			double result = calculator.Division(a, b);

			Assert.Equals(expectedResult, result);
		}


		[Test]
		public void ModulusTest ()
		{
			double a = 10;
			double b = 4;

			double expectedResult = 2;

			double result = calculator.Modulus(a, b);

			Assert.Equals(expectedResult, result);
		}
	}
}

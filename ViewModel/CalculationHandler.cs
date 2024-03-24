namespace Calculator_V3421048.ViewModel
{
	/// <summary>
	/// Handles mathematical calculations.
	/// </summary>
	internal class CalculationHandler
	{
		/// <summary>
		/// Performs the specified calculation based on the given operator.
		/// </summary>
		/// <param name="number1">The first operand.</param>
		/// <param name="number2">The second operand.</param>
		/// <param name="calcOperator">The operator specifying the calculation to perform.</param>
		/// <returns>The result of the calculation.</returns>
		public double Calculate (double number1, double number2, char calcOperator)
		{
			switch (calcOperator)
			{
				case '+':
					return Addition(number1, number2);
				case '-':
					return Subtraction(number1, number2);
				case 'x':
					return Multiplication(number1, number2);
				case '÷':
					return Division(number1, number2);
				case '%':
					return Modulus(number1, number2);
				default:
					return 0;
			}
		}

		/// <summary>
		/// Adds two numbers.
		/// </summary>
		/// <param name="number1">The first number.</param>
		/// <param name="number2">The second number.</param>
		/// <returns>The sum of the two numbers.</returns>
		public double Addition (double number1, double number2) => number1 + number2;

		/// <summary>
		/// Subtracts one number from another.
		/// </summary>
		/// <param name="number1">The minuend.</param>
		/// <param name="number2">The subtrahend.</param>
		/// <returns>The result of the subtraction.</returns>
		public double Subtraction (double number1, double number2) => number1 - number2;

		/// <summary>
		/// Multiplies two numbers.
		/// </summary>
		/// <param name="number1">The first factor.</param>
		/// <param name="number2">The second factor.</param>
		/// <returns>The product of the multiplication.</returns>
		public double Multiplication (double number1, double number2) => number1 * number2;

		/// <summary>
		/// Divides one number by another.
		/// </summary>
		/// <param name="number1">The dividend.</param>
		/// <param name="number2">The divisor.</param>
		/// <returns>The result of the division.</returns>
		public double Division (double number1, double number2) => number1 / number2;

		/// <summary>
		/// Computes the remainder of dividing one number by another.
		/// </summary>
		/// <param name="number1">The dividend.</param>
		/// <param name="number2">The divisor.</param>
		/// <returns>The remainder of the division.</returns>
		public double Modulus (double number1, double number2) => number1 % number2;
	}
}

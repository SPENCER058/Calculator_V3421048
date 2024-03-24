namespace Calculator_V3421048.ViewModel
{
	internal class CalculationHandler
	{
		public long Calculate (long number1, long number2, char calcOperator)
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

		public long Addition (long number1, long number2) => number1 + number2;
		public long Subtraction (long number1, long number2) => number1 - number2;
		public long Multiplication (long number1, long number2) => number1 * number2;
		public long Division (long number1, long number2) => number1 / number2;
		public long Modulus (long number1, long number2) => number1 % number2;

	}
}

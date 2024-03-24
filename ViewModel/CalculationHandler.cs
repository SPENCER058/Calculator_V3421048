namespace Calculator_V3421048.ViewModel
{
	internal class CalculationHandler
	{
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

		public double Addition (double number1, double number2) => number1 + number2;
		public double Subtraction (double number1, double number2) => number1 - number2;
		public double Multiplication (double number1, double number2) => number1 * number2;
		public double Division (double number1, double number2) => number1 / number2;
		public double Modulus (double number1, double number2) => number1 % number2;

	}
}

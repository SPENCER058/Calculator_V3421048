
using NUnit.Framework.Constraints;

namespace Calculator_V3421048.Model
{
	public class CalculationHistory
	{
		public int Id { get; set; }
		public double FirstNumber { get; set; }
		public double SecondNumber { get; set; }
		public Char CalculationOperator { get; set; }
		public double Result { get; set; }

		public CalculationHistory () { }

		public CalculationHistory (double number1, double number2, double result, char calcOperator)
		{
			FirstNumber = number1;
			SecondNumber = number2;
			Result = result;
			CalculationOperator = calcOperator;
		}

		public override string ToString ()
		{
			return $"{FirstNumber} {CalculationOperator} {SecondNumber} = {Result}";
		}
	}
}

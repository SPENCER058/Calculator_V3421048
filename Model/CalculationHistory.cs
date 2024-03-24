namespace Calculator_V3421048.Model
{
	/// <summary>
	/// Represents a calculation history entry.
	/// </summary>
	public class CalculationHistory
	{
		/// <summary>
		/// Gets or sets the unique identifier of the calculation.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the first number in the calculation.
		/// </summary>
		public double FirstNumber { get; set; }

		/// <summary>
		/// Gets or sets the second number in the calculation.
		/// </summary>
		public double SecondNumber { get; set; }

		/// <summary>
		/// Gets or sets the operator used in the calculation.
		/// </summary>
		public char CalculationOperator { get; set; }

		/// <summary>
		/// Gets or sets the result of the calculation.
		/// </summary>
		public double Result { get; set; }

		/// <summary>
		/// Default constructor for <see cref="CalculationHistory"/>.
		/// </summary>
		public CalculationHistory () { }

		/// <summary>
		/// Parameterized constructor for <see cref="CalculationHistory"/>.
		/// </summary>
		/// <param name="number1">The first number in the calculation.</param>
		/// <param name="number2">The second number in the calculation.</param>
		/// <param name="result">The result of the calculation.</param>
		/// <param name="calcOperator">The operator used in the calculation.</param>
		public CalculationHistory (double number1, double number2, double result, char calcOperator)
		{
			FirstNumber = number1;
			SecondNumber = number2;
			Result = result;
			CalculationOperator = calcOperator;
		}

		/// <summary>
		/// Returns a string representation of the calculation history entry.
		/// </summary>
		/// <returns>A string representing the calculation history entry.</returns>
		public override string ToString ()
		{
			return $"{FirstNumber} {CalculationOperator} {SecondNumber} = {Result}";
		}
	}
}

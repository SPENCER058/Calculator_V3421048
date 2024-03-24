using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Calculator_V3421048.Model;
using Calculator_V3421048.MVVM;

namespace Calculator_V3421048.ViewModel
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Global Variables
		/// <summary>
		/// Represents the first number in the calculation.
		/// </summary>
		private double number1 = 0;

		/// <summary>
		/// Represents the second number in the calculation.
		/// </summary>
		private double number2 = 0;

		/// <summary>
		/// Represents the result of the calculation.
		/// </summary>
		private double result = 0;

		/// <summary>
		/// Indicates whether the current number is the first number in the calculation.
		/// </summary>
		private bool isFirstNumber = true;

		/// <summary>
		/// Indicates whether the calculation is loaded from history.
		/// </summary>
		private bool isLoaded = false;

		/// <summary>
		/// Represents the operator used in the calculation.
		/// </summary>
		private char calcOperator = ' ';
		#endregion

		/// <summary>
		/// Handles mathematical calculations.
		/// </summary>
		private readonly CalculationHandler _calculationHandler;

		/// <summary>
		/// Provides access to SQLite database for storing calculation history.
		/// </summary>
		private readonly SQLiteDataAccess _dataAccess;

		/// <summary>
		/// Collection of calculation history entries.
		/// </summary>
		public ObservableCollection<CalculationHistory> HistoryEntries { get; set; } = new ObservableCollection<CalculationHistory>();

		#region Command Binding

		/// <summary>
		/// Command for handling digit (number) input.
		/// </summary>
		public ICommand DigitCommand => new RelayCommand(HandleDigit);

		private void HandleDigit (object sender)
		{
			double currentNumber = (isFirstNumber) ? number1 : number2;

			if (isLoaded)
			{
				UpdateCoreData(result,0,calcOperator,0,false);

				UpdateLastHistoryText();
			}

			if (result != 0)
			{
				ClearAll();
				currentNumber = 0;
			}

			if (!isFirstNumber && number2 == 0)
			{
				_inputDisplayText = "0";
				InputDisplayText = "0";
				currentNumber = 0;
			}

			if (currentNumber < long.MaxValue * 0.001)
			{
				currentNumber = (currentNumber * 10) + Convert.ToInt64(sender);

				if (isFirstNumber)
				{
					number1 = currentNumber;
				}
				else
				{
					number2 = currentNumber;
				}

				InputDisplayText = FormatNumber(currentNumber);
			}
		}

		/// <summary>
		/// Command for handling calculation operator input.
		/// </summary>
		public ICommand OperatorCommand => new RelayCommand(HandleOperator);

		private void HandleOperator (object sender)
		{
			if (isLoaded)
			{
				UpdateCoreData(result, 0, 0);

				isLoaded = false;
			}

			if (result != 0)
			{
				UpdateCoreData(result, 0, 0);
			}

			calcOperator = Convert.ToChar(sender);
			UpdateLastHistoryText();
			isFirstNumber = false;
			InputDisplayText = "0";
		}

		/// <summary>
		/// Command for performing calculation.
		/// </summary>
		public ICommand EqualsCommand => new RelayCommand(HandleEqual);

		private void HandleEqual (object sender)
		{
			if (result != 0)
			{
				number1 = result;
			}

			if (calcOperator != ' ' && !isFirstNumber)
			{
				result = _calculationHandler.Calculate(number1, number2, calcOperator);
				InputDisplayText = FormatNumber(result);
				UpdateLastHistoryText();

				CalculationHistory historyEntry = new CalculationHistory(number1, number2, result, calcOperator);

				HistoryEntries.Add(historyEntry);

				// Save calculation history to the database
				_dataAccess.SaveCalculationHistory(historyEntry);
			}
		}

		/// <summary>
		/// Command for converting positive to negative value and vice versa.
		/// </summary>
		public ICommand ConvertPositiveNegativeCommand => new RelayCommand(HandleConvertPositiveNegativeValue);
		
		private void HandleConvertPositiveNegativeValue (object sender)
		{
			if (isFirstNumber) number1 *= -1; number2 *= -1;
			InputDisplayText = FormatNumber(isFirstNumber ? number1 : number2);
		}

		/// <summary>
		/// Command for clearing the input display.
		/// </summary>
		public ICommand ClearCommand => new RelayCommand(execute => ClearAll());

		private void ClearAll ()
		{
			number1 = 0;
			number2 = 0;
			result = 0;
			isFirstNumber = true;
			calcOperator = ' ';

			InputDisplayText = "0";
			UpdateLastHistoryText();
		}

		/// <summary>
		/// Command for handling backspace.
		/// </summary>
		public ICommand BackspaceCommand => new RelayCommand(execute => HandleBackSpace());

		/// <summary>
		/// Command for clearing all history entries.
		/// </summary>
		private void HandleBackSpace ()
		{
			if (InputDisplayText.Length > 0 && InputDisplayText != "0")
			{
				// Remove the last character from the InputDisplayText
				InputDisplayText = InputDisplayText.Substring(0, InputDisplayText.Length - 1);

				// Update the corresponding number value
				if (isFirstNumber)
				{
					// Remove the last digit from the first number
					number1 = number1 / 10;
				}
				else
				{
					// Remove the last digit from the second number
					number2 = number2 / 10;
				}

				// If the InputDisplayText is empty, set it to 0
				if (InputDisplayText.Length == 0)
				{
					InputDisplayText = "0";
				}
			}
		}

		/// <summary>
		/// Clears all history entries.
		/// </summary>
		/// <param name="parameter">The parameter for the command.</param>
		public ICommand ClearAllHistoryCommand => new RelayCommand(ClearAllHistory);

		private void ClearAllHistory (object parameter)
		{
			MessageBoxResult result = MessageBox.Show(
				"Are you sure you want to clear the list?",
				"Clear List",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				HistoryEntries.Clear();

				_dataAccess.ClearAllHistory();

			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
		/// </summary>
		public MainWindowViewModel ()
		{
			// Initialize CalculationHandler for performing calculations
			_calculationHandler = new CalculationHandler();

			// Initialize SQLiteDataAccess for accessing the SQLite database
			_dataAccess = new SQLiteDataAccess();

			// Load calculation history from the database
			LoadHistoryFromDatabase();
		}

		#region Events

		/// <summary>
		/// Event that occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The name of the property that changed.</param>
		protected virtual void OnPropertyChanged ([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Calculation View Functions

		#region Input Diplay Text
		private string _inputDisplayText = "0";

		public string InputDisplayText
		{
			get { return _inputDisplayText; }
			set
			{
				if (_inputDisplayText != value)
				{
					_inputDisplayText = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion

		#region Last History Text
		private string _lastHistoryText = " ";

		public string LastHistoryText
		{
			get { return _lastHistoryText; }
			set
			{
				if (_lastHistoryText != value)
				{
					_lastHistoryText = value;
					OnPropertyChanged();
				}
			}
		}

		private void UpdateLastHistoryText ()
		{
			if (number1 == 0 && calcOperator == ' ')
			{
				LastHistoryText = " ";
			}
			else if (number1 == 0 && calcOperator == ' ')
			{
				LastHistoryText = $"{number1} {calcOperator}";
			}
			else
			{
				if (!isFirstNumber && InputDisplayText != number2.ToString())
				{
					LastHistoryText = $"{FormatNumber(number1)} {calcOperator} {FormatNumber(number2)}";
				}
				else
				{
					LastHistoryText = $"{FormatNumber(number1)} {calcOperator}";
				}

			}
		}

		#endregion

		#endregion

		#region Collection / Database Functions

		/// <summary>
		/// Loads the selected history entry.
		/// </summary>
		/// <param name="selectedHistory">The selected history entry.</param>
		private void LoadHistory (object selectedHistory)
		{
			if (selectedHistory is CalculationHistory history)
			{
				// Set the numbers and operator to the selected history entry
				UpdateCoreData(history.FirstNumber, history.SecondNumber, history.CalculationOperator, history.Result, false);

				// Update display text and last history text
				InputDisplayText = FormatNumber(result);
				UpdateLastHistoryText();

				isLoaded = true;

			}
		}

		private CalculationHistory _selectedHistory;

		/// <summary>
		/// Gets or sets the selected history entry.
		/// </summary>
		public CalculationHistory SelectedHistory
		{
			get { return _selectedHistory; }
			set
			{
				if (_selectedHistory != value)
				{
					_selectedHistory = value;
					LoadHistory(value);
					OnPropertyChanged(nameof(SelectedHistory));
				}
			}
		}

		/// <summary>
		/// Loads history entries from the database.
		/// </summary>
		private void LoadHistoryFromDatabase ()
		{

			var historyFromDb = _dataAccess.GetAllHistoryEntries();

			HistoryEntries.Clear();

			foreach (var historyEntry in historyFromDb)
			{
				HistoryEntries.Add(historyEntry);
			}
		}

		/// <summary>
		/// Closes the connection to the database.
		/// </summary>
		public void CloseConnection ()
		{
			_dataAccess.CloseConnection();
		}

		#endregion

		#region Helper Functions

		/// <summary>
		/// Formats a number as a string with thousands separator.
		/// </summary>
		/// <param name="number">The number to format.</param>
		/// <returns>The formatted number string.</returns>
		private string FormatNumber (double number)
		{
			return number.ToString("N0");
		}

		/// <summary>
		/// Updates the core data used in calculations.
		/// </summary>
		/// <param name="newFirstNumber">The new value for the first number.</param>
		/// <param name="newSecondNumber">The new value for the second number.</param>
		/// <param name="newCalcOperator">The new value for the calculation operator.</param>
		/// <param name="newResult">The new value for the result.</param>
		/// <param name="newIsFirstNumber">The new value indicating whether the current number is the first number.</param>
		
		private void UpdateCoreData(double newFirstNumber, double newSecondNumber, double newResult)
		{
			number1 = newFirstNumber;
			number2 = newSecondNumber;
			result = newResult;
		}

		private void UpdateCoreData (double newFirstNumber, double newSecondNumber, Char newCalcOperator, double newResult, bool newIsFirstNumber)
		{
			number1 = newFirstNumber;
			number2 = newSecondNumber;
			calcOperator = newCalcOperator;
			result = newResult;
			isFirstNumber = newIsFirstNumber;
		}

		#endregion
	}
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Calculator_V3421048.Model;
using Calculator_V3421048.MVVM;
using System.Data.SQLite;
using System.IO;

namespace Calculator_V3421048.ViewModel
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Global Variables
		private double number1 = 0;
		private double number2 = 0;
		private double result = 0;
		private bool isFirstNumber = true;
		private bool isLoaded = false;
		private char calcOperator = ' ';
		#endregion

		private readonly CalculationHandler _calculationHandler;
		public ObservableCollection<CalculationHistory> HistoryEntries { get; set; } = new ObservableCollection<CalculationHistory>();

		private readonly SQLiteDataAccess _dataAccess;

		#region Command Binding
		public ICommand DigitCommand => new RelayCommand(HandleDigit);
		public ICommand OperatorCommand => new RelayCommand(HandleOperator);
		public ICommand EqualsCommand => new RelayCommand(HandleEqual);
		public ICommand ConvertPositiveNegativeCommand => new RelayCommand(HandleConvertPositiveNegativeValue);
		public ICommand ClearCommand => new RelayCommand(execute => ClearAll());
		public ICommand BackspaceCommand => new RelayCommand(execute => HandleBackSpace());

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

		private void LoadHistory (object selectedHistory)
		{
			if (selectedHistory is CalculationHistory history)
			{
				// Set the numbers and operator to the selected history entry
				number1 = history.FirstNumber;
				number2 = history.SecondNumber;
				calcOperator = history.CalculationOperator;
				result = history.Result;
				isFirstNumber = false;

				// Update display text and last history text
				InputDisplayText = FormatNumber(result);
				UpdateLastHistoryText();

				isLoaded = true;

			}
		}

		private CalculationHistory _selectedHistory;

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


		#endregion

		public MainWindowViewModel ()
		{
			_calculationHandler = new CalculationHandler();

			// Initialize SQLite database

			_dataAccess = new SQLiteDataAccess();

			LoadHistoryFromDatabase();
		}


		#region Events

		public event PropertyChangedEventHandler? PropertyChanged;

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

		private void HandleDigit (object sender)
		{
			double currentNumber = (isFirstNumber) ? number1 : number2;

			if (isLoaded)
			{
				isLoaded = false;
				number1 = result;
				number2 = 0;
				result = 0;
				isFirstNumber = false;
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

		private void HandleOperator (object sender)
		{
			if (result != 0)
			{
				number1 = result;
				number2 = 0;
				result = 0;
			}

			calcOperator = Convert.ToChar(sender);
			UpdateLastHistoryText();
			isFirstNumber = false;
			InputDisplayText = "0";
		}

		private string FormatNumber (double number)
		{
			return number.ToString("N0");
		}

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

		private void HandleConvertPositiveNegativeValue (object sender)
		{
			if (isFirstNumber) number1 *= -1; number2 *= -1;
			InputDisplayText = FormatNumber(isFirstNumber ? number1 : number2);
		}

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

		public void CloseConnection ()
		{
			_dataAccess.CloseConnection();
		}

		private void LoadHistoryFromDatabase ()
		{
			// Assuming SQLiteDataAccess has a method to retrieve all history entries
			// Modify the method accordingly if it's not implemented yet
			var historyFromDb = _dataAccess.GetAllHistoryEntries();

			// Clear existing entries if any
			HistoryEntries.Clear();

			// Add fetched history entries to the HistoryEntries collection
			foreach (var historyEntry in historyFromDb)
			{
				HistoryEntries.Add(historyEntry);
			}
		}
	}
}

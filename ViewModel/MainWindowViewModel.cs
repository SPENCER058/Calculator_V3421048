
using Calculator_V3421048.Model;
using Calculator_V3421048.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Calculator_V3421048.ViewModel
{
	internal class MainWindowViewModel
	{
		public ObservableCollection<CalculationHistory> _calculationHistories;

		private readonly CalculationHandler _calculationHandler;

        public MainWindowViewModel()
        {
            _calculationHandler = new CalculationHandler();
			_calculationHistories = new ObservableCollection<CalculationHistory>();
        }

		public ICommand DigitCommand => new RelayCommand(HandleDigit);

		private void HandleDigit (object obj)
		{
			throw new NotImplementedException();
		}

		public ICommand OperatorCommand => new RelayCommand(HandleOperator);

		private void HandleOperator (object obj)
		{
			throw new NotImplementedException();
		}

		public ICommand EqualsCommand => new RelayCommand(HandleEqual);

		private void HandleEqual (object obj)
		{
			throw new NotImplementedException();
		}

		public ICommand ConvertPositiveNegativeCommand => new RelayCommand(HandleConvertPositiveNegativeValue);

		private void HandleConvertPositiveNegativeValue (object obj)
		{
			throw new NotImplementedException();
		}

		public ICommand ClearCommand => new RelayCommand(execute => ClearAll());

		private void ClearAll ()
		{
			throw new NotImplementedException();
		}

		public ICommand BackspaceCommand => new RelayCommand(execute => HandleBackSpace());

		private void HandleBackSpace ()
		{
			throw new NotImplementedException();
		}

		public ICommand DecimalCommand => new RelayCommand(execute => HandleDecimal());

		private void HandleDecimal ()
		{
			throw new NotImplementedException();
		}

		public ICommand ClearAllHistoryCommand => new RelayCommand(ClearAllHistory);

		private void ClearAllHistory (object obj)
		{
			throw new NotImplementedException();
		}
	}
}

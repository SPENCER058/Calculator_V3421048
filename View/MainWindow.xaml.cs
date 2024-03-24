using Calculator_V3421048.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Calculator_V3421048.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel vm;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow ()
		{
			InitializeComponent();
			vm = new MainWindowViewModel();
			DataContext = vm;
		}

		/// <summary>
		/// Handles the event when the left mouse button is pressed down on the window, allowing for window dragging.
		/// </summary>
		public void Window_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		/// <summary>
		/// Handles the event when the minimize button is clicked.
		/// </summary>
		public void BtnMinimize_Click (object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		/// <summary>
		/// Handles the event when the maximize/restore button is clicked.
		/// </summary>
		public void BtnMaximize_Click (object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Normal)
			{
				WindowState = WindowState.Maximized;
			}
			else
			{
				WindowState = WindowState.Normal;
			}
		}

		/// <summary>
		/// Handles the event when the close button is clicked.
		/// </summary>
		public void BtnClose_Click (object sender, RoutedEventArgs e)
		{
			vm.CloseConnection();
			Application.Current.Shutdown();
		}
	}
}

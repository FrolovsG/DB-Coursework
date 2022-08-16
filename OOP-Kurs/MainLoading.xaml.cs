using OOP_Kurs.Models;
using System.Windows;
using System.Windows.Input;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for MainLoading.xaml
	/// </summary>
	public partial class MainLoading : Window
	{
		public MainLoading()
		{
			InitializeComponent();
		}

		private void Validate()
		{
			submitbtn.IsEnabled = false;
			exitbtn.IsEnabled = false;
			PasswordTextBox.IsEnabled = false;

			TourDB.Init(PasswordTextBox.Password);
			if (TourDB.Check())
			{
				DialogResult = true;
				Close();
			}
			else
			{
				MessageBox.Show("Failed to connect to the database: incorrect password", "Invalid Password",
									MessageBoxButton.OK, MessageBoxImage.Exclamation);
				submitbtn.IsEnabled = true;
				exitbtn.IsEnabled = true;
				PasswordTextBox.IsEnabled = true;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Validate();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult = DialogResult.GetValueOrDefault(false);
		}

		private void PasswordTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				Validate();
			}
		}
	}
}

using OOP_Kurs.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for AddGuideWindow.xaml
	/// </summary>
	public partial class AddGuideWindow : Window
	{
		//regex patterns to check correctness of forms
		private readonly string ALLOWED_SYMBOLS = "^[a-zA-Z\\-]+$";
		private readonly string IDCODE_PATTERN = "^\\d{6}-\\d{5}$";

		//to color textbox' background color if format is invalid
		Brush paleRed;

		public AddGuideWindow()
		{
			InitializeComponent();

			IDTextBox.Text = TourDB.NextGuideId().ToString();

			BirthDateDatePicker.SelectedDate = DateTime.Now.Date - TimeSpan.FromDays(365 * 18);
			EmploymentDateDatePicker.SelectedDate = DateTime.Now.Date;

			paleRed = new BrushConverter().ConvertFrom("#FFFFCCCC") as Brush;

			checkBox.IsChecked = true;
		}

		public void Validate()
		{
			SubmitButton.IsEnabled = false;
			bool valid = true;

			Func<TextBox, string, bool> RegexCheck = (TextBox textBox, string pattern) =>
			{
				textBox.Background = Brushes.White;
				if (!Regex.Match(textBox.Text, pattern).Success)
				{
					textBox.Background = paleRed;
					return false;
				}

				return true;
			};

			if(!RegexCheck(NameTextBox, ALLOWED_SYMBOLS) || NameTextBox.Text == string.Empty)
				valid = false;
			
			if (!RegexCheck(SurnameTextBox, ALLOWED_SYMBOLS) || SurnameTextBox.Text == string.Empty)
				valid = false;
			
			if (!RegexCheck(IDCodeTextBox, IDCODE_PATTERN))
				valid = false;

			if (BirthDateDatePicker.SelectedDate > EmploymentDateDatePicker.SelectedDate)
				valid = false;

			if (valid)
				SubmitButton.IsEnabled = true;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			EmploymentDateDatePicker.SelectedDate = DateTime.Now.Date;
			EmploymentDateDatePicker.IsEnabled = false;
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			EmploymentDateDatePicker.IsEnabled = true;
		}

		private void NameTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();
		private void SurnameTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();
		private void IDCodeTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();
		private void BirthDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => Validate();
		private void EmploymentDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => Validate();


		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				short id = short.Parse(IDTextBox.Text);

				TourDB.AddGuide
				(
					new Guide(id, NameTextBox.Text, SurnameTextBox.Text, IDCodeTextBox.Text, BirthDateDatePicker.SelectedDate.Value, EmploymentDateDatePicker.SelectedDate)
				);
			}
			catch (ArgumentException ee)
			{
				Console.WriteLine(ee.Message);
			}

			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}

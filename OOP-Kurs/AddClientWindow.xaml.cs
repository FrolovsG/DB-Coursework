using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for AddClientWindow.xaml
	/// </summary>
	public partial class AddClientWindow : Window
	{
		//regex patterns to check correctness of forms
		private readonly string ALLOWED_SYMBOLS = "^[a-zA-Z\\-]+$";
		private readonly string IDCODE_PATTERN = "^\\d{6}-\\d{5}$";

		//to color textbox' background color if format is invalid
		Brush paleRed;

		ClientStatus selectedStatus;
		List<ClientStatus> statusList;


		public AddClientWindow()
		{
			paleRed = new BrushConverter().ConvertFrom("#FFFFCCCC") as Brush;
			selectedStatus = ClientStatus.Common;
			statusList = new List<ClientStatus>
			{
				ClientStatus.Common,
				ClientStatus.Pensioner,
				ClientStatus.VIP
			};
			

			InitializeComponent();

			IDTextBox.Text = TourDB.NextClientId().ToString();

			StatusComboBox.ItemsSource = statusList;
			StatusComboBox.SelectedItem = selectedStatus;
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

			if (!RegexCheck(NameTextBox, ALLOWED_SYMBOLS) || NameTextBox.Text == string.Empty)
				valid = false;

			if (!RegexCheck(SurnameTextBox, ALLOWED_SYMBOLS) || SurnameTextBox.Text == string.Empty)
				valid = false;

			if (!RegexCheck(IDCodeTextBox, IDCODE_PATTERN))
				valid = false;

			if (valid)
				SubmitButton.IsEnabled = true;
		}
		private void NameTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();
		private void SurnameTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();
		private void IDCodeTextBox_LostFocus(object sender, RoutedEventArgs e) => Validate();

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			short id = short.Parse(IDTextBox.Text);

			TourDB.AddClient(new Client(id, NameTextBox.Text, SurnameTextBox.Text, IDCodeTextBox.Text, selectedStatus));
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedStatus = (ClientStatus)StatusComboBox.SelectedItem;
		}
	}
}

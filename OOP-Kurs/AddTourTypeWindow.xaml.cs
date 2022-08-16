using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for AddTourTypeWindow.xaml
	/// </summary>
	public partial class AddTourTypeWindow : Window, INotifyPropertyChanged
	{
		//regex pattern to check correctness of forms
		private readonly string ALLOWED_SYMBOLS = "^[a-zA-Z0-9\\-]+$";

		Brush paleRed;

		private double height;

		public double CustomHeight
		{
			get => height;
			set
			{
				if (value != height)
				{
					height = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CustomHeight"));
				}
			}
		}

		public AddTourTypeWindow()
		{
			InitializeComponent();
			DataContext = this;
			CustomHeight = 380;

			paleRed = new BrushConverter().ConvertFrom("#FFFFCCCC") as Brush;

			IDTextBox.Text = TourDB.NextTourTypeId().ToString();

			SiteListBox.ItemsSource = TourDB.GetSiteList();
			SubmitButton.IsEnabled = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void ValidateSite()
		{
			bool valid = true;

			QualityTextBox.Background = Brushes.White;
			if (!short.TryParse(QualityTextBox.Text, out _))
			{
				QualityTextBox.Background = paleRed;
				SubmitSiteButton.IsEnabled = false;
			}

			SiteNameTextBox.Background = Brushes.White;
			if (SiteListBox.ItemsSource.Cast<Site>().ToList().Exists(site => site.SiteName == SiteNameTextBox.Text))
			{
				QualityTextBox.Background = paleRed;
				SubmitSiteButton.IsEnabled = false;
			}

			SubmitSiteButton.IsEnabled = valid;
		}

		public void ValidateTourType()
		{
			SubmitButton.IsEnabled = false;
			bool valid = true;

			NameTextBox.Background = Brushes.White;
			if (!Regex.Match(NameTextBox.Text, ALLOWED_SYMBOLS).Success || NameTextBox.Text == string.Empty)
			{
				NameTextBox.Background = paleRed;
				valid = false;
			}

			PriceTextBox.Background = Brushes.White;
			if (!double.TryParse(PriceTextBox.Text, out _))
			{
				valid = false;
				PriceTextBox.Background = paleRed;
			}

			MaxParticipantsTextBox.Background = Brushes.White;
			if (!long.TryParse(MaxParticipantsTextBox.Text, out _))
			{
				valid = false;
				MaxParticipantsTextBox.Background = paleRed;
			}

			DurationTextBox.Background = Brushes.White;
			if (!long.TryParse(DurationTextBox.Text, out _))
			{
				valid = false;
				DurationTextBox.Background = paleRed;
			}

			if (SiteListBox.SelectedItems.Count == 0)
				valid = false;

			if (valid)
				SubmitButton.IsEnabled = true;
		}

		private void NewSiteButton_Click(object sender, RoutedEventArgs e)
		{
			CustomHeight += SiteGrid.Height;
			SiteGrid.Visibility = Visibility.Visible;
			NewSiteButton.IsEnabled = false;

			SiteNameTextBox.Text = string.Empty;
			AddressTextBox.Text = string.Empty;
			QualityTextBox.Text = string.Empty;
			SubmitSiteButton.IsEnabled = false;

			SearchTextBox.Text = string.Empty;
			SearchTextBox.IsEnabled = false;
		}

		private void SubmitSiteButton_Click(object sender, RoutedEventArgs e)
		{
			TourDB.AddSite(new Site(SiteNameTextBox.Text, AddressTextBox.Text, short.Parse(QualityTextBox.Text)));
			SiteListBox.ItemsSource = TourDB.GetSiteList();
			SiteListBox.Items.Refresh();

			CustomHeight -= SiteGrid.Height;
			SiteGrid.Visibility = Visibility.Hidden;
			NewSiteButton.IsEnabled = true;
			SubmitSiteButton.IsEnabled = false;
			SearchTextBox.IsEnabled = true;
		}
		private void HideButton_Click(object sender, RoutedEventArgs e)
		{
			CustomHeight -= SiteGrid.Height;
			SiteGrid.Visibility = Visibility.Hidden;
			NewSiteButton.IsEnabled = true;
			SearchTextBox.IsEnabled = true;
		}

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			long price = (long)(double.Parse(PriceTextBox.Text) * 100);

			var siteList = new List<Site>();
			siteList.AddRange(SiteListBox.SelectedItems.Cast<Site>());

			TourDB.AddTourType(new TourType(short.Parse(IDTextBox.Text), NameTextBox.Text, price,
				short.Parse(MaxParticipantsTextBox.Text), new TimeSpan(int.Parse(DurationTextBox.Text), 0, 0),
				siteList
			));

			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void QualityTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateSite();
		private void SiteNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateSite();

		private void SiteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ValidateTourType();
		private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateTourType();
		private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateTourType();
		private void MaxParticipantsTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateTourType();
		private void DurationTextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidateTourType();

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string pattern = SearchTextBox.Text;
			IEnumerable<Site> matches = TourDB.GetSiteList();

			if (!string.IsNullOrWhiteSpace(pattern))
				matches = matches.Where(site => site.ToString().Contains(pattern));

			SiteListBox.ItemsSource = matches;
		}

	}
}

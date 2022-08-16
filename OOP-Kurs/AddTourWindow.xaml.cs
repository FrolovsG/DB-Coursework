using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for AddTourWindow.xaml
	/// </summary>
	public partial class AddTourWindow : Window
	{
        private static List<TourType> switchFunc(List<TourType> matches, Func<Predicate<TourType>, List<TourType>> Find, string tag, string pattern)
        {
            //some chars that need parsing
            char[] specialChars = new char[] { '+', '-', '^', '$' };

            switch (tag)
            {
                case "id":
                    short id;
                    if (short.TryParse(pattern.Trim(specialChars), out id))
                        matches = Find(ttype =>
                        {
                            if (pattern.Contains('+'))
                                return ttype.Id >= id;
                            else if (pattern.Contains('-'))
                                return ttype.Id <= id;
                            else
                                return ttype.Id == id;
                        });
                    break;

                case "name":
                    matches = Find(ttype => Regex.Match(ttype.Name, pattern).Success);
                    break;

                case "duration":
                    double duration;
                    if (double.TryParse(pattern.Trim(specialChars), out duration))
                    {
                        matches = Find(ttype =>
                        {
                            if (pattern.Contains('+'))
                                return ttype.DurationHours >= duration;
                            else if (pattern.Contains('-'))
                                return ttype.DurationHours <= duration;
                            else
                                return ttype.DurationHours == duration;
                        });
                    }
                    break;


                case "price":
                    long price;
                    if (double.TryParse(pattern.Trim(specialChars), out _))
                    {
                        price = (long)(double.Parse(pattern.Trim(specialChars)) * 100);
                        matches = Find(ttype =>
                        {
                            if (pattern.Contains('+'))
                                return ttype.Price >= price;
                            else if (pattern.Contains('-'))
                                return ttype.Price <= price;
                            else
                                return ttype.Price == price;
                        });
                    }
                    break;

                case "maxparticipants":
                    short max;
                    if (short.TryParse(pattern.Trim(specialChars), out max))
                        matches = Find(ttype =>
                        {
                            if (pattern.Contains('+'))
                                return ttype.MaxParticipants >= max;
                            else if (pattern.Contains('-'))
                                return ttype.MaxParticipants <= max;
                            else
                                return ttype.MaxParticipants == max;
                        });
                    break;

                case "site":
                    matches = Find(ttype =>
                    {
                        bool found = false;
                        foreach (Site site in ttype.SiteList)
                        {
                            if (Regex.Match(site.ToString(), pattern).Success)
                            {
                                found = true;
                                break;
                            }
                        }
                        return found;
                    });
                    break;

                default:
                    matches = Find(ttype => Regex.Match(ttype.ToString(), pattern).Success);
                    break;
            }

            return matches;
        }

        public AddTourWindow()
		{
			InitializeComponent();
			DataContext = this;

			TourTypeGridView.ItemsSource = TourDB.GetTourTypeList();

            IDTextBox.Text = TourDB.NextTourId().ToString();

            StartDatePicker.SelectedDate = DateTime.Now.Date.AddDays(1);
			SubmitButton.IsEnabled = false;
		}

		private void Validate()
		{
			if (StartDatePicker.SelectedDate.Value <= DateTime.Now.Date && (TourType)TourTypeGridView.SelectedItem != null && !TimeSpan.TryParse(StartTimeTextBox.Text, out _))
				SubmitButton.IsEnabled = false;
			else
				SubmitButton.IsEnabled = true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			new AddTourTypeWindow().ShowDialog();
            TourTypeGridView.ItemsSource = TourDB.GetTourTypeList();
            TourTypeGridView.Items.Refresh();
		}

        //ignore deletion, all ttypes should be archived
		private void TourTypeGridView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
				e.Handled = true;
		}

		private void TourTypeGridView_SelectionChanged(object sender, SelectionChangedEventArgs e) => Validate();
		private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => Validate();
        private void StartTimeTextBox_TextChanged(object sender, TextChangedEventArgs e) => Validate();

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
            var datetime = new DateTime(StartDatePicker.SelectedDate.Value.Ticks) + TimeSpan.Parse(StartTimeTextBox.Text);

            TourDB.AddTour(new Tour(
				short.Parse(IDTextBox.Text), (TourType)TourTypeGridView.SelectedItem,
				new List<Client>(), new List<Guide>(), datetime
			));

			Close();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			App.TagSearch(TourDB.GetTourTypeList(), ref TourTypeGridView, SearchTextBox.Text, switchFunc);
		}
    }
}

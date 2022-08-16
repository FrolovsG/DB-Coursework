using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for GuideViewWindow.xaml
	/// </summary>
	public partial class GuideViewWindow : Window
	{
		private static List<Guide> SwitchFunc(List<Guide> matches, Func<Predicate<Guide>, List<Guide>> Find, string tag, string pattern)
		{
			//some chars that need parsing
			char[] specialChars = new char[] { '+', '-', '^', '$' };

			switch (tag)
			{
				case "id":
					short id;
					if (short.TryParse(pattern.Trim(specialChars), out id))
						matches = Find(guide =>
						{
							if (pattern.Contains('+'))
								return guide.Id >= id;
							else if (pattern.Contains('-'))
								return guide.Id <= id;
							else
								return guide.Id == id;
						});
					break;

				case "name":
					matches = Find(guide => Regex.Match(guide.Name, pattern).Success);
					break;

				case "surname":
					matches = Find(guide => Regex.Match(guide.Surname, pattern).Success);
					break;

				case "idcode":
					matches = Find(guide => Regex.Match(guide.IDCode, pattern).Success);
					break;

				case "available":
					matches = Find(guide => Regex.Match(guide.IsAvailable.ToString(), pattern).Success);
					break;

				case "birthdate":
					matches = Find(tour => Regex.Match(tour.BirthDate.ToString(), pattern).Success);
					break;

				case "employmentdate":
					matches = Find(tour => Regex.Match(tour.EmploymentDate.ToString(), pattern).Success);
					break;

				default:
					matches = Find(guide => Regex.Match(guide.ToString(), pattern).Success);
					break;
			}

			return matches;
		}

		public GuideViewWindow()
		{
			InitializeComponent();
			DataContext = this;
			GuideGridView.ItemsSource = TourDB.GetGuideList();

			AssignToTourButton.IsEnabled = false;
		}

		private void AssignToTourButton_Click(object sender, RoutedEventArgs e)
		{
			new AssignToTourWindow(GuideGridView.SelectedItem).ShowDialog();
			GuideGridView.Items.Refresh();
		}

		private void AddGuideButton_Click(object sender, RoutedEventArgs e)
		{
			new AddGuideWindow().ShowDialog();
			GuideGridView.ItemsSource = TourDB.GetGuideList();
			GuideGridView.Items.Refresh();
		}

		private void ReturnButton_Clients_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void GuideGridView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Guide guide = (Guide)GuideGridView.SelectedItem;

			if (e.Key == Key.Delete)
			{
				bool isAssigned = false;
				StringBuilder tourListString = new StringBuilder("Assigned to planned tours:\n");
				foreach (Tour tour in TourDB.GetTourList(guide))
				{
					if (tour.GuideList.Contains(guide) && tour.Status == TourStatus.Planned.ToString())
					{
						isAssigned = true;
						tourListString.AppendLine($"{tour}");
					}
				}


				MessageBoxResult result = MessageBox.Show($"Warning: this action will permanently delete the selected guide ({guide})\n" +
														  $"and all of its information from the system.\n"+
														  (isAssigned ? tourListString.ToString() : "Guide is not assigned to any planned tours") + "\n" +
														  "Are you still willing to commit?",
														  "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

				if (result == MessageBoxResult.Yes)
				{
					TourDB.RemoveGuide(guide.Id);
					AssignToTourButton.IsEnabled = false;

					GuideGridView.ItemsSource = TourDB.GetGuideList();
					GuideGridView.Items.Refresh();
				}

				e.Handled = true;
			}
		}

		//if guide in selected row is available, enable AssignToTourButton
		private void GuideGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Guide guide = (Guide)GuideGridView.SelectedItem;

			if(guide != null)
				AssignToTourButton.IsEnabled = guide.IsAvailable ? true : false;
		}

		private void SearchButton_Guides_Click(object sender, RoutedEventArgs e)
		{
			App.TagSearch(TourDB.GetGuideList(), ref GuideGridView, SearchTextBox.Text, SwitchFunc);
		}
	}
}

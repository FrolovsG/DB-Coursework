using OOP_Kurs.Models;
using System.Linq;
using System.Windows;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for AssignToTourWindow.xaml
	/// </summary>
	public partial class AssignToTourWindow : Window
	{
		private Guide guide;

		public AssignToTourWindow(object obj)
		{
			InitializeComponent();

			guide = (Guide)obj;
			if (guide == null)
			{
				MessageBoxResult result = System.Windows.MessageBox.Show($"Error: attempted to assign unselected guide. \n" +
											  $"Make sure you've selected a guide from the list and try again.",
											  "", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}

			UpdateLists();
		}
		private void UpdateLists()
		{
			TourListBox_Assigned.ItemsSource = TourDB.GetTourList(guide);
			TourListBox_Unassigned.ItemsSource = TourDB.GetTourListWithoutGuide().Where((Tour tour) => tour.Status != TourStatus.Completed.ToString());
			TourListBox_Other.ItemsSource = TourDB.GetTourList()
												.Except(TourListBox_Unassigned.ItemsSource.Cast<Tour>())
												.Except(TourListBox_Assigned.ItemsSource.Cast<Tour>());


			TourListBox_Assigned.Items.Refresh();
			TourListBox_Unassigned.Items.Refresh();
			TourListBox_Other.Items.Refresh();
		}


		private void UnassignButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (Tour tour in TourListBox_Assigned.SelectedItems.Cast<Tour>())
			{
				TourDB.UnassignGuide(tour.Id, guide.Id);
			}
			
			UpdateLists();
		}

		private void AssignButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (Tour tour in TourListBox_Unassigned.SelectedItems.Cast<Tour>().Union(TourListBox_Other.SelectedItems.Cast<Tour>()))
			{
				TourDB.AssignGuide(tour.Id, guide.Id);
			}
				
			UpdateLists();
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}

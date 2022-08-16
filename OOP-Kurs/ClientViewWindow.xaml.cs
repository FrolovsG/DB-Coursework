using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace OOP_Kurs
{
	/// <summary>
	/// Interaction logic for ClientViewWindow.xaml
	/// </summary>
	public partial class ClientViewWindow : Window
	{
        private static List<Client> SwitchFunc(List<Client> matches, Func<Predicate<Client>, List<Client>> Find, string tag, string pattern)
        {
            //some chars that need parsing
            char[] specialChars = new char[] { '+', '-', '^', '$' };

            switch (tag)
            {
                case "id":
                    short id;
                    if (short.TryParse(pattern.Trim(specialChars), out id))
                        matches = Find(client =>
                        {
                            if (pattern.Contains('+'))
                                return client.Id >= id;
                            else if (pattern.Contains('-'))
                                return client.Id <= id;
                            else
                                return client.Id == id;
                        });
                    break;

                case "name":
                    matches = Find(client => Regex.Match(client.Name, pattern).Success);
                    break;

                case "surname":
                    matches = Find(client => Regex.Match(client.Surname, pattern).Success);
                    break;

                case "idcode":
                    matches = Find(client => Regex.Match(client.IDCode, pattern).Success);
                    break;

                case "status":
                    matches = Find(client => Regex.Match(client.Status.ToString(), pattern).Success);
                    break;

                default:
                    matches = Find(client => Regex.Match(client.ToString(), pattern).Success);
                    break;
            }

            return matches;
        }

        public ClientViewWindow()
		{
			InitializeComponent();
			DataContext = this;
			ClientGridView.ItemsSource = TourDB.GetClientList();
		}

		private void ClientGridView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Client client = ClientGridView.SelectedItem as Client;

			if (e.Key == Key.Delete)
			{
				MessageBoxResult result = MessageBox.Show($"Warning: this action will permanently delete the selected client\n" +
														  $"({client}) and all of its information from the system.\n" +
														  "Are you still willing to commit?",
														  "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

				if (result == MessageBoxResult.Yes)
				{
					//replace clients in completed tours in order to keep the revenue
					//Client replacementClient = new Client(-1, "REMOVED", "CLIENT", "000000-00000", ClientStatus.Common);

					TourDB.RemoveClient(client.Id);

					ClientGridView.ItemsSource = TourDB.GetClientList();
					ClientGridView.Items.Refresh();
				}

				e.Handled = true;
			}
		}

		private void AddClientButton_Click(object sender, RoutedEventArgs e)
		{
			new AddClientWindow().ShowDialog();
			ClientGridView.ItemsSource = TourDB.GetClientList();
			ClientGridView.Items.Refresh();
		}

		private void ReturnButton_Clients_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SearchButton_Clients_Click(object sender, RoutedEventArgs e)
		{
			App.TagSearch(TourDB.GetClientList(), ref ClientGridView, SearchTextBox.Text, SwitchFunc);
		}
	}
}

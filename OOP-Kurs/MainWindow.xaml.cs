using OOP_Kurs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OOP_Kurs
{
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        private double width;
        public long revenue;

        public double CustomWidth
        {
            get => width;
            set
            {
                if (value != width)
                {
                    width = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CustomWidth"));
                }
            }
        }

        public string Revenue
        {
            get => $"{revenue / 100}.{revenue % 100}";
            set
            {
                long rev = revenue;
                long.TryParse(value, out rev);

                if (rev != revenue)
                {
                    revenue = rev;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Revenue"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Tour lastSelectedGridRow;

        public void CheckGuides()
        {
            WarningTextBlock.Visibility = Visibility.Hidden;

            //check if tours have at least one guide assigned to it and if it's planned
            foreach (Tour tour in TourGridView_Main.Items)
            {
                if (tour.GuideList.Count == 0 && tour.Status == TourStatus.Planned.ToString())
                {
                    WarningTextBlock.Visibility = Visibility.Visible;
                    break;
                }
            }
        }

        public void UpdateRevenue()
        {
            Revenue = TourDB.GetRevenue(Date_From.SelectedDate.Value, Date_To.SelectedDate.Value.AddDays(1).AddMinutes(-1)).ToString();
        }

        private void UpdateAddClientGrid(object sender = null)
        {
            lastSelectedGridRow = (Tour)TourGridView_Main.SelectedItem;

            if (lastSelectedGridRow != null && AddClientToTourGrid.Visibility == Visibility.Visible && lastSelectedGridRow.Status != TourStatus.Completed.ToString())
            {
                ParticipantsListBox.ItemsSource = lastSelectedGridRow.ClientList;
                ParticipantsListBox.Items.Refresh();
                ParticipantsLabel.Content = $"_Participants (Free slots: {lastSelectedGridRow.TourType.MaxParticipants - lastSelectedGridRow.ClientList.Count}):";

                ClientsListBox.ItemsSource = TourDB.GetClientList().Except(lastSelectedGridRow.ClientList);
                ClientsListBox.Items.Refresh();

                RemoveFromTourButton.IsEnabled = true;
                AddToTourButton.IsEnabled = true;
            }
            else
            {
                ParticipantsLabel.Content = "_Participants:";
                RemoveFromTourButton.IsEnabled = false;
                AddToTourButton.IsEnabled = false;
            }

            if (TourGridView_Main.Equals(sender))
                SearchClientTextBox.Text = "";
        }

        internal static List<Tour> SwitchFunc(List<Tour> matches, Func<Predicate<Tour>, List<Tour>> Find, string tag, string pattern)
        {
            //some chars that need parsing
            char[] specialChars = new char[] { '+', '-', '^', '$' };

            switch (tag)
            {
                case "id":
                    short id;
                    if (short.TryParse(pattern.Trim(specialChars), out id))
                        matches = Find(tour =>
                        {
                            if (pattern.Contains('+'))
                                return tour.Id >= id;
                            else if (pattern.Contains('-'))
                                return tour.Id <= id;
                            else
                                return tour.Id == id;
                        });
                    break;

                case "type":
                    matches = Find(tour => Regex.Match(tour.TourTypeName, pattern).Success);
                    break;

                case "startdate":
                    matches = Find(tour => Regex.Match(tour.StartDate.ToString(), pattern).Success);
                    break;

                case "status":
                    matches = Find(tour => Regex.Match(tour.Status, pattern).Success);
                    break;

                case "price":
                    long price;
                    if (double.TryParse(pattern.Trim(specialChars), out _))
                    {
                        price = (long)(double.Parse(pattern.Trim(specialChars)) * 100);
                        matches = Find(tour =>
                        {
                            if (pattern.Contains('+'))
                                return tour.TourType.Price >= price;
                            else if (pattern.Contains('-'))
                                return tour.TourType.Price <= price;
                            else
                                return tour.TourType.Price == price;
                        });
                    }
                    break;

                case "maxparticipants":
                    short max;
                    if (short.TryParse(pattern.Trim(specialChars), out max))
                        matches = Find(tour =>
                        {
                            if (pattern.Contains('+'))
                                return tour.TourType.MaxParticipants >= max;
                            else if (pattern.Contains('-'))
                                return tour.TourType.MaxParticipants <= max;
                            else
                                return tour.TourType.MaxParticipants == max;
                        });
                    break;

                case "duration":
                    double duration;
                    if (double.TryParse(pattern.Trim(specialChars), out duration))
                        matches = Find(tour =>
                        {
                            if (pattern.Contains('+'))
                                return tour.TourType.DurationHours >= duration;
                            else if (pattern.Contains('-'))
                                return tour.TourType.DurationHours <= duration;
                            else
                                return tour.TourType.DurationHours == duration;
                        });
                    break;

                case "client":
                    matches = Find(tour =>
                    {
                        bool found = false;
                        foreach (Client client in tour.ClientList)
                            if (Regex.Match(client.ToString(), pattern).Success)
                            {
                                found = true;
                                break;
                            }
                        return found;
                    });
                    break;

                case "guide":
                    matches = Find(tour =>
                    {
                        bool found = false;
                        foreach (Guide guide in tour.GuideList)
                            if (Regex.Match(guide.ToString(), pattern).Success)
                            {
                                found = true;
                                break;
                            }
                        return found;
                    });
                    break;

                case "site":
                    matches = Find(tour =>
                    {
                        bool found = false;
                        foreach (Site site in tour.TourType.SiteList)
                            if (Regex.Match(site.ToString(), pattern).Success)
                            {
                                found = true;
                                break;
                            }
                        return found;
                    });
                    break;

                default:
                    matches = Find(tour => Regex.Match(tour.ToString(), pattern).Success);
                    break;
            }

            return matches;
        }

        public MainWindow()
        {
            var loading = new MainLoading();

            if (!loading.ShowDialog().Value)
                Close();
            else
            {
                InitializeComponent();
                DataContext = this;
                CustomWidth = 700;
                TourGridView_Main.ItemsSource = TourDB.GetTourList();

                Date_From.SelectedDate = DateTime.Now.Date.AddMonths(-1);
                Date_To.SelectedDate = DateTime.Now.Date;

                UpdateRevenue();
                CheckGuides();
            }
        }


        private void NewTourButton_Click(object sender, RoutedEventArgs e)
        {
            new AddTourWindow().ShowDialog();
            TourGridView_Main.ItemsSource = TourDB.GetTourList();
            TourGridView_Main.Items.Refresh();
            CheckGuides();
        }

        private void ViewClientsButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientViewWindow().ShowDialog();
            TourGridView_Main.ItemsSource = TourDB.GetTourList();
            TourGridView_Main.Items.Refresh();
            UpdateRevenue();

            if (AddClientToTourGrid.Visibility == Visibility.Visible)
                UpdateAddClientGrid();
        }

        private void ViewGuidesButton_Click(object sender, RoutedEventArgs e)
        {
            new GuideViewWindow().ShowDialog();
            TourGridView_Main.ItemsSource = TourDB.GetTourList();
            TourGridView_Main.Items.Refresh();
            CheckGuides();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            CustomWidth += AddClientToTourGrid.Width;
            AddClientToTourGrid.Visibility = Visibility.Visible;
            AddClientButton.IsEnabled = false;
            SearchClientTextBox.Text = "";

            UpdateAddClientGrid(sender);
        }

        //Hides the AddClientGrid and clears some data
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CustomWidth -= AddClientToTourGrid.Width;
            AddClientToTourGrid.Visibility = Visibility.Hidden;
            AddClientButton.IsEnabled = true;

            ParticipantsListBox.ItemsSource = null;
            ClientsListBox.ItemsSource = null;
        }

        private void Date_From_CalendarClosed(object sender, RoutedEventArgs e)
        {
            UpdateRevenue();
        }

        private void Date_To_CalendarClosed(object sender, RoutedEventArgs e)
        {
            UpdateRevenue();
        }

        private void TourGridView_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAddClientGrid(sender);
        }

        private void RemoveFromTourButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTour = (Tour)TourGridView_Main.SelectedItem;

            IEnumerable<Client> clientList = ParticipantsListBox.SelectedItems.Cast<Client>().AsEnumerable();
            selectedTour.ClientList.RemoveAll(client => clientList.Contains(client));
            foreach (var c in clientList)
            {
                TourDB.RemoveClientFromTour(selectedTour.Id, c.Id);
            }

            TourGridView_Main.Items.Refresh();

            UpdateAddClientGrid(sender);
            UpdateRevenue();
        }

        private void AddToTourButton_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Client> clientList = ClientsListBox.SelectedItems.Cast<Client>().AsEnumerable();
            Tour tour = (Tour)TourGridView_Main.SelectedItem;

            int allowedNumberOfClients = tour.TourType.MaxParticipants - tour.ClientList.Count;

            if (allowedNumberOfClients >= clientList.Count())
            {
                tour.ClientList.AddRange(clientList);

                foreach (var c in clientList)
                {
                    TourDB.AddClientToTour(tour.Id, c.Id);
                }

                TourGridView_Main.Items.Refresh();

                UpdateAddClientGrid(sender);
                UpdateRevenue();
            }
            else
                System.Windows.MessageBox.Show($"Error: attempted to add number of clients\n" +
                                               $"that exceeds max participants allowed:\n" +
                                               $"Tried: {clientList.Count()}, Allowed: {allowedNumberOfClients}.",
                                               "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        //adds tooltip to each row in the tour grid
        // --> should find a way to add toolbar to cells in TourType column <--
        private void TourGridView_Main_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row != null)
            {
                Tour tour = (Tour)e.Row.Item;

                StackPanel panel = new StackPanel();

                TextBlock block = new TextBlock
                {
                    Text = $"Tour Type ID: {tour.TourType.Id}\n" +
                           $"Price per client: {tour.TourType.PriceString}\n" +
                           $"Max Participants: {tour.TourType.MaxParticipants}\n" +
                           $"Duration (dd:hh:mm:ss): {tour.TourType.Duration}"
                };

                panel.Children.Add(block);

                e.Row.ToolTip = panel;
            }
        }

        private void SearchClientButton_Click(object sender, RoutedEventArgs e)
        {
            string pattern = SearchClientTextBox.Text;
            Tour tour = (Tour)TourGridView_Main.SelectedItem;
            IEnumerable<Client> matches = TourDB.GetClientList();

            if (tour != null)
            {
                if (!string.IsNullOrWhiteSpace(pattern))
                    matches = matches.Where(client => client.ToString().Contains(pattern));
            }
            ClientsListBox.ItemsSource = matches.Except(tour.ClientList);
            ParticipantsListBox.ItemsSource = tour.ClientList.Intersect(matches);
        }

        private void TourGridView_Main_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Tour tour = (Tour)TourGridView_Main.SelectedItem;

            //for now only tour deletion is handled
            if (e.Key == Key.Delete)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show($"Warning: this action will permanently delete the selected tour ({tour})\n" +
                                               $"and all of its information from the system. Are you still willing to commit?",
                                               "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.Yes)
                {
                    TourDB.RemoveTour(tour.Id);

                    TourGridView_Main.ItemsSource = TourDB.GetTourList();
                    TourGridView_Main.Items.Refresh();
                    UpdateRevenue();
                    CheckGuides();
                }

                e.Handled = true;
            }
        }

        private void SearchButton_Main_Click(object sender, RoutedEventArgs e)
        { 
           App.TagSearch(TourDB.GetTourList(), ref TourGridView_Main, SearchTextBox_Main.Text, SwitchFunc);
        }

        private void Window_Main_Closing(object sender, CancelEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Are you sure you want to exit the program?",
                                               "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            MessageBox.Show(TourDB.ReportRevenue(Date_From.SelectedDate.Value, Date_To.SelectedDate.Value.AddDays(1).AddMinutes(-1)));
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}


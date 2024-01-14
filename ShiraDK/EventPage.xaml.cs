using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiraRDKWork
{
    /// <summary>
    /// Логика взаимодействия для EventPage.xaml
    /// </summary>
    public partial class EventPage : Page
    {
        int role;
        public EventPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            DataGridUpdate();
            orginazerCbox.ItemsSource = DBEntities. GetContext().Organizers.ToList();
            orginazerCbox.DisplayMemberPath = "Name";

            if (role == 2)
            {
                CRUDpanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DataGridUpdate()
        {
            List<Event> events = DBEntities.GetContext().Events.ToList();

            if (orginazerCbox.SelectedValue != null)
                events = events.Where(eve => eve.Organizer == (Organizer)orginazerCbox.SelectedItem).ToList();
            if(startDatePicer.SelectedDate != null)
                events = events.Where(eve => eve.DateStart >= startDatePicer.SelectedDate).ToList();
            if(endDatePicer.SelectedDate != null)
                events = events.Where(eve => eve.DateStart <= endDatePicer.SelectedDate).ToList();
            if(eventNameBox.Text.Length > 0)
                events =events.Where(eve => eve.Name.ToLower().IndexOf(eventNameBox.Text.ToLower()) != -1).ToList();

            dataGrid.ItemsSource = events.ToList();


        }

        private void cleanBtn_Click(object sender, RoutedEventArgs e)
        {
             

            orginazerCbox.SelectedValue = null;
            startDatePicer.SelectedDate = null;
            endDatePicer.SelectedDate = null;
            eventNameBox.Text = null;

            DataGridUpdate();
        }




        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Event> ClientForRemoving = dataGrid.SelectedItems.Cast<Event>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {ClientForRemoving.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().Events.RemoveRange(ClientForRemoving);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                    DataGridUpdate();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditEventPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedValue == null)
                    return;
            NavigationService.Navigate(new AddEditEventPage((Event)dataGrid.SelectedValue));
        }

        private void editItemForEventBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            NavigationService.Navigate(new ItemForEventPage((Event)dataGrid.SelectedValue));
        }

        private void eventNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridUpdate();
        }

        private void orginazerCbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridUpdate();

        }
    }
}

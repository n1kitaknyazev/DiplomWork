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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        int role;
        public UserPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridUpdate();
        }

        private void DataGridUpdate()
        {
            List<User> events = DBEntities.GetContext().Users.ToList();

            if (firstNameBox.Text.Length != 0)
            {
                string txt = firstNameBox.Text.ToLower();

                events = events.Where(x => x.FirstName.ToLower().Contains(txt) ||
                        x.LastName.ToLower().Contains(txt) ||
                        x.Login.ToLower().Contains(txt) ||
                        x.Password.ToLower().Contains(txt)).ToList();
            }
            dataGrid.ItemsSource = events.ToList();
        }

        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectRows = dataGrid.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {selectRows.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().Users.RemoveRange(selectRows);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");                 
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }

            DataGridUpdate();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditUserPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null)
                return;
            NavigationService.Navigate(new AddEditUserPage((User)dataGrid.SelectedValue));
        }


        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            firstNameBox.Text = null;

            dataGrid.ItemsSource = DBEntities.GetContext().Users.ToList();
        }

        private void firstNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridUpdate();
        }
    }
}

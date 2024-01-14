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
    /// Логика взаимодействия для ItemPage.xaml
    /// </summary>
    public partial class ItemPage : Page
    {
        int role;
        public ItemPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridUpdate();
        }

        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {        
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Ничего не выбрано!");
                return;
            }
               

            if (MessageBox.Show($"Не рекомендуем удолять старые записи, это может повлечь ошибку с данными. \nВы уверены?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Item items = dataGrid.SelectedItem as Item;

                    DBEntities.GetContext().Items.Remove(items);
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
            NavigationService.Navigate(new AddEditItemPage());
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;
     
            NavigationService.Navigate(new AddEditItemPage((Item)dataGrid.SelectedItem));
        }

        private void DataGridUpdate()
        {
            List<Item> items = DBEntities.GetContext().Items.ToList();

            if (tBoxName.Text.Length > 0)
            {
                string txt = tBoxName.Text.ToLower();
                items = items.Where(eve => eve.Name.ToLower().Contains(txt) ||
                eve.Description.ToLower().Contains(txt)).ToList();

            }
         
            dataGrid.ItemsSource = items;
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            tBoxName.Text = ""; 
        }

        private void tBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridUpdate();
        }
    }
}

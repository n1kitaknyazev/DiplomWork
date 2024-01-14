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
    /// Логика взаимодействия для AddEditTicketPage.xaml
    /// </summary>
    public partial class AddEditTicketPage : Page
    {
        BuyingTicket _ticet = new BuyingTicket();
        bool add = true;
        public AddEditTicketPage(BuyingTicket _ticet)
        {
 
            InitializeComponent();
            if (_ticet != null)
            {
                this._ticet = _ticet;
                add = false;
            }
        }





        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (dateSellDatePicer.SelectedDate == null || sellerCBox.SelectedItem == null || 
                eventCBox.SelectedItem == null || countTBox.Text.Length == 0)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            
            if (((Event)eventCBox.SelectedItem).AvailableOfSeat.Value < Convert.ToInt32(countTBox.Text) ||
                ((Event)eventCBox.SelectedItem).AvailableOfSeat == null ||
                ((Event)eventCBox.SelectedItem).AvailableOfSeat == 0)
            {
                MessageBox.Show("Мест не хватает!");
                return;
            }
            try
                { 
                _ticet.PurchaseDate =  (DateTime)dateSellDatePicer.SelectedDate;
                _ticet.SalesmanID = ((User)sellerCBox.SelectedItem).ID;
                _ticet.BuyerID = ((User)buyerCBox.SelectedItem).ID;
                _ticet.EventID = ((Event)eventCBox.SelectedItem).ID;
                Event evt = (Event)eventCBox.SelectedItem;
                evt.AvailableOfSeat -= Convert.ToInt32(countTBox.Text);
                _ticet.Count = Convert.ToInt32(countTBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: "+ex.Message,"Проверьте введёные данные!");
                return;
            }

            if (add)
                DBEntities.GetContext().BuyingTickets.Add(_ticet);

            try
            {
                DBEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
            buyerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            eventCBox.ItemsSource = DBEntities.GetContext().Events
                .Where(d=>d.DateStart >= DateTime.Now)
                 .Where(d => d.AvailableOfSeat > 0)
                .ToList();
            if(eventCBox.Items.Count == 0)
            {
                MessageBox.Show("Извините но доступных мироприятий нет");
                NavigationService.GoBack();
            }
            sellerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();

            if (!add)
            {
                IdTBox.Text = _ticet.Number.ToString();
                dateSellDatePicer.SelectedDate = _ticet.PurchaseDate;

                buyerCBox.SelectedItem = _ticet.User;
                sellerCBox.SelectedItem = _ticet.User1;
                eventCBox.SelectedItem = _ticet.Event;
            }
            else
            {
                dateSellDatePicer.SelectedDate = DateTime.Now;
            }



        }

        private void countTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (countTBox.Text.Length == 0 || eventCBox.SelectedValue == null)
                return;
            
            priceTBlock.Text = "Сумма: " + (Convert.ToInt32(countTBox.Text) * ((Event)eventCBox.SelectedItem).Price) +" руб.";
        }

        private void eventCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tickPriceTBlock.Text = "Стоимость одного билета: " + ((Event)eventCBox.SelectedItem).Price.ToString();
          
             tBlockSeatsCount.Text = "Кол-во мест:" + ((Event)eventCBox.SelectedItem).AvailableOfSeat.ToString();
        }
    }
}

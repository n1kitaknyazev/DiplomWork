using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ItemForEventPage.xaml
    /// </summary>
    public partial class ItemForEventPage : Page
    {
        Event events;
        public ItemForEventPage(Event events)
        {
            InitializeComponent();
            this.events = events;
        }

        private void addItemForEvent_Click(object sender, RoutedEventArgs g)
        {
            if (dataGridBase.SelectedItem == null || countItems.Text.Length < 0)
            {
                MessageBox.Show("Ошибка добавления");
                return;
            }
                  
            
            Item item = dataGridBase.SelectedItem as Item;            
            int countTBox = Convert.ToInt32(countItems.Text);
            List<ItemsForEvent> itemsForEvent = DBEntities.GetContext().ItemsForEvents
                .Where(e => e.Event.DateStart == events.DateStart).ToList();

            int count = 0;
            foreach (ItemsForEvent itemForEvent in itemsForEvent.Where(i => i.Item == item))
            {
                count += (int)itemForEvent.Quantity;
            }
            count = (int)item.Count - count;

            if (count >= countTBox)
            {
                try
                {
                    ItemsForEvent ife = new ItemsForEvent();
                    ife.Item = item;
                    ife.Quantity = countTBox;
                    ife.Event = events;

                    DBEntities.GetContext().ItemsForEvents.Add(ife);
                    DBEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Недостаточно инвентаря!");
            }

            UpdateGrid();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
            startTimeDatePicer.SelectedDate = events.DateStart;
            countTBox.Text = events.AvailableOfSeat.ToString();
            durationTBox.Text = events.Duration.ToString();
            eventNameTBox.Text = events.Name.ToString();
            descriptionTBox.Text = events.Description.ToString();
        }

        private void UpdateGrid()
        {
            
            List<Item> items = DBEntities.GetContext().Items.ToList();

            List<ItemsForEvent> itemsForEvent = DBEntities.GetContext().ItemsForEvents.ToList();
          
            dataGridEvent.ItemsSource = itemsForEvent.Where(e => e.EventsID == events.ID).ToList();

            itemsForEvent = itemsForEvent.Where(e => e.Event.DateStart.ToShortDateString() == events.DateStart.ToShortDateString()).ToList();

            List<Item> viewItems = new List<Item>();

            foreach (Item item in items)
            {                             
                int count = Convert.ToInt32(item.Count);
                foreach (ItemsForEvent itemForEvent in itemsForEvent.Where(i => i.Item == item))
                {
                    count -= (int)itemForEvent.Quantity;
                }               
                if (count > 0)
                    viewItems.Add(item);
            }

            dataGridBase.ItemsSource = viewItems;
            dataGridBase_SelectionChanged(null, null);


        }

        private void delItemForEvent_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvent.SelectedItem == null)
                return;

            var ItemForEvents = dataGridEvent.SelectedItems.Cast<ItemsForEvent>().ToList();

            if (MessageBox.Show($"Вы уверены? Удалится {ItemForEvents.Count()} элемент(ов)?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DBEntities.GetContext().ItemsForEvents.RemoveRange(ItemForEvents);
                    DBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены! ");
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdateGrid();
        }


        private void dataGridBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridBase.SelectedItem == null)
                return;

            Item _item = ((Item)dataGridBase.SelectedItem);
            List<ItemsForEvent> itemsForEvent = DBEntities.GetContext().ItemsForEvents.ToList().
                Where(g => g.Event.DateStart.ToShortDateString() == events.DateStart.ToShortDateString()).ToList();

            int count = Convert.ToInt32(_item.Count);
           
            
            foreach (ItemsForEvent itemForEvent in itemsForEvent.Where(i => i.Item == _item))
            {
                count -= (int)itemForEvent.Quantity;
            }

            VersteckTBlock.Text = "(на складе " + count + ")";

            delItemForEvent.Visibility = Visibility.Collapsed;
            addItemForEvent.Visibility = Visibility.Visible;
            countItems.Visibility = Visibility.Visible;
        }

        private void dataGridEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridEvent.SelectedItem == null)
                return;
            Item _item = ((ItemsForEvent)dataGridEvent.SelectedItem).Item;

            delItemForEvent.Visibility = Visibility.Visible;
            addItemForEvent.Visibility = Visibility.Collapsed;
            countItems.Visibility = Visibility.Collapsed;
            
            //countTBox.Visibility = Visibility.Collapsed;
        }
    }
}

using Microsoft.Office.Interop.Excel;
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
    /// Логика взаимодействия для TicketPage.xaml
    /// </summary>
    public partial class TicketPage : System.Windows.Controls.Page
    {
        int role;
        public TicketPage(int role)
        {
            this.role = role;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridUpdate();



            clientCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            clientCBox.DisplayMemberPath = "FirstName";

            sellerCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            sellerCBox.DisplayMemberPath = "FirstName";

        }



        private void deletBtn_Click(object sender, RoutedEventArgs e)
        {
            BuyingTicket selectedRow = dataGrid.SelectedItem as BuyingTicket;

            if (MessageBox.Show($"Вы уверены?", "Внимание",
            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Event _event = selectedRow.Event;
                    _event.AvailableOfSeat += selectedRow.Count;
                    DBEntities.GetContext().BuyingTickets.Remove(selectedRow);
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
            NavigationService.Navigate(new AddEditTicketPage(null));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null)
                return;
            NavigationService.Navigate(new AddEditTicketPage((BuyingTicket)dataGrid.SelectedValue));
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {

            clientCBox.SelectedValue = null;
            startDatePicer.SelectedDate = null;
            endDatePicer.SelectedDate = null;
            sellerCBox.SelectedValue = null;

            DataGridUpdate();
        }

        private void DataGridUpdate()
        {

            List<BuyingTicket> tickets = DBEntities.GetContext().BuyingTickets.ToList();

            if (clientCBox.SelectedItem != null)
                tickets = tickets.Where(eve => eve.User == (User)clientCBox.SelectedValue).ToList();
            if (startDatePicer.SelectedDate != null)
                tickets = tickets.Where(eve => eve.PurchaseDate >= startDatePicer.SelectedDate).ToList();
            if (endDatePicer.SelectedDate != null)
                tickets = tickets.Where(eve => eve.PurchaseDate <= endDatePicer.SelectedDate).ToList();
            if (sellerCBox.SelectedItem != null)
                tickets = tickets.Where(eve => eve.User1 == (User)sellerCBox.SelectedValue).ToList();

            dataGrid.ItemsSource = tickets.ToList();
        }
        /// <summary>
        /// Формирование отчётности
        /// </summary>
        private void otchetBtn_Click(object sender, RoutedEventArgs e)
        {
            getOtchetTask(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
        }
        public void getOtchetTask(DateTime start, DateTime end)
        {
            //подключение таблиц
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;

            //создание страницы
            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];

            //18
            //форматирование текста
            ws.StandardWidth = 18;

            ws.Range["A1:F1"].Merge();
            ws.Range["A1"].Value = "Отчёт по продажам"; ws.Range["A1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;

            ws.Range["B2"].Value = "Дата начала:";
            ws.Range["C2"].Value = start.Year + "." + start.Month + "." + start.Day;
            ws.Range["D2"].Value = "Дата окончания:";
            ws.Range["E2"].Value = end.Year + "." + end.Month + "." + end.Day;

            ws.Range["A4"].Value = "#"; ws.Range["A6"].ColumnWidth = 6;
            ws.Range["B4"].Value = "Название мироприятия"; ws.Range["B4"].ColumnWidth = 22;
            ws.Range["C4"].Value = "Дата продажи";
            ws.Range["D4"].Value = "Цена продажи";
            ws.Range["E4"].Value = "Кол-во"; ws.Range["e4"].ColumnWidth = 9;
            ws.Range["F4"].Value = "Сумма"; ws.Range["F4"].ColumnWidth = 10;

            List<BuyingTicket> bt = DBEntities.GetContext().BuyingTickets
            .Where(t => t.PurchaseDate >= start).ToList();

            int startPoint = 5;
            int point = startPoint;
            foreach (BuyingTicket t in bt)
            {

                ws.Range["A" + point].Value = point - 4;
                ws.Range["B" + point].Value = t.Event.Name;
                ws.Range["C" + point].Value = t.PurchaseDate;
                ws.Range["D" + point].Value = t.Event.Price;
                ws.Range["E" + point].Value = t.Count;
                ws.Range["F" + point].Formula = "=D" + point + "*E" + point;
                point++;
            }


            ws.Range["B" + point].Value = "Итого:";
            ws.Range["E" + point].Formula = "=СУММ(E" + startPoint + ":E" + (point - 1) + ")";
            ws.Range["F" + point].Formula = "=СУММ(F" + startPoint + ":F" + (point - 1) + ")";

            app.Calculation = XlCalculation.xlCalculationAutomatic;
            ws.Calculate();
        }

        private void chekBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            BuyingTicket bTick = dataGrid.SelectedItem as BuyingTicket;
            try
            {
                string n0 = bTick.Event.Name;
                string n1 = bTick.Event.Organizer.Name;
                string n2 = bTick.Event.Price.ToString();
                string n3 = bTick.Event.DateStart.ToString();
                string n4 = bTick.User1.LastName + " " + bTick.User1.FirstName;
                string n5 = bTick.Number.ToString();
                string n6 = bTick.PurchaseDate.ToString();
                string n7 = bTick.User.LastName + " " + bTick.User.FirstName;
                string n8 = bTick.Count.ToString();
                string n9 = (bTick.Count * bTick.Event.Price).ToString();
                ChekWindow window
                = new ChekWindow(n0,
                                    n1,
                                    n2,
                                    n3,
                                    n4,
                                    n5,
                                    n6,
                                    n7,
                                    n8,
                                    n9);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                MessageBox.Show("Оформите чек в ручную.", "Сообщение");
                return;
            }
        }

        private void sellerCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridUpdate();
        }
    }
}

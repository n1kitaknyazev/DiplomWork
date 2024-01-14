using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShiraRDKWork
{
    /// <summary>
    /// Логика взаимодействия для AddEditVersteckPage.xaml
    /// </summary>
    public partial class AddEditVersteckPage : Page
    {
        WareHouse _wareHouse = new WareHouse();
       
        bool add = true;
        int doEditCount = 0;

        Item _item = new Item();
        BitmapImage _image;
        List<User> organizers = new List<User>();

        public AddEditVersteckPage(WareHouse _wareHouse)
        {         
            InitializeComponent();
            if (_wareHouse != null)
            {
                this._wareHouse = _wareHouse;
                this._item = _wareHouse.Item;
                add = false;
                doEditCount = (int)this._wareHouse.Quantity;
            }     
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            userIdCBox.ItemsSource = DBEntities.GetContext().Users.ToList();
            userIdCBox.DisplayMemberPath = "FirstName";

            itemCBox.ItemsSource = DBEntities.GetContext().Items.ToList();
            itemCBox.DisplayMemberPath = "Name";

            if (!add)
            {
               
                recepiptDatePicer.SelectedDate = _wareHouse.DateOfReceipt;
                countTBox.Text = _wareHouse.Quantity.ToString();
                userIdCBox.SelectedItem = _wareHouse.User;
                itemCBox.SelectedItem = _wareHouse.Item;

                if(_wareHouse.Quantity< 0)
                    minusCBox.IsChecked = true;
                else 
                    minusCBox.IsChecked = false;

             
                itemNameTBox.Text = _item.Name;
                xValueTBox.Text = _item.Width.ToString();
                yValueTBox.Text = _item.Height.ToString();
                zValueTBox.Text = _item.Length.ToString();
                descriptionTBox.Text = _item.Description;
                if (_item.Image != null)
                {
                    MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                    ms.Write(_item.Image, 0, _item.Image.Length);
                    _image = ConvertToBitmap(_item.Image);
                    imageItemImg.Source = _image;
                }

                minusCBox.IsEnabled = false;
            }
            else
            {
                recepiptDatePicer.SelectedDate = DateTime.Now;
               
                addNewItemBtn.IsEnabled = true;
            }

        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (recepiptDatePicer.SelectedDate == null || countTBox.Text == null ||
                userIdCBox.SelectedItem == null)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }
            try
            {
                _item = itemCBox.SelectedItem as Item;

                _wareHouse.DateOfReceipt = (DateTime)recepiptDatePicer.SelectedDate;
                _wareHouse.UserID = ((User)userIdCBox.SelectedItem).ID;
                
                if(minusCBox.IsChecked.Value)
                    _wareHouse.Quantity = Convert.ToInt32("-"+countTBox.Text);
                else
                    _wareHouse.Quantity = Convert.ToInt32(countTBox.Text);

                _wareHouse.ItemID = _item.ID;


                if (minusCBox.IsChecked.Value)
                {
                   if( Convert.ToInt32(_item.Count) + doEditCount + _wareHouse.Quantity >= 0)
                        _item.Count = Convert.ToInt32(_item.Count) + doEditCount + _wareHouse.Quantity;
                else
                {
                    MessageBox.Show("Кол-во предметов на складе уйдёт в минус!", "Операция прервана");
                    return;
                }
                }
                else
                    _item.Count = Convert.ToInt32(_item.Count) - doEditCount + Convert.ToInt32(countTBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Проверьте введёные данные!");
                return;
            }
            if (add)
            {
                DBEntities.GetContext().WareHouses.Add(_wareHouse);
            }

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



        private void deletImageBtn_Click(object sender, RoutedEventArgs e)
        {
            _image = null;
            imageItemImg.Source = null;
        }


     
        private void addNewItemBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditItemPage());
        }

        private void itemIdCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _item = itemCBox.SelectedItem as Item;
            //TODO: при выборе изменять активный предмет 
            itemCBox.SelectedItem = _item;
            itemNameTBox.Text = _item.Name;
            xValueTBox.Text = _item.Width.ToString();
            yValueTBox.Text = _item.Height.ToString();
            zValueTBox.Text = _item.Length.ToString();
            widhValueTBox.Text = _item.Weight.ToString();
            descriptionTBox.Text = _item.Description;
            if (_item.Image != null)
            {//конвертация из базы
                MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                ms.Write(_item.Image, 0, _item.Image.Length);
                _image = ConvertToBitmap(_item.Image);
                imageItemImg.Source = _image;
            }
            else
                imageItemImg.Source = null;
        }


        private void xValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void yValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void zValueTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void countTBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
        public BitmapImage ConvertToBitmap(byte[] value)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(value);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                MessageBox.Show("Ошибка изображения в базе данных");
            }
            return null;

        }

        public byte[] ConvertToArray(BitmapImage value)
        {
            BitmapImage image = (BitmapImage)value;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }


    }
}

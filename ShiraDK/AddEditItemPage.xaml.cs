using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditItemPage.xaml
    /// </summary>
    public partial class AddEditItemPage : Page
    {
        Item _item = new Item();
        bool add = true;
        BitmapImage _image;
        public AddEditItemPage()
        {
            InitializeComponent();
            add = true;
            
        }
        public AddEditItemPage(Item item)
                {
            
            InitializeComponent();
            this._item = item;
            add = false;
        }

        private void imageUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                _image = new BitmapImage(new Uri(filename));
                imageItemImg.Source = _image;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (itemNameTBox.Text.Length < 0)
            {
                MessageBox.Show("Не все поля были заполнены");
                return;
            }

            try
            {
                _item.Name = itemNameTBox.Text;
                _item.Width = Convert.ToDouble(xValueTBox.Text);
                _item.Height = Convert.ToDouble(yValueTBox.Text);
                _item.Length = Convert.ToDouble(zValueTBox.Text);
                _item.Weight = Convert.ToDouble(widhValueTBox.Text);
                _item.Description = descriptionTBox.Text;

                if (_image != null)
                    _item.Image = ConvertToArray(_image);
                else
                    _item.Image = null;
                if (add)
                {
                    DBEntities.GetContext().Items.Add(_item);
                    add = false;
                }

                DBEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно изменены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте введёные данные!");
                return;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!add)
            {
                itemNameTBox.Text = _item.Name;
                xValueTBox.Text = _item.Width.ToString();
                yValueTBox.Text = _item.Height.ToString();
                zValueTBox.Text = _item.Length.ToString();
                widhValueTBox.Text = _item.Weight.ToString();
                descriptionTBox.Text = _item.Description;
                if (_item.Image != null)
                {
                    MemoryStream ms = new MemoryStream(_item.Image, 0, _item.Image.Length);
                    ms.Write(_item.Image, 0, _item.Image.Length);
                    _image = ConvertToBitmap(_item.Image);
                    imageItemImg.Source = _image;
                }      
            }
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

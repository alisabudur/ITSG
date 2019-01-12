using Microsoft.Win32;
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

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                SourceImage.Source = bitmap;
                DestImage.Source = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SourceImage.Source == null)
            {
                MessageBox.Show("Please load an image");
                return;
            }

            // do training and when complete display message
            MessageBox.Show("Training complete");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SourceImage.Source == null)
            {
                MessageBox.Show("Please load an image");
                return;
            }

            // process source image bitmap
            var processedImage = SourceImage.Source;
            DestImage.Source = processedImage;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            EpochLabel.Visibility = Visibility.Visible;
            EpochTxt.Visibility = Visibility.Visible;
            MinErrorLabel.Visibility = Visibility.Visible;
            MinErrorTxt.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            EpochLabel.Visibility = Visibility.Hidden;
            EpochTxt.Visibility = Visibility.Hidden;
            MinErrorLabel.Visibility = Visibility.Hidden;
            MinErrorTxt.Visibility = Visibility.Hidden;
        }
    }
}

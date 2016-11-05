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
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Windows.Interop;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for ChangeInformationWindow.xaml
    /// </summary>
    public partial class ChangeInformationWindow : Window
    {
        private byte[] Data { get; set; }
        private BitmapImage Image { get; set; }

        public ChangeInformationWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;

            textBlockInformation.Text = "Profile: "+ ClientInformation.Username;
            textBoxEmail.Text = ClientInformation.Email;
            passwordBox.Password = ClientInformation.Password;
            if (ClientInformation.Image == null)
                image.Source = new BitmapImage(new Uri(@"/Images/default_avatar.png", UriKind.Relative));
            else
            {
                image.Source = ClientInformation.ToImage(ClientInformation.Image);
                /*Bitmap bitMap;
                using (var ms = new MemoryStream(ClientInformation.Image))
                {
                    bitMap = new Bitmap(ms);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch { }
                    try
                    
                    ms.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    image.Source = bitmapImage;
                }*/
            }


        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                image.Source = new BitmapImage(new Uri(filename));
                Image = (BitmapImage)image.Source;


            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                Data = ms.ToArray();
            }
            ClientInformation.CommunicationService.UpdateProfile(ClientInformation.Username, textBoxEmail.Text, passwordBox.Password, Data);
            Close();

        }
    }
}

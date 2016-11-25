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

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for ShareScreenEnding.xaml
    /// </summary>
    public partial class ShareScreenEnding : Window
    {
        private string Partner;

        public ShareScreenEnding(string partner)
        {
            InitializeComponent();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = "Share screen with " + partner + ".";
            Partner = partner;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.ShareScreenEndingWindows.Remove(Partner);
            if (ClientInformation.MainWindow.myGuest != null)
            {
                ClientInformation.MainWindow.myGuest.TerminateConnection();
                ClientInformation.MainWindow.myGuest = null;
            }
            Close();
        }

        private void Closing_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientInformation.ShareScreenEndingWindows.Remove(Partner);
            if (ClientInformation.MainWindow.myGuest != null)
            {
                ClientInformation.MainWindow.myGuest.TerminateConnection();
                ClientInformation.MainWindow.myGuest = null;
            }
        }

        public void DeclineRequest()
        {
            textBlock.Text = Partner + " has declined your request.";
        }
    }
}

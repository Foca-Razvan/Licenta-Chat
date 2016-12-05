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
        private bool ok = false;
        private bool status = true;
        private bool group = false;

        public ShareScreenEnding(string partner,bool groupStatus)
        {
            InitializeComponent();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = "Share screen with " + partner + ".";
            Partner = partner;
            ClientInformation.MainWindow.RdpSession.Resume();
            AvatarImage.Fill = new ImageBrush(ClientInformation.MainWindow.GetImageFromFriendList(Partner));
            group = groupStatus;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.ShareScreenEndingWindows.Remove(Partner);
            if (group)
            {
                ClientInformation.ScreenShareService.GroupEndShareScreen(ClientInformation.Username, Partner);
                GroupConversationWindow group;
                ClientInformation.GroupConversationWindows.TryGetValue(Partner, out group);
                group.CloseShareScreen();
            }
            else
                ClientInformation.ScreenShareService.EndShareScreen(ClientInformation.Username, Partner);
            ok = true;
            Close();
        }

        private void Closing_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!ok)
            {
                ClientInformation.ShareScreenEndingWindows.Remove(Partner);
                ClientInformation.ScreenShareService.EndShareScreen(ClientInformation.Username, Partner);
            }
        }

        public void DeclineRequest()
        {
            textBlock.Text = Partner + " has declined your request.";
            button.Visibility = Visibility.Hidden;
            buttonPause.Visibility = Visibility.Hidden;
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if (status)
            {
                ClientInformation.MainWindow.RdpSession.Pause();
                buttonPause.Content = "Resume";
                status = false;
            }
            else
            {
                ClientInformation.MainWindow.RdpSession.Resume();
                buttonPause.Content = "Pause";
                status = true;
            }
        }
    }
}

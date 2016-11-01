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
    /// Interaction logic for FriendNotificationWindow.xaml
    /// </summary>
    public partial class FriendNotificationWindow : Window
    {
        private string Sender;

        public FriendNotificationWindow(string sender)
        {
            InitializeComponent();
            Sender = sender;
            textBlockInformation.Text = Sender + " wants to add you to his friend list.";
            textBlockInformation.TextAlignment = TextAlignment.Center;
            ResizeMode = ResizeMode.CanMinimize;

        }

        private void buttonAcceot_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.CommunicationService.AcceptFriendRequest(Sender);
            Close();
        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.CommunicationService.DeclineFriendRequest(Sender);
            Close();
        }
    }
}

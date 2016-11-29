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
    /// Interaction logic for GroupConversationNotificationWindow.xaml
    /// </summary>
    public partial class GroupConversationNotificationWindow : Window
    {
        public List<string> Partners;
        private string GroupName { get; set; }
        private bool ok = false;

        public GroupConversationNotificationWindow(string sender,string groupName)
        {
            InitializeComponent();
            GroupName = groupName;
            textBlock.Text = sender + " has invited you to " + groupName + " group";
            //Partners = partners;
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (ClientInformation.CommunicationService.AcceptGroupRequest(ClientInformation.Username, GroupName))
            {
                ok = true;
                GroupConversationWindow window = new GroupConversationWindow(GroupName);
                ClientInformation.GroupConversationWindows.Add(GroupName, window);
                window.Show();
                Close();
            }
            textBlock.Text = "The " + GroupName + " doesn't exist anymore";
            buttonAccept.Visibility = Visibility.Hidden;
            buttonDecline.Visibility = Visibility.Hidden;
        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            ClientInformation.CommunicationService.DeclineGroupRequest(ClientInformation.Username, GroupName);
            ok = true;
            Close();
        }

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!ok)
                ClientInformation.CommunicationService.DeclineGroupRequest(ClientInformation.Username, GroupName);
        }
    }
}

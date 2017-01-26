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
    /// Interaction logic for AddFriendConversationWindow.xaml
    /// </summary>
    public partial class AddFriendConversationWindow : Window
    {
        public List<string> Partners { get; set;}
        public string GroupName { get; set; }

        public AddFriendConversationWindow(List<string> partners,string groupName)
        {
            InitializeComponent();
            Partners = partners;
            GroupName = groupName;
            FriendListLoad();
            Title = "Invitation";
        }

        public void FriendListLoad()
        {
            List<FriendData> list = ClientInformation.MainWindow.Friends.GetOnlineFriends();
            foreach (FriendData item in list)
                if(!Partners.Exists( x => x == item.Username))
                    listViewFriendList.Items.Add(item);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FriendData friend = (FriendData)listViewFriendList.SelectedItem;
                ClientInformation.CommunicationService.InviteToGroupConversation(ClientInformation.Username, friend.Username, GroupName);
                Close();
            }
            catch { }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

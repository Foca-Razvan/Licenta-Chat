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
using RDPCOMAPILib;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for GroupConversationWindow.xaml
    /// </summary>
    public partial class GroupConversationWindow : Window
    {
        public List<string> Partners = new List<string>();
        public string GroupName { get; set;}

        public IRDPSRAPIInvitation Invitation;
        private List<string> ShareScreenPartners = new List<string>();

        public GroupConversationWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            Height = 110;
            Width = 270;
            textBoxConversation.IsReadOnly = true;
        }

        public GroupConversationWindow(string groupName)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            Partners = ClientInformation.CommunicationService.GetGroupMembers(groupName);
            Partners.Remove(ClientInformation.Username);

            textBoxName.Visibility = Visibility.Hidden;
            buttonCreate.Visibility = Visibility.Hidden;
            textBoxConversation.IsReadOnly = true;
            GroupName = groupName;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddFriendConversationWindow window = new AddFriendConversationWindow(Partners, GroupName);
            window.ShowDialog();
        }

        private void textBoxMessage_PressEnterDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxConversation.Text += ClientInformation.Username + " " + DateTime.Now + ":" + textBoxMessage.Text + "\n";
                ClientInformation.CommunicationService.SendGroupMessage(ClientInformation.Username, GroupName, textBoxMessage.Text);
                textBoxMessage.Clear();
            }
        }

        private void buttonCall_Click(object sender, RoutedEventArgs e)
        {
            // TO DO
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (ClientInformation.MainWindow.RdpSession == null)
            {
                ClientInformation.MainWindow.RdpSession = new RDPSession();
                ClientInformation.MainWindow.RdpSession.OnAttendeeConnected += Incoming;
                ClientInformation.MainWindow.RdpSession.Open();
            }

            if (Invitation == null)
            {
                Invitation = ClientInformation.MainWindow.RdpSession.Invitations.CreateInvitation("auth" + ClientInformation.authNr++, "group" + ClientInformation.groupNr++, "", 10);
                ClientInformation.authNr++;
                ClientInformation.groupNr++;

                ClientInformation.ScreenShareService.InitShareScreenGroup(ClientInformation.Username, GroupName, Invitation.ConnectionString);
                ShareScreenEnding window = new ShareScreenEnding(GroupName, true);
                foreach (string partner in Partners)
                {
                    //ClientInformation.ShareScreenEndingWindows.Add(partner, window);
                    ShareScreenPartners.Add(partner);
                }
                ClientInformation.ShareScreenEndingWindows.Add(GroupName, window);
                window.Show();
            }
            else
            {
                ClientInformation.ScreenShareService.InitShareScreenGroup(ClientInformation.Username, GroupName, Invitation.ConnectionString);
                foreach (string partner in Partners)
                    if (!ShareScreenPartners.Exists(x => x == partner))
                        ShareScreenPartners.Add(partner);

            }



        }

        private void Incoming(object partner)
        {
            IRDPSRAPIAttendee myGuest = (IRDPSRAPIAttendee)partner;
            myGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
        }

        public void UserJoined(string user)
        {
            textBoxConversation.Text += user + " has joined.\n";
            Partners.Add(user);
        }

        public void UserLeft(string user)
        {
            textBoxConversation.Text += user + " left.\n";
            Partners.Remove(user);
            ClientInformation.ShareScreenEndingWindows.Remove(user);
            ClientInformation.CallingWindows.Remove(user);
            ClientInformation.AnswerWindows.Remove(user);
            ShareScreenPartners.Remove(user);
        }

        public void UserLeftShareScreen(string user)
        {
            textBoxConversation.Text += user + " left share screen group.\n";
            ClientInformation.ShareScreenEndingWindows.Remove(user);
            ClientInformation.ShareScreenWindows.Remove(user);
            ShareScreenPartners.Remove(user);
            if (ShareScreenPartners.Count == 0)
            {
                ShareScreenEnding window;
                ClientInformation.ShareScreenEndingWindows.TryGetValue(GroupName, out window);
                if (window != null)
                    window.AllUsersLeft();
                ClientInformation.ShareScreenEndingWindows.Remove(GroupName);
                Invitation = null;
            }
        }

        public void UserRefusedSharreScreenGroup(string user)
        {
            ClientInformation.ShareScreenEndingWindows.Remove(user);
            ClientInformation.ShareScreenWindows.Remove(user);
            ShareScreenPartners.Remove(user);
            if (ShareScreenPartners.Count == 0)
            {
                ShareScreenEnding window;
                ClientInformation.ShareScreenEndingWindows.TryGetValue(GroupName, out window);
                if (window != null)
                    window.AllUserRefused();
                ClientInformation.ShareScreenEndingWindows.Remove(GroupName);
                Invitation = null;
            }
        }

        public void UserRefused(string user)
        {
            textBoxConversation.Text += user + " refused to join.\n";
        }

        public void CloseShareScreen()
        {
            foreach (string member in Partners)
            {
                ClientInformation.ShareScreenEndingWindows.Remove(member);
                ClientInformation.ShareScreenWindows.Remove(member);
            }
            ClientInformation.ShareScreenEndingWindows.Remove(GroupName);
            Invitation = null;
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            GroupName = textBoxName.Text;

            if (ClientInformation.CommunicationService.CreateGroupConversation(ClientInformation.Username, GroupName))
            {
                Height = 374;
                Width = 562.67;                
                textBoxName.Visibility = Visibility.Hidden;
                buttonCreate.Visibility = Visibility.Hidden;

                ClientInformation.GroupConversationWindows.Add(GroupName, this);
            }        
        }

        public void ReceiveMessage(string sender,string message)
        {
            textBoxConversation.Text += sender + " " + DateTime.Now + ": " + message + "\n";
        }

        private void OnClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GroupName != null)
            {
                ShareScreenEnding screenShare;
                ClientInformation.ShareScreenEndingWindows.TryGetValue(GroupName, out screenShare);
                if (screenShare != null)
                    screenShare.Close();

                CloseShareScreen();

                ClientInformation.CommunicationService.LeaveGroup(ClientInformation.Username, GroupName);
                ClientInformation.GroupConversationWindows.Remove(GroupName);
                ClientInformation.ShareScreenEndingWindows.Remove(GroupName);
            }


        }
    }
}

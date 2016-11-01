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
using System.ServiceModel;
using Interfaces;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using RDPCOMAPILib;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RDPSession rdpSession;
        string ConversationPartner { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            textBoxConversation.IsReadOnly = true;

            CommunicationServiceCallback callback = new CommunicationServiceCallback(this);  
            DuplexChannelFactory<ICommunication> channelServerService = new DuplexChannelFactory<ICommunication>(callback, new NetTcpBinding(SecurityMode.None),
                new EndpointAddress("net.tcp://" + ClientInformation.IPAdressServer + ":4444/CommunicationService"));
            ClientInformation.CommunicationService = channelServerService.CreateChannel();
            ClientInformation.CommunicationService.Subscribe(ClientInformation.Username);

            ClientInformation.scrrenShareCallback = new ScreenShareCallback();
            DuplexChannelFactory<IScreenShare> channelScreenShare = new DuplexChannelFactory<IScreenShare>(ClientInformation.scrrenShareCallback, new NetTcpBinding(SecurityMode.None),
                new EndpointAddress("net.tcp://" + ClientInformation.IPAdressServer + ":4444/ScreenShareService"));
            ClientInformation.ScreenShareService = channelScreenShare.CreateChannel();
            ClientInformation.ScreenShareService.Subscribe(ClientInformation.Username);

            Title = ClientInformation.Username;
            laber_username.Content = ClientInformation.Username;
            avatar_image.Source = new BitmapImage(new Uri(@"/Images/default_avatar.png", UriKind.Relative));
            comboBox_status.Items.Add("Online");
            comboBox_status.Items.Add("Away");
            comboBox_status.Items.Add("Offline");
            comboBox_status.SelectedItem = "Online";
            ResizeMode = ResizeMode.CanMinimize;

            ClientInformation.CommunicationService.GetNotifications();
        }

        private void textBoxMessage_PressEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                textBoxConversation.Text += Title + ": " + textBoxMessage.Text + "\n";
                ClientInformation.CommunicationService.SendMessage(textBoxMessage.Text,ConversationPartner);
                textBoxMessage.Clear();
            }
        }

        private void listBoxFriendList_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = ClientInformation.CommunicationService.GetFriendList();
            foreach(string username in list)
                listBoxFriendList.Items.Add(username);
                

        }

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientInformation.CommunicationService.Logout();
        }

        private void MouseDoubleClick_Event(object sender, MouseButtonEventArgs e)
        {
            if (listBoxFriendList.SelectedItem is string)
                ConversationPartner = (string)listBoxFriendList.SelectedItem;
        }

        private void MenuItemApplicationLogoutEvent(object sender, RoutedEventArgs e)
        {
            if (ClientInformation.CommunicationService.Logout())
            {
                Login login = new Login();
                login.Show();
                Close();
            }
        }

        private void MenuItemApplicationCloseClickEvent(object sender, RoutedEventArgs e)
        {
            if(ClientInformation.CommunicationService.Logout())
                Close();
        }

        private void MenuItemConversationCallMouseClickEvent(object sender, RoutedEventArgs e)
        {
            if (ConversationPartner != null)
            {
                CallingWindow callingWindow = new CallingWindow(ConversationPartner);
                ClientInformation.CallingWindows.Add(ConversationPartner, callingWindow);
                callingWindow.Show();
            }         
        }

        private void MenuItemConversationShareScreenMouseClickEvent(object sender ,RoutedEventArgs e)
        {
            if(ConversationPartner != null)
            {
                rdpSession = new RDPSession();
                rdpSession.OnAttendeeConnected += Incoming;
                rdpSession.Open();

                IRDPSRAPIInvitation Invitation = rdpSession.Invitations.CreateInvitation("Trial", "MyGroup", "", 10);
                ClientInformation.ScreenShareService.InitShareScreen(ClientInformation.Username, ConversationPartner,Invitation.ConnectionString);           
            }
        }

        private void Incoming(object partner)
        {
            IRDPSRAPIAttendee myGuest = (IRDPSRAPIAttendee)partner;
            myGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
        }

        private void listBoxFriendListMouseDown_Event(object sender, MouseButtonEventArgs e)
        {           
            if (listBoxFriendList.SelectedItem is string)
                ConversationPartner = (string)listBoxFriendList.SelectedItem;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddFriendWindow window = new AddFriendWindow();
            window.Show();
        }
    }
}

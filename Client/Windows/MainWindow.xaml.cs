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
using Client.Data;
using Client.Windows;

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

            NetTcpBinding tcp = new NetTcpBinding(SecurityMode.None);
            tcp.MaxReceivedMessageSize = 20000000;

            CommunicationServiceCallback callback = new CommunicationServiceCallback(this);  
            DuplexChannelFactory<ICommunication> channelServerService = new DuplexChannelFactory<ICommunication>(callback,tcp,
                new EndpointAddress("net.tcp://" + ClientInformation.IPAdressServer + ":4444/CommunicationService"));
            ClientInformation.CommunicationService = channelServerService.CreateChannel();
            ClientInformation.CommunicationService.Subscribe(ClientInformation.Username);

            NetTcpBinding tcp1 = new NetTcpBinding(SecurityMode.None);
            tcp1.MaxReceivedMessageSize = 20000000;

            ClientInformation.scrrenShareCallback = new ScreenShareCallback();
            DuplexChannelFactory<IScreenShare> channelScreenShare = new DuplexChannelFactory<IScreenShare>(ClientInformation.scrrenShareCallback,tcp1,
                new EndpointAddress("net.tcp://" + ClientInformation.IPAdressServer + ":4444/ScreenShareService"));
            ClientInformation.ScreenShareService = channelScreenShare.CreateChannel();
            ClientInformation.ScreenShareService.Subscribe(ClientInformation.Username);

            Title = ClientInformation.Username;
            laber_username.Content = ClientInformation.Username;

            byte[] image = ClientInformation.CommunicationService.GetAvatarImage(ClientInformation.Username);
            if (image == null)
                avatar_image.Source = new BitmapImage(new Uri(@"/Images/default_avatar.png", UriKind.Relative));
            else
                avatar_image.Source = ClientInformation.ToImage(image);
            ResizeMode = ResizeMode.CanMinimize;

            ClientInformation.CommunicationService.GetNotifications();
            ClientInformation.CommunicationService.GetInformation(ClientInformation.Username);
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

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientInformation.CommunicationService.Logout();
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddFriendWindow window = new AddFriendWindow();
            window.Show();
        }

        private void listViewFriendList_Loaded(object sender, RoutedEventArgs e)
        {
            GridViewColumn column = new GridViewColumn();
            Dictionary<string, int> list = ClientInformation.CommunicationService.GetFriendList();
            
           
            foreach (KeyValuePair<string, int> item in list)
            {
                if(item.Value == 1)
                    listViewFriendList.Items.Add(new FriendData
                    {
                        Username = item.Key,
                        Status = new BitmapImage(new Uri(@"/Images/online_status.jpg", UriKind.Relative)),
                        Image = ClientInformation.ToImage(ClientInformation.CommunicationService.GetAvatarImage(item.Key))
                    });
                else
                    listViewFriendList.Items.Add(new FriendData
                    {
                        Username = item.Key,
                        Status = new BitmapImage(new Uri(@"/Images/offline_circle.jpg", UriKind.Relative)),
                        Image = ClientInformation.ToImage(ClientInformation.CommunicationService.GetAvatarImage(item.Key))
                    });
            }
        }

        private void MouseLeftButtonDown_avatarImage(object sender, MouseButtonEventArgs e)
        {     
            ChangeInformationWindow window = new ChangeInformationWindow();
            window.ShowDialog();
        }
    }
}

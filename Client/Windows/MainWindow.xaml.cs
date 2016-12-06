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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RDPSession RdpSession { get; set; }
        public FriendDataView Friends { get; set; }

        public MainWindow()
        {
            InitializeComponent();

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
            ResizeMode = ResizeMode.CanMinimize;

            ClientInformation.CommunicationService.GetNotifications();          
            listViewFriendListLoad();
            AvatarImageLoad(ClientInformation.CommunicationService.GetAvatarImage(ClientInformation.Username));

            ClientInformation.CommunicationService.GetInformation(ClientInformation.Username);
        }

        private void listViewFriendListLoad()
        {
            Dictionary<string, int> list = ClientInformation.CommunicationService.GetFriendList();
            Friends = new FriendDataView();

            listViewFriendList.DataContext = Friends.Items;
            listViewFriendList.ItemsSource = Friends.Items;
            foreach (KeyValuePair<string, int> item in list)
            {
                FriendData data = new FriendData();
                data.Username = item.Key;
                data.AvatarImage = ClientInformation.ToImage(ClientInformation.CommunicationService.GetAvatarImage(item.Key));

                if (item.Value == 1)
                {
                    data.StatusImage = new BitmapImage(new Uri(@"/Images/online_status.jpg", UriKind.Relative));
                    data.Status = true;
                    Friends.Items.Add(data);
                }
                else
                {
                    data.StatusImage = new BitmapImage(new Uri(@"/Images/offline_circle.jpg", UriKind.Relative));
                    data.Status = false;
                    Friends.Items.Add(data);
                }
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewFriendList.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));
        }

        public void AvatarImageLoad(byte[] image)
        {
            if (image == null)
                avatar_image.Fill = new ImageBrush(new BitmapImage(new Uri(@"/Images/default_avatar.png", UriKind.Relative)));
            else
                avatar_image.Fill = new ImageBrush(ClientInformation.ToImage(image));
        }

        public BitmapImage GetImageFromFriendList(string username)
        {
            FriendData data = Friends.Items.ToList().Find(x => x.Username == username);
            if (data != null)
                return data.AvatarImage;
            else
                return null;
        }

        private void ClosingEvent(object sender, CancelEventArgs e)
        {
            try
            {
                ClientInformation.CommunicationService.Logout();
            }
            catch {  }
        }


        private void MenuItemApplicationLogoutEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientInformation.CommunicationService.Logout())
                {
                    Login login = new Login();
                    login.Show();
                    Close();
                }
            }
            catch { Close(); }
        }

        private void MenuItemApplicationCloseClickEvent(object sender, RoutedEventArgs e)
        {
            if(ClientInformation.CommunicationService.Logout())
                Close();
        }

        private void Incoming(object partner)
        {
            IRDPSRAPIAttendee myGuest = (IRDPSRAPIAttendee)partner;
            myGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;
        }

        private void Disconnected(object partner)
        {
            //IRDPSRAPIAttendee myGuest = (IRDPSRAPIAttendee)partner;
            //myGuest.TerminateConnection();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddFriendWindow window = new AddFriendWindow();
            window.Show();
        }

        private void listViewFriendList_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(FriendData item in listViewFriendList.Items)
            {
                if(!item.Status)
                {
                    ListViewItem row = listViewFriendList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    row.Opacity = 0.5;
                }
            }          
        }

        public void listViewFriendListRefreshOpacity()
        {
            foreach (FriendData item in listViewFriendList.Items)
            {
                if (!item.Status)
                {
                    ListViewItem row = listViewFriendList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    row.Opacity = 0.5;
                }
            }
        }

        private void MouseLeftButtonDown_avatarImage(object sender, MouseButtonEventArgs e)
        {     
            ChangeInformationWindow window = new ChangeInformationWindow();
            window.ShowDialog();
        }

        private void buttonCall_click(object sender,RoutedEventArgs e)
        {
            Button b = sender as Button;
            FriendData data = b.CommandParameter as FriendData;

            if (data.Status && !ClientInformation.AnswerWindows.ContainsKey(data.Username) && !ClientInformation.CallingWindows.ContainsKey(data.Username))
            {
                CallingWindow callingWindow = new CallingWindow(data.Username,data.AvatarImage);
                ClientInformation.CallingWindows.Add(data.Username, callingWindow);
                callingWindow.Show();
            }
        }

        private void buttonConversation_click(object sender ,RoutedEventArgs e)
        {
            Button b = sender as Button;
            FriendData data = b.CommandParameter as FriendData;

            if(!ClientInformation.ConversationsWindows.ContainsKey(data.Username))
            {
                ConversationWindow window = new ConversationWindow(data.Username);
                ClientInformation.ConversationsWindows.Add(data.Username, window);
                window.Show();
            }               
        }

        private void buttonShareScreen_click(object sender , RoutedEventArgs e)
        {
            Button b = sender as Button;
            FriendData data = b.CommandParameter as FriendData;

            if(data.Status && !ClientInformation.ShareScreenWindows.ContainsKey(data.Username))
            {

                if (RdpSession == null)
                {
                    RdpSession = new RDPSession();
                    RdpSession.OnAttendeeConnected += Incoming;
                    RdpSession.Open();

                }
                IRDPSRAPIInvitation Invitation = RdpSession.Invitations.CreateInvitation("auth" + ClientInformation.authNr++, "group"+ ClientInformation.groupNr++, "", 1);
                ClientInformation.authNr++;
                ClientInformation.groupNr++;

                ClientInformation.ScreenShareService.InitShareScreen(ClientInformation.Username, data.Username, Invitation.ConnectionString);

                ShareScreenEnding window = new ShareScreenEnding(data.Username,false);
                ClientInformation.ShareScreenEndingWindows.Add(data.Username, window);
                window.Show();
            }
        }

        private void OnMouseRightButtonDown_Handler(object sender ,RoutedEventArgs e)
        {
           
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            MenuItem source = sender as MenuItem;
            FriendData data = source.CommandParameter as FriendData;

            ClientInformation.CommunicationService.RemoveFriend(ClientInformation.Username,data.Username);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            GroupConversationWindow window = new GroupConversationWindow();
            window.Show();
        }
    }
}

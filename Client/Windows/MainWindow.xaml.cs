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
        RDPSession rdpSession;
        string ConversationPartner { get; set; }
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
            ClientInformation.CommunicationService.GetInformation(ClientInformation.Username);

            listViewFriendListLoad();
            AvatarImageLoad(ClientInformation.CommunicationService.GetAvatarImage(ClientInformation.Username));
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
            return data.AvatarImage;
        }

        /*private void textBoxMessage_PressEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                textBoxConversation.Text += Title + ": " + textBoxMessage.Text + "\n";
                ClientInformation.CommunicationService.SendMessage(textBoxMessage.Text,ConversationPartner);
                textBoxMessage.Clear();
            }
        }*/

        private void ClosingEvent(object sender, CancelEventArgs e)
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
            foreach(FriendData item in listViewFriendList.Items)
            {
                if(!item.Status)
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

            if(data.Status)
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
        }

        private void buttonShareScreen_click(object sender , RoutedEventArgs e)
        {
            Button b = sender as Button;
            FriendData data = b.CommandParameter as FriendData;

            if(data.Status)
            {
                rdpSession = new RDPSession();
                rdpSession.OnAttendeeConnected += Incoming;
                rdpSession.Open();

                IRDPSRAPIInvitation Invitation = rdpSession.Invitations.CreateInvitation("Trial", "MyGroup", "", 10);
                ClientInformation.ScreenShareService.InitShareScreen(ClientInformation.Username, data.Username, Invitation.ConnectionString);
            }
        }
    }
}

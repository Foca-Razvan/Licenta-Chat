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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ICommunication connectionService;
        IAudio audioService;
        string ConversationPartner { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            textBoxConversation.IsReadOnly = true;
        }

        public MainWindow(string username)
        {
            InitializeComponent();
            textBoxConversation.IsReadOnly = true;

            CommunicationServiceCallback callback = new CommunicationServiceCallback(this);  
            DuplexChannelFactory<ICommunication> channelServerService = new DuplexChannelFactory<ICommunication>(callback, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4444/CommunicationService"));
            connectionService = channelServerService.CreateChannel();

            /*AudioCallback audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();*/

            connectionService.Subscribe(username);
            //audioService.Subscribe(username);

            Title = username;
        }

        private void textBoxMessage_PressEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                textBoxConversation.Text += Title + ": " + textBoxMessage.Text + "\n";
                connectionService.SendMessage(textBoxMessage.Text,ConversationPartner);
                textBoxMessage.Clear();
            }
        }

        private void listBoxFriendList_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = connectionService.GetListOfContacts();
            foreach(string username in list)
                listBoxFriendList.Items.Add(username);

        }

        private void ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connectionService.Logout();
        }

        private void MouseDoubleClick_Event(object sender, MouseButtonEventArgs e)
        {
            if (listBoxFriendList.SelectedItem is string)
                ConversationPartner = (string)listBoxFriendList.SelectedItem;
        }

        private void MenuItemApplicationLogoutEvent(object sender, RoutedEventArgs e)
        {
            if (connectionService.Logout())
            {
                Login login = new Login();
                login.Show();
                Close();
            }
        }

        private void MenuItemApplicationCloseClickEvent(object sender, RoutedEventArgs e)
        {
            if(connectionService.Logout())
                Close();
        }

        private void MenuItemConversationCallMouseClickEvent(object sender, RoutedEventArgs e)
        {
            if (ConversationPartner != null)
            {
                CallingWindow callingWindow = new CallingWindow(Title,ConversationPartner,true);
                callingWindow.Show();
            }         
        }

    }
}

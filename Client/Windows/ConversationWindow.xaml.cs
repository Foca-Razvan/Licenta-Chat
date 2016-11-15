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
    /// Interaction logic for ConversationWindow.xaml
    /// </summary>
    public partial class ConversationWindow : Window
    {
        public string ConversationPartner { get; set; }

        public ConversationWindow(string username)
        {
            InitializeComponent();

            ConversationPartner = username;
            avatarImage.Fill = new ImageBrush(ClientInformation.MainWindow.GetImageFromFriendList(username));
            textBlockUsername.Text = username;
            textBoxConversation.IsReadOnly = true;
            ResizeMode = ResizeMode.CanMinimize;
        }

        private void textBoxMessage_PressEnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxConversation.Text += ClientInformation.Username + " " + DateTime.Now + ":" + textBoxMessage.Text + "\n";
                ClientInformation.CommunicationService.SendMessage(textBoxMessage.Text, ConversationPartner);
                textBoxMessage.Clear();
            }
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientInformation.ConversationsWindows.Remove(ConversationPartner);
        }
    }
}

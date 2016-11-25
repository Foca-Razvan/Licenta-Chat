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
    /// Interaction logic for GroupConversationWindow.xaml
    /// </summary>
    public partial class GroupConversationWindow : Window
    {
        public List<string> Partners = new List<string>();

        public GroupConversationWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddFriendConversationWindow window = new AddFriendConversationWindow(Partners);
            window.ShowDialog();
        }

        private void textBoxMessage_PressEnterDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxConversation.Text += ClientInformation.Username + " " + DateTime.Now + ":" + textBoxMessage.Text + "\n";
                //ClientInformation.CommunicationService.SendMessage(textBoxMessage.Text, Partner);
                textBoxMessage.Clear();
            }
        }

        private void buttonCall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender,RoutedEventArgs e)
        {

        }
    }
}

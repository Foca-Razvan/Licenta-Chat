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
using Interfaces;

namespace Client
{
    /// <summary>
    /// Interaction logic for AddFriendWindow.xaml
    /// </summary>
    public partial class AddFriendWindow : Window
    {

        public AddFriendWindow()
        {
            InitializeComponent();

            ResizeMode = ResizeMode.CanMinimize;
            textBlockInformation.Text = "Insert the username of the person you want to add in the textbox below.";
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (ClientInformation.CommunicationService.CheckUserExistance(textBoxUsername.Text))
            {
                if (!ClientInformation.CommunicationService.IsFriendWith(ClientInformation.Username, textBoxUsername.Text))
                {
                    ClientInformation.CommunicationService.AddFriend(textBoxUsername.Text);
                    Close();
                }
                else
                    textBlockInformation.Text += "\n\nThe username you introduced is already friends with you.";
            }
            else
                textBlockInformation.Text += "\n\nThe username you introduced doesn't exist.";
          
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

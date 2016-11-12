using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Controls;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using Client.Data;
using System.Windows.Media.Imaging;
using System.Media;
using System.Windows.Media;
using System.Threading;
using System.Windows.Data;
using System.ComponentModel;

namespace Client
{
    public class CommunicationServiceCallback : IClientCallback
    {
        public MainWindow MainWindow { get; set; }

        public CommunicationServiceCallback() { }

        public CommunicationServiceCallback(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public void Send(string meessage, string to)
        {
           // MainWindow.textBoxConversation.Text += to + ": " + meessage + "\n";
        }

        public void SendNotification(string username,byte[] image)
        { 
            ListViewItem row = new ListViewItem();
            foreach(FriendData item in MainWindow.Friends.Items)
            {
                if(item.Username == username)
                {
                    row = MainWindow.listViewFriendList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    row.Opacity = 1;
                    item.StatusImage = new BitmapImage(new Uri(@"/Images/online_status.jpg", UriKind.Relative));
                    break;
                }
            }
        }

        public void UpdateListOfContacts(string username)
        {
            ListViewItem row = new ListViewItem();
            FriendData _item = new FriendData();
            foreach (FriendData item in MainWindow.Friends.Items)
            {
                if (item.Username == username)
                {                    
                    item.StatusImage = new BitmapImage(new Uri(@"/Images/offline_circle.jpg", UriKind.Relative));
                    row = MainWindow.listViewFriendList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    row.Opacity = 0.4;
                    break;
                }
            }
        }

        public void SendAudioNotification(string caller)
        { 
            AnswerWindow answerWindow = new AnswerWindow(caller);
            ClientInformation.AnswerWindows.Add(caller, answerWindow);
            answerWindow.Show();
        }

        public void SendProfileInformation(string password ,string email , byte[] Image)
        {
            ClientInformation.Password = password;
            ClientInformation.Image = Image;
            ClientInformation.Email = email;
        }

    }

}

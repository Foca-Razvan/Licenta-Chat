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

namespace Client
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
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
            MainWindow.textBoxConversation.Text += to + ": " + meessage + "\n";
        }

        public void SendNotification(string username,byte[] image)
        {
            MainWindow.listViewFriendList.Items.Add(new FriendData { Username = username,
                Status = new BitmapImage(new Uri(@"/Images/online_status.jpg", UriKind.Relative)),
                Image =  ClientInformation.ToImage(image)});
        }

        public void UpdateListOfContacts(string username)
        {
           foreach(FriendData data in MainWindow.listViewFriendList.Items)
            {
                if (data.Username == username)
                {
                    MainWindow.listViewFriendList.Items.Remove(data);
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

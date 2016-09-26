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

namespace Client
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
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

        public void SendNotification(string username)
        {
            MainWindow.listBoxFriendList.Items.Add(username);
        }

        public void UpdateListOfContacts(string username)
        {
            MainWindow.listBoxFriendList.Items.Remove(username);
        }

        public void SendAudioNotification(string caller, string receiver)
        { 
            AnswerWindow answerWindow = new AnswerWindow();
            ClientInformation.AnswerWindows.Add(caller, answerWindow);
            ClientInformation.AnswerWindows[caller].Show();
        }

    }

}

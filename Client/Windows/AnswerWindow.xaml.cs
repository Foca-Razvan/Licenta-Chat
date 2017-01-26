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
using System.ServiceModel;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using Interfaces;

namespace Client
{
    /// <summary>
    /// Interaction logic for AnswerWindow.xaml
    /// </summary>
    public partial class AnswerWindow : Window
    {
        string ConversationPartner { get; set; }
        public bool IsGroup { get; set; }
        IAudio audioService;
        AudioCallback audioCallback;

        public AnswerWindow()
        {
            InitializeComponent();
        }

        public AnswerWindow(string sender,bool isGroup)
        {
            InitializeComponent();
            IsGroup = isGroup;
            ResizeMode = ResizeMode.CanMinimize;
            textBlock.TextAlignment = TextAlignment.Center;

            ConversationPartner = sender;
            textBlock.Text = sender + " is calling you.";

            audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(SecurityMode.None),
                new EndpointAddress("net.tcp://" + ClientInformation.IPAdressServer + ":4444/AudioService"));

            audioService = channelAudioService.CreateChannel();
            audioService.Subscribe(ClientInformation.Username);

            buttonClose.Visibility = Visibility.Hidden;
            ImageBrush brush = new ImageBrush(ClientInformation.MainWindow.GetImageFromFriendList(sender));
            avatarImage.Fill = brush;
            Title = sender;
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            buttonAccept.Visibility = Visibility.Hidden;
            buttonCancel.Visibility = Visibility.Hidden;
            buttonClose.Visibility = Visibility.Visible;

            WaveIn wi = new WaveIn();
            wi.WaveFormat = ClientInformation.waveFormat;
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailableCallback);

            audioCallback.Wi = wi;
            audioService.Confirmation(ClientInformation.Username, ConversationPartner, true, IsGroup);
            audioCallback.StartRecording();

            textBlock.Text = "Audio Call with " + ConversationPartner;

        }

        private void wi_DataAvailableCallback(object sender, WaveInEventArgs e)
        {
            if (IsGroup)
                audioService.SendVoiceGroup(e.Buffer, e.BytesRecorded, ConversationPartner, ClientInformation.Username);
            else
                audioService.SendVoice(e.Buffer, e.BytesRecorded, ConversationPartner);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            audioService.Confirmation(ClientInformation.Username, ConversationPartner, false, IsGroup);
            ClientInformation.AnswerWindows.Remove(ConversationPartner);
            Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, ConversationPartner,IsGroup);
            ClientInformation.AnswerWindows.Remove(ConversationPartner);
            Close();          
        }

        public void ClosedCall()
        {
            textBlock.Text = ConversationPartner + " has stopped the conversation";
            buttonCancel.Visibility = Visibility.Hidden;
            buttonAccept.Visibility = Visibility.Hidden;
            buttonClose.Visibility = Visibility.Hidden;
        }

        private void AnswerWindow_ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, ConversationPartner,IsGroup);
            ClientInformation.AnswerWindows.Remove(ConversationPartner);
        }
    }
}

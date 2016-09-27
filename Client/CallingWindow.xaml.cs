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
using Interfaces;
using NAudio;
using NAudio.WindowsMediaFormat;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Diagnostics;

namespace Client
{
    /// <summary>
    /// Interaction logic for CallingWindow.xaml
    /// </summary>
    public partial class CallingWindow : Window
    {
        string ConversationPartner { get; set; } 
        IAudio audioService;
        AudioCallback audioCallback;

        public CallingWindow()
        {
            InitializeComponent();
        }

        public CallingWindow(string conversationPartner)
        {
            InitializeComponent();
            ConversationPartner = conversationPartner;

            audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();

            audioService.Subscribe(ClientInformation.Username);

            textBlockInfo.TextAlignment = TextAlignment.Center;

            Init();
        }

        private void Init()
        {
            WaveIn wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(8000, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailableCallback);
            audioCallback.Wi = wi;

            textBlockInfo.Text = "Calling " + ConversationPartner;

            audioService.InitCommunication(ClientInformation.Username, ConversationPartner);
        }

        private void wi_DataAvailableCallback(object sender, WaveInEventArgs e)
        {
            audioService.SendVoice(e.Buffer, e.BytesRecorded, ConversationPartner);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, ConversationPartner);
            ClientInformation.CallingWindows.Remove(ConversationPartner);
            Close();
        }

        public void DeclinedCall()
        {
            textBlockInfo.Text = ConversationPartner + " has declined your call";
            buttonCancel.Visibility = Visibility.Hidden;
        }

        public void AcceptedCall()
        {
            textBlockInfo.Text = ConversationPartner + " has accepted your call.";
            buttonCancel.Content = "Close";
            audioCallback.StartRecording();
        }

        public void ClosedCall()
        {
            textBlockInfo.Text = ConversationPartner + " has stopped the conversation";
            buttonCancel.Visibility = Visibility.Hidden;
        }

        private void CallingWindow_ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, ConversationPartner);
            ClientInformation.CallingWindows.Remove(ConversationPartner);
        }
    }
}

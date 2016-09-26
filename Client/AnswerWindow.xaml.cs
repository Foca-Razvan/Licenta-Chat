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
        string Sender { get; set; }
        IAudio audioService;
        AudioCallback audioCallback;

        public AnswerWindow()
        {
            InitializeComponent();
        }

        public AnswerWindow(string sender)
        {
            InitializeComponent();
            buttonClose.Visibility = Visibility.Hidden;
            textBlock.TextAlignment = TextAlignment.Center;

            Sender = sender;
            textBlock.Text = sender + " is calling you.";

            audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            buttonAccept.Visibility = Visibility.Hidden;
            buttonCancel.Visibility = Visibility.Hidden;
            buttonClose.Visibility = Visibility.Visible;

            audioService.Subscribe(ClientInformation.Username);

            WaveIn wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(8000, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailableCallback);

            audioCallback.Wi = wi;
            audioService.Confirmation(ClientInformation.Username, Sender, true);
            audioCallback.StartRecording();

            textBlock.Text = "Audio Call with " + Sender;

        }

        private void wi_DataAvailableCallback(object sender, WaveInEventArgs e)
        {
            audioService.SendVoice(e.Buffer, e.BytesRecorded, Sender);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            audioService.Confirmation(ClientInformation.Username, Sender , false);
            ClientInformation.AnswerWindows.Remove(Sender);
            Close();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, Sender);
            ClientInformation.AnswerWindows.Remove(Sender);
            Close();
            
        }

        public void ClosedCall()
        {
            textBlock.Text = Sender + " has stopped the conversation";
            buttonCancel.Visibility = Visibility.Hidden;
            buttonAccept.Visibility = Visibility.Hidden;
            buttonCancel.Visibility = Visibility.Hidden;
        }

        private void AnswerWindow_ClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            audioCallback.StopPlayingOutput();
            audioCallback.StopRecording();
            audioService.StopCall(ClientInformation.Username, Sender);
            ClientInformation.AnswerWindows.Remove(Sender);
        }
    }
}

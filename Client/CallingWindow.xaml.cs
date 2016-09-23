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
        string Sender { get; set; }
        string Receiver { get; set; }
        bool Type { get; set; }
        WaveIn wi;
        IAudio audioService;
        AudioCallback audioCallback;

        public CallingWindow()
        {
            InitializeComponent();
        }

        public CallingWindow(string username,string ConversationPartner,bool type)
        {
            InitializeComponent();

            Receiver = ConversationPartner;
            Sender = username;
            Type = type;

            audioCallback = new AudioCallback();
            audioCallback.callingWindows = this;
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();
            if (type)
                audioService.Subscribe(Sender);
            else
                audioService.Subscribe(Receiver);

            textBlockInfo.TextAlignment = TextAlignment.Center;
        }


        private void Init()
        {
            wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(44100, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);

            audioCallback.Wi = wi;


            audioService.InitCommunication(Sender, Receiver);

        }

        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            audioService.SendVoice(e.Buffer, e.BytesRecorded, Receiver);
        }

        private void wi_DataAvailableCallback(object sender, WaveInEventArgs e)
        {
            audioService.SendVoice(e.Buffer, e.BytesRecorded, Sender);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            wi.DataAvailable += null;
            wi.StopRecording();
            audioCallback.StopPlayingOutput();
            if (Type)
                audioService.StopCall(Sender, Receiver);
            else
                audioService.StopCall(Receiver, Sender);
            Close();
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            buttonAccept.Visibility = Visibility.Hidden;
            buttonCancel.Visibility = Visibility.Visible;
            buttonDecline.Visibility = Visibility.Hidden;

            wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(44100, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailableCallback);

            audioCallback.Wi = wi;
            audioService.Confirmation(Sender, Receiver, true);
            audioCallback.StartRecording();

            textBlockInfo.Text = "Audio Call with " + Sender;
        }

        private void CallingWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Type)
            {
                buttonAccept.Visibility = Visibility.Hidden;
                buttonDecline.Visibility = Visibility.Hidden;
                buttonCancel.Visibility = Visibility.Hidden;           

                Title = "";
                textBlockInfo.Text = "Calling " + Receiver + "...";

                Init();

            }
            else
            {
                buttonCancel.Visibility = Visibility.Hidden;
                buttonCancel2.Visibility = Visibility.Hidden;
                Title = "";
                textBlockInfo.Text = Sender + " is calling you.";
            }
        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            audioService.Confirmation(Sender, Receiver, false);
            Close();
        }

        public void DeclinedCall()
        {
            if(Type)
                textBlockInfo.Text = Receiver + " has declined your call...";
            else
                textBlockInfo.Text = Sender + " has declined your call...";
            buttonCancel.Visibility = Visibility.Hidden;
            buttonCancel2.Visibility = Visibility.Hidden;
            buttonAccept.Visibility = Visibility.Hidden;
            buttonDecline.Visibility = Visibility.Hidden;
        }

        private void buttonCancel2_Click(object sender, RoutedEventArgs e)
        {
            audioService.Confirmation(Receiver, Sender, false);
            Close();
        }
    }
}

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
        }


        private void Init()
        {
            wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(8000, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);

            AudioCallback audioCallback = new AudioCallback(wi);
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();

            audioService.Subscribe(Sender);
            audioService.InitCommunication(Sender, Receiver);

        }

        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            audioService.SendVoice(e.Buffer, e.BytesRecorded, Receiver);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {           
            audioService.StopCall(Sender, Receiver);
            wi.DataAvailable += null;
            wi.StopRecording();
            Close();
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            buttonAccept.Visibility = Visibility.Hidden;
            buttonCancel.Visibility = Visibility.Visible;
            buttonDecline.Visibility = Visibility.Hidden;

            AudioCallback audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();

            audioService.Subscribe(Receiver);

            wi = new WaveIn();
            wi.WaveFormat = new WaveFormat(8000, 1);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            wi.StartRecording();

            textBlockInfo.Text = "Audio Call with " + Sender;
        }

        private void CallingWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Type)
            {
                Init();

                buttonAccept.Visibility = Visibility.Hidden;
                Title = "";
                textBlockInfo.Text = "Calling " + Receiver + "...";
            }
            else
            {
                buttonCancel.Visibility = Visibility.Hidden;

                Title = "";
                textBlockInfo.Text = Sender + " is calling you.";
            }
        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            AudioCallback audioCallback = new AudioCallback();
            DuplexChannelFactory<IAudio> channelAudioService = new DuplexChannelFactory<IAudio>(audioCallback, new NetTcpBinding(), new EndpointAddress("net.tcp://192.168.0.100:4444/AudioService"));
            audioService = channelAudioService.CreateChannel();

            audioService.Confirmation(Sender, Receiver, false);
            Close();
        }
    }
}

﻿using System;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IConnection connectionService;
        //CommunicationServiceCallback callback;

        BufferedWaveProvider bwp;

        public Login()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            //callback = new CommunicationServiceCallback();
            //DuplexChannelFactory<IConnection> channelServerService = new DuplexChannelFactory<IConnection>(callback, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4444/ConnectionService"));
            ChannelFactory<IConnection> channelServerService = new ChannelFactory<IConnection>(new NetTcpBinding(), new EndpointAddress("net.tcp://192.168.0.100:4444/ConnectionService"));
            connectionService = channelServerService.CreateChannel();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool ok = connectionService.Login(textBoxUsername.Text, passwordBox.Password);
            if (ok)
            {
                MainWindow mainWindow = new MainWindow(textBoxUsername.Text);
                mainWindow.Show();
                Close();
            }
            else
            {
                textBlockError.Text = "Username sau parola este gresita";
            }
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUpWindow = new SignUp(connectionService);
            signUpWindow.ShowDialog();
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            WaveIn wi = new WaveIn();
            WaveOut wo = new WaveOut();

            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);

            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;

            wo.Init(bwp);
            wi.StartRecording();
            wo.Play();
        }

        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

    }
}

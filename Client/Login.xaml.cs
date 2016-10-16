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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IConnection connectionService;

        BufferedWaveProvider bwp;

        public Login()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            ChannelFactory<IConnection> channelServerService = new ChannelFactory<IConnection>(new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://86.124.188.8:4444/ConnectionService"));
            connectionService = channelServerService.CreateChannel();

            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool ok = false;

            try
            {
                ok = connectionService.Login(textBoxUsername.Text, passwordBox.Password);
            }
            catch
            {
                textBlockError.Text = "Nu s-a putut conecta la server. Incercati din nou.";
                return;
            }


            if (ok)
            {
                ClientInformation.Username = textBoxUsername.Text;
                MainWindow mainWindow = new MainWindow();
                ClientInformation.MainWindow = mainWindow;
                ClientInformation.MainWindow.Show();
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

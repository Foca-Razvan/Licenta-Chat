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

        public Login()
        {
            InitializeComponent();
            Init();
            ResizeMode = ResizeMode.CanMinimize;
        }

        public void Init()
        {
            NetTcpBinding tcp = new NetTcpBinding(SecurityMode.None);
            tcp.SendTimeout = new TimeSpan(10, 0, 0);

            ChannelFactory<IConnection> channelServerService = new ChannelFactory<IConnection>(tcp,
                new EndpointAddress("net.tcp://"+ ClientInformation.IPAdressServer + ":4444/ConnectionService"));
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
                textBlockError.Text = "Couldn't connect to the server";
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
                textBlockError.Text = "Wrong password or username";
            }
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUpWindow = new SignUp(connectionService);
            signUpWindow.ShowDialog();
        }

    }
}

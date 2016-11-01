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
using Interfaces;

namespace Client
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {

        IConnection connectionService;
        public SignUp()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
        }

        public SignUp(IConnection connectionService)
        {
            InitializeComponent();
            this.connectionService = connectionService;
            ResizeMode = ResizeMode.CanMinimize;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBoxFirst.Password != passwordBoxRetype.Password)
            {
                textBlockError.Text = "The Passwords doesn't match.";
                return;
            }
            if (passwordBoxFirst.Password.Length < 5)
            {
                textBlockError.Text = "The password is too short";
                return;
            }

            int answer = connectionService.SignUp(textBoxUsername.Text, passwordBoxFirst.Password, textBoxEmail.Text);
            switch (answer)
            {
                case 0:
                    Close();
                    break;
                case 1 :
                    textBlockError.Text = "The username already exists.";
                    break;
                case 2:
                    textBlockError.Text = "The email is already used by another user.";
                    break;
                case 3:
                    textBlockError.Text = "An error was occured when tried to setup your account.Please try again";
                    break;
            }
           
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

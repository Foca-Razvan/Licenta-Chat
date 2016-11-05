using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Client.Windows;

namespace Client
{
    public class ScreenShareCallback : IScreenShareCallback
    {
        public void ShareScrennNotification(string from,string connectionString)
        {
            ScrenShareForm screenShareForm = new ScrenShareForm(from,connectionString);
            screenShareForm.Show();
        }

        public void SendFriendNotification(string username)
        {
            FriendNotificationWindow window = new FriendNotificationWindow(username);
            window.Show();
        }

        public void SendProfileInformation(string password , string email,byte[] image)
        {
            ClientInformation.Password = password;
            ClientInformation.Email = email;
            ClientInformation.Image = image;
        }
    }
}

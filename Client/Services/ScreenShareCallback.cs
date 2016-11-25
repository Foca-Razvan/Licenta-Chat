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
            ClientInformation.ShareScreenWindows.Add(from,screenShareForm);
            screenShareForm.Show();
        }

        public void SendFriendNotification(string username,byte[] image)
        {
            FriendNotificationWindow window = new FriendNotificationWindow(username ,image);
            window.Show();
        }

        public void SendProfileInformation(string password , string email,byte[] image)
        {
            ClientInformation.Password = password;
            ClientInformation.Email = email;
            ClientInformation.Image = image;
        }

        public void SendRefuseNotification(string sender)
        {
            ShareScreenEnding window;
            ClientInformation.ShareScreenEndingWindows.TryGetValue(sender,out window);
            window.DeclineRequest();
        }
    }
}

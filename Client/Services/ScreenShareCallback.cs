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
            ScrenShareForm screenShareForm = new ScrenShareForm(from,connectionString, false);
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
            if(window != null)
                window.DeclineRequest();
        }

        public void UserLeftShareScreenGroup(string sender,string groupName)
        {
            ClientInformation.ShareScreenEndingWindows.Remove(sender);

            GroupConversationWindow group;
            ClientInformation.GroupConversationWindows.TryGetValue(groupName, out group);
            if (group != null)
                group.UserLeftShareScreen(sender);

        }

        public void UserRefusedShareScreenGroup(string sender, string groupName)
        {
         
            if(ClientInformation.ShareScreenEndingWindows.ToList().Exists(x=> x.Key == sender))
            {
                GroupConversationWindow group;
                ClientInformation.GroupConversationWindows.TryGetValue(groupName, out group);

                if (group != null)
                    group.UserRefusedSharreScreenGroup(sender);
            }

            ClientInformation.ShareScreenEndingWindows.Remove(sender);
        }

        public void EndShareScreen(string sender)
        {
            ScrenShareForm form;
            ClientInformation.ShareScreenWindows.TryGetValue(sender,out form);
            if (form != null)
                form.Disconnect();
        }

        public void GroupShareScreenNotification(string sender,string groupName,string connectionString)
        {
            ScrenShareForm screenShareForm = new ScrenShareForm(groupName, connectionString,true);
            ClientInformation.ShareScreenWindows.Add(sender, screenShareForm);
            ClientInformation.ShareScreenWindows.Add(groupName, screenShareForm);
            screenShareForm.Show();
        }


    }
}

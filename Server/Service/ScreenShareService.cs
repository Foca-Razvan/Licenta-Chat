using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.ServiceModel;

namespace Server
{
    public class ScreenShareService : IScreenShare
    {
        /// <summary>
        /// Subscribes the screen share callback.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Subscribe(string username)
        {
            IScreenShareCallback callback = OperationContext.Current.GetCallbackChannel<IScreenShareCallback>();
            foreach (var client in Subscriber.subscribers)
                if (client.Username == username)
                {
                    client.ScreenShareCallback = callback;
                    return true;
                }
            return false;
        }


        /// <summary>
        /// Initialiaze a share screen with the partner.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="partner"></param>
        /// <param name="connectionString"></param>
        public void InitShareScreen(string client, string partner, string connectionString)
        {
            UserInformation user = Subscriber.getUser(partner);
            if (user != null && user.ScreenShareCallback != null)
                user.ScreenShareCallback.ShareScrennNotification(client, connectionString);
        }

        public void RefuseShareScreen(string sender, string partner)
        {
            UserInformation Partner = Subscriber.getUser(partner);
            if (Partner != null && Partner.ScreenShareCallback != null)
                Partner.ScreenShareCallback.SendRefuseNotification(sender);

        }

        public void RefuseGroupShareScreen(string sender, string groupName)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            if (group != null)
                group.UserRefusedShareSCreen(sender);
        }

        public void EndShareScreen(string sender,string receiver)
        {
            UserInformation user = Subscriber.getUser(receiver);
            if(user != null && user.ScreenShareCallback != null)
                user.ScreenShareCallback.EndShareScreen(sender);
        }

        public void InitShareScreenGroup(string sender,string groupName,string connectionString)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            foreach(UserInformation user in group.Members)
                if(user.Username != sender )
                    user.ScreenShareCallback.GroupShareScreenNotification(sender,group.GroupName, connectionString);
        }

        public void GroupEndShareScreen(string sender, string groupName)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            foreach (UserInformation user in group.Members)
                if (user.Username != sender)
                    user.ScreenShareCallback.EndShareScreen(groupName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Server
{
    public static class Subscriber
    {
        public static List<UserInformation> subscribers = new List<UserInformation>();
        public static List<GroupConversation> GroupConversations = new List<GroupConversation>();

        public static void Subscribe(IClientCallback client,string username)
        {
            UserInformation user = new UserInformation(client,username);
            if (!subscribers.Exists(x => x.CommunicationCallback == client))
                subscribers.Add(user);
        }

        public static void Unsubscribe(IClientCallback client)
        {
            subscribers.RemoveAll(x => x.CommunicationCallback == client);
        }

        public static UserInformation getUser(string username)
        {
            UserInformation user = subscribers.Find(x => x.Username == username);
            return user;
        }

        public static GroupConversation GetGroup(string groupName)
        {
            return GroupConversations.Find(x => x.GroupName == groupName);
        }
    }
}

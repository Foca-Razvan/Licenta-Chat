using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GroupConversation
    {
        public string Creator { get; set;}
        public string GroupName { get; set;}
        public List<UserInformation> Members = new List<UserInformation>();

        public GroupConversation(string creator,string groupName)
        {
            Creator = creator;
            GroupName = groupName;
            UserInformation user = Subscriber.getUser(Creator);
            Members.Add(user);
        }

        public void UserJoined(string username)
        {
            foreach (UserInformation member in Members)
                if(member.Username != username)
                    member.CommunicationCallback.UserJoinedGroup(username, GroupName);

            UserInformation user = Subscriber.getUser(username);
            Members.Add(user);
        }

        public void UserLeft(string username)
        {
            foreach (UserInformation member in Members)
                if (member.Username != username)
                    member.CommunicationCallback.UserLeft(username, GroupName);

            UserInformation user = Subscriber.getUser(username);
            Members.Remove(user);
        }

        public void UserRefusedShareSCreen(string sender)
        {
            foreach(UserInformation member in Members)
                member.ScreenShareCallback.UserRefusedShareScreenGroup(sender,GroupName);
        }

        public void UserDeclined(string username)
        {
            foreach (UserInformation member in Members)
                if (member.Username != username)
                    member.CommunicationCallback.UserRefusedGroup(username,GroupName);
        }

        public void SendMessage(string from,string message)
        {
            foreach (UserInformation user in Members)
                if(user.Username != from)
                    user.CommunicationCallback.SendGroup(from, GroupName, message);
        }

        public bool UserExits(string username)
        {
            if (Members.Exists(x => x.Username == username))
                return true;

            return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Interfaces;

namespace Server
{
    public class AudioService : IAudio
    {
        public void Subscribe(string username)
        {
            IAudioCallback callback = OperationContext.Current.GetCallbackChannel<IAudioCallback>();

            if (Subscriber.subscribers.Exists(x => x.Username == username))
            {
                foreach (var client in Subscriber.subscribers)
                    if (client.Username == username)
                        client.AudioCallback = callback;
            }
            else
            {
                UserInformation user = new UserInformation();
                user.Username = username;
                user.AudioCallback = callback;
                Subscriber.subscribers.Add(user);
            }
        }

        public void Unsubscribe(string username)
        {
            UserInformation user = Subscriber.getUser(username);
            user.AudioCallback = null;
        }

        public void InitCommunication(string sender ,string conversationPartner)
        {
            UserInformation receiver = Subscriber.subscribers.Find(x => x.Username == conversationPartner);
            if(receiver != null && receiver.CommunicationCallback != null)
                receiver.CommunicationCallback.SendAudioNotification(sender,false);
        }

        public void SendVoice(byte[] voice,int byteRecorded,string conversationPartner)
        {
            UserInformation user = Subscriber.subscribers.Find(x => x.Username == conversationPartner);
            if (user != null && user.AudioCallback != null)
                user.AudioCallback.SendVoiceCallback(voice,byteRecorded);
        }

        public void Confirmation(string sender , string receiver, bool ok,bool isGroup)
        {
            if (isGroup)
            {
                GroupConversation group = Subscriber.GetGroup(receiver);
                if (ok)
                    group.UserJoinAudio(sender);
            }
            else
            {
                UserInformation user = Subscriber.getUser(receiver);
                if (user != null && user.AudioCallback != null)
                    if (ok)
                        user.AudioCallback.ChannelAccepted(sender);
                    else
                        user.AudioCallback.ChannelDeclined(sender);
            }
        }

        public void StopCall(string sender,string receiver,bool isGroup)
        {
            if (isGroup)
            {
                GroupConversation group = Subscriber.GetGroup(receiver);
                group.UserLeftAudio(sender);
            }
            else
            {
                UserInformation user = Subscriber.getUser(receiver);
                if (user != null && user.AudioCallback != null)
                    user.AudioCallback.StopCall(sender);
            }
        }

        public void InitCommunicationGroup(string sender, string groupName)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            group.InviteMembersToAudioCall(sender);
        }

        public void SendVoiceGroup(byte[] voice, int byteRecorded, string groupName, string sender)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            if(group != null)
                group.SendVoiceAudio(sender, voice, byteRecorded);
        }
    }
}

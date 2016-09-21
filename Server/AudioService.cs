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

        public void InitCommunication(string username ,string conversationPartner)
        {
            UserInformation receiver = Subscriber.subscribers.Find(x => x.Username == conversationPartner);
            receiver.CommunicationCallback.SendAudioNotification(username, conversationPartner);
        }

        public void SendVoice(byte[] voice,int byteRecorded,string conversationPartner)
        {
            UserInformation user = Subscriber.subscribers.Find(x => x.Username == conversationPartner);
            user.AudioCallback.SendVoiceCallback(voice,byteRecorded);
        }

        public void Confirmation(string sender , string receiver,bool ok)
        {
            UserInformation user = Subscriber.getUser(sender);
            if (ok)
                user.AudioCallback.ChannelAccepted(receiver);
            else
                user.AudioCallback.ChannelDeclined(receiver);
        }

        public void StopCall(string sender,string receiver)
        {
            UserInformation user = Subscriber.getUser(receiver);
            user.AudioCallback.StopCall(sender);
            Unsubscribe(sender);
            Unsubscribe(receiver);
        }
    }
}

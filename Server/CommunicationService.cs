using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.ServiceModel;


namespace Server
{
    public class CommunicationService : ICommunication
    {
        public void Subscribe(string username)
        {
            IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            Subscriber.Subscribe(callback,username);
        }

        public bool Logout()
        {
            try
            {
                IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                UserInformation user = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);

                Subscriber.Unsubscribe(callback);

                foreach (UserInformation client in Subscriber.subscribers)
                    client.CommunicationCallback.UpdateListOfContacts(user.Username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SendMessage(string message,string to)
        {
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation information = Subscriber.subscribers.Find(x => x.CommunicationCallback == clientCallback);

            foreach (UserInformation user in Subscriber.subscribers)
                if (user.Username == to)
                    user.CommunicationCallback.Send(message, information.Username);
        }

        public List<string> GetListOfContacts()
        {
            List<string> contacts = new List<string>();
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            foreach (UserInformation client in Subscriber.subscribers)
                if(client.CommunicationCallback != clientCallback)
                    contacts.Add(client.Username);

            return contacts;
        }

        public void InitAudio(string username)
        {
            UserInformation user = Subscriber.getUser(username);
            //user.CommunicationCallback.s
        }

    }
}

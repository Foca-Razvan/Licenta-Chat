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
            Console.WriteLine(username + "!");
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
        public void InitShareScreen(string client , string partner,string connectionString)
        {
            UserInformation user = Subscriber.getUser(partner);
            if (user != null && user.ScreenShareCallback != null)
                user.ScreenShareCallback.ShareScrennNotification(client,connectionString);
        }
    }
}

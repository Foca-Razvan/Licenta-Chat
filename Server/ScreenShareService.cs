﻿using System;
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

        public void Subscribe(string username)
        {
            IScreenShareCallback callback = OperationContext.Current.GetCallbackChannel<IScreenShareCallback>();
            foreach (var client in Subscriber.subscribers)
                if (client.Username == username)
                    client.ScreenShareCallback = callback;
        }

        public void InitShareScreen(string client , string partner,string connectionString)
        {
            UserInformation user = Subscriber.getUser(partner);
            if (user != null && user.ScreenShareCallback != null)
                user.ScreenShareCallback.ShareScrennNotification(client,connectionString);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Server
{
    public class ScreenShareService : IScreenShare
    {
        public void InitShareScreen(string client , string partner,string connectionString)
        {
            UserInformation user = Subscriber.getUser(partner);
            if (user != null && user.ScreenShareCallback != null)
                user.ScreenShareCallback.ShareScrennNotification(client,connectionString);
        }
    }
}

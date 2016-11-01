using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using DataBase;

namespace Server
{
    public class UserInformation
    {
        public IClientCallback CommunicationCallback { get; set; }
        public IAudioCallback AudioCallback { get; set; }
        public IScreenShareCallback ScreenShareCallback { get; set; }
        public string Username { get; set; }

        public UserInformation() { }

        public UserInformation(IClientCallback callback, string username)
        {
            CommunicationCallback = callback;
            Username = username;
        }

        public UserInformation(IClientCallback callback)
        {
            CommunicationCallback = callback;
        }

        public bool IsFriendWith(string friendUsername)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == Username);
                User friend = context.Users.ToList().Find(x => x.Username == friendUsername);

                History history = user.Histories.ToList().Find(x => x.User == friend || x.User1 == friend);
                History history1 = user.Histories1.ToList().Find(x => x.User == friend || x.User1 == friend);

                if(history != null || history1 != null)                
                    return true;            
                return false;
            }
        }
    }
}

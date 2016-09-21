using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Server
{
    public class UserInformation
    {
        public IClientCallback CommunicationCallback { get; set; }
        public IAudioCallback AudioCallback { get; set; }
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
    }
}

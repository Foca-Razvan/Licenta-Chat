using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using DataBase;
using System.ServiceModel;

namespace Server
{
    public class ConnectionService : IConnection
    {

        public bool Login(string username, string password)
        {
            if (Subscriber.subscribers.Exists(x => x.Username == username))
                return false;

            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username && x.Password == password);
                if (user != null)
                {
                    user.Status = 1;
                    foreach (var client in Subscriber.subscribers)
                    {
                        if (client.IsFriendWith(username))
                            if (user.UserAvatar != null)
                                client.CommunicationCallback.SendNotification(username, user.UserAvatar.Image);
                            else
                                client.CommunicationCallback.SendNotification(username, null);
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            return false;        
        }

        public int SignUp(string username, string password , string email)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                if (context.Users.ToList().Exists(x => x.Username == username))                   
                    return 1;
                if (context.Users.ToList().Exists(x => x.Email == email))
                    return 2;
                User user = new User();
                user.Username = username;
                user.Password = password;
                user.Email = email;
                user.Status = 0;

                try
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return 0;
                }
                catch
                {
                    return 3;
                }                           
            }           
        }
    }
}

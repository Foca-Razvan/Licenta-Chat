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
                foreach (User user in context.Users)
                    if (user.Username == username && user.Password == password)
                    {
                        foreach (UserInformation c in Subscriber.subscribers)
                            c.CommunicationCallback.SendNotification(username);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.ServiceModel;
using DataBase;


namespace Server
{
    public class CommunicationService : ICommunication
    {
        public bool Subscribe(string username)
        {
            try
            {
                IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                Subscriber.Subscribe(callback, username);
                return true;
            }
            catch
            {
                return false;
            }          
        }

        public bool Logout()
        {
            try
            {
                IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                UserInformation user = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);

                Subscriber.Unsubscribe(callback);

                foreach(UserInformation client in Subscriber.subscribers)
                {
                    if (client.IsFriendWith(user.Username) && user.CommunicationCallback != client.CommunicationCallback)
                        client.CommunicationCallback.UpdateListOfContacts(user.Username);
                }
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

        public List<string> GetFriendList()
        {
            List<string> contacts = new List<string>();
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation client = Subscriber.subscribers.Find(x => x.CommunicationCallback == clientCallback);

            foreach(UserInformation user in Subscriber.subscribers)
            {
                if(user != client && user.IsFriendWith(client.Username))
                    contacts.Add(user.Username);
            }
            
                    

            return contacts;
        }

        public void AddFriend(string username)
        {
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username);
                UserInformation clientRequest = Subscriber.subscribers.ToList().Find(x => x.CommunicationCallback == clientCallback);
                User sender = context.Users.ToList().Find(x => x.Username == clientRequest.Username);

                Request request = new Request();
                request.FromUserId = sender.IdUser;
                request.FromUsername = sender.Username;
                request.User = user;

                user.Requests.Add(request);

                context.SaveChanges();
            }

        }

        public bool CheckUserExistance(string username)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                if (context.Users.ToList().Exists(x => x.Username == username))
                    return true;
                return false;
            }
        }

        public bool IsFriendWith(string sender , string friend)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User Sender = context.Users.ToList().Find(x => x.Username == sender);
                User Friend = context.Users.ToList().Find(x => x.Username == friend);

                bool exist = context.Histories.ToList().Exists(x => (x.User == Sender && x.User1 == Friend) || (x.User == Friend && x.User1 == Sender));

                return exist;
            }
        }

        public void AcceptFriendRequest(string username)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                Request request = context.Requests.ToList().Find(x => x.FromUsername == username);
                User sender = context.Users.ToList().Find(x => x.IdUser == request.FromUserId);
                User receiver = context.Users.ToList().Find(x => x.IdUser == request.User.IdUser);

                History history = new History();
                history.User = sender;
                history.User1 = receiver;
                history.Conversation = "";

                sender.Histories.Add(history);
                receiver.Histories1.Add(history);

                context.Requests.Remove(request);
                context.SaveChanges();
            }
        }

        public void DeclineFriendRequest(string username)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                Request request = context.Requests.ToList().Find(x => x.FromUsername == username);
                context.Requests.Remove(request);
                context.SaveChanges();
            }
        }

        public void GetNotifications()
        {
            IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation user = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);
            using (DataBaseContainer context = new DataBaseContainer())
            {
                foreach (Request request in context.Requests)
                    if (request.User.Username == user.Username)
                        user.ScreenShareCallback.SendFriendNotification(request.FromUsername);
            }
        }

        public void GetInformation(string username)
        {
            IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation client = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username);
                if (user != null)
                    if (user.UserAvatar != null)
                        client.ScreenShareCallback.SendProfileInformation(user.Password, user.Email, user.UserAvatar.Image);
                    else
                        client.ScreenShareCallback.SendProfileInformation(user.Password, user.Email, null);
                
            }
        }


        public void UpdateProfile(string username,string email,string password,byte[] image)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username);
                user.Email = email;
                user.Password = password;
                UserAvatar avatar = new UserAvatar();
                avatar.Image = image;
                user.UserAvatar = avatar;
                context.SaveChanges();
            }
        }

        public byte[] GetAvatarImage(string username)
        {
            byte[] image;
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username);
                if (user.UserAvatar != null)
                {
                    image = user.UserAvatar.Image;
                    return image;
                }
                return null;
            }
        }
    }
}

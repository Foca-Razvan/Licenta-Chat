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
    /// <summary>
    /// Subscribes the client to the server so that the server can acces the callback.
    /// </summary>
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
        /// <summary>
        /// Removes the user from the active users in the server and updates the friend list with users who are friends with him.
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            try
            {
                IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
                UserInformation user = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);

                Subscriber.Unsubscribe(callback);

                foreach (UserInformation client in Subscriber.subscribers)
                {
                    if (client.IsFriendWith(user.Username) && user.CommunicationCallback != client.CommunicationCallback)
                        client.CommunicationCallback.UpdateListOfContacts(user.Username);
                }

                using (DataBaseContainer context = new DataBaseContainer())
                {
                    User client = context.Users.ToList().Find(x => x.Username == user.Username);
                    client.Status = 0;
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// text message 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        public void SendMessage(string message, string to)
        {
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation information = Subscriber.subscribers.Find(x => x.CommunicationCallback == clientCallback);

            foreach (UserInformation user in Subscriber.subscribers)
                if (user.Username == to)
                    user.CommunicationCallback.Send(message, information.Username);
        }

        /// <summary>
        /// Returns a dictionary with the name as key and 1 or 0 as status. 1 = online 0 = offline
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetFriendList()
        {
            Dictionary<string, int> contacts = new Dictionary<string, int>();
            IClientCallback clientCallback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation client = Subscriber.subscribers.Find(x => x.CommunicationCallback == clientCallback);

            using (DataBaseContainer context = new DataBaseContainer())
            {
                foreach (User u in context.Users)
                    if (client.IsFriendWith(u.Username) && u.Username != client.Username)
                        contacts.Add(u.Username, (int)u.Status);
            }
            return contacts;
        }

        /// <summary>
        /// Creates a friend request.
        /// </summary>
        /// <param name="username">The username of the friend</param>
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

                UserInformation _user = Subscriber.subscribers.Find(x => x.Username == user.Username);
                if (_user != null)
                    _user.ScreenShareCallback.SendFriendNotification(sender.Username, sender.UserAvatar.Image);
            }



        }
        /// <summary>
        /// Checks if the user exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUserExistance(string username)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                if (context.Users.ToList().Exists(x => x.Username == username))
                    return true;
                return false;
            }
        }


        /// <summary>
        /// Check if the two user are friends.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="friend"></param>
        /// <returns></returns>
        public bool IsFriendWith(string sender, string friend)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User Sender = context.Users.ToList().Find(x => x.Username == sender);
                User Friend = context.Users.ToList().Find(x => x.Username == friend);

                bool exist = context.Histories.ToList().Exists(x => (x.User == Sender && x.User1 == Friend) || (x.User == Friend && x.User1 == Sender));

                return exist;
            }
        }
        /// <summary>
        /// Accepts friend request and updates the friend list.
        /// </summary>
        /// <param name="username"></param>
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


                UserInformation _sender = Subscriber.subscribers.Find(x => x.Username == sender.Username);
                UserInformation _receiver = Subscriber.subscribers.Find(x => x.Username == receiver.Username);

                bool statusSender = false, statusReceiver = false;
                if (receiver.Status == 1)
                    statusReceiver = true;
                if (sender.Status == 1)
                    statusSender = true;

                if (_sender != null)
                    _sender.CommunicationCallback.AddFriendInFriendList(receiver.Username, receiver.UserAvatar.Image, statusReceiver);
                if (_receiver != null)
                    _receiver.CommunicationCallback.AddFriendInFriendList(sender.Username, sender.UserAvatar.Image, statusSender);

            }


        }
        /// <summary>
        /// Decline friend request.
        /// </summary>
        /// <param name="username"></param>
        public void DeclineFriendRequest(string username)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                Request request = context.Requests.ToList().Find(x => x.FromUsername == username);
                context.Requests.Remove(request);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Gets friend notification from the server.
        /// </summary>
        public void GetNotifications()
        {
            IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            UserInformation user = Subscriber.subscribers.Find(x => x.CommunicationCallback == callback);
            using (DataBaseContainer context = new DataBaseContainer())
            {
                foreach (Request request in context.Requests)
                    if (request.User.Username == user.Username)
                    {
                        User friend = context.Users.ToList().Find(x => x.Username == request.FromUsername);
                        user.ScreenShareCallback.SendFriendNotification(request.FromUsername, friend.UserAvatar.Image);
                    }
            }
        }


        /// <summary>
        /// Returns profile information.
        /// </summary>
        /// <param name="username"></param>
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

        /// <summary>
        /// Updates the profile information in the data base.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="image"></param>
        public void UpdateProfile(string username, string email, string password, byte[] image)
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
        /// <summary>
        /// Returs an array of bytes[] of the image of the "username".
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public byte[] GetAvatarImage(string username)
        {
            byte[] image;
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User user = context.Users.ToList().Find(x => x.Username == username);
                if (user != null && user.UserAvatar != null)
                {
                    image = user.UserAvatar.Image;
                    return image;
                }
                return null;
            }
        }
        /// <summary>
        /// Removes the connection between the two user of frienship and updates the friend list of the both users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        public void RemoveFriend(string sender, string receiver)
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                User Sender = context.Users.ToList().Find(x => x.Username == sender);
                User Receiver = context.Users.ToList().Find(x => x.Username == receiver);

                History history = context.Histories.ToList().Find(x => x.User == Sender && x.User1 == Receiver || x.User == Receiver && x.User1 == Sender);

                Sender.Histories.Remove(history);
                Sender.Histories1.Remove(history);
                Receiver.Histories.Remove(history);
                Receiver.Histories1.Remove(history);

                context.Histories.Remove(history);
                context.SaveChanges();
            }

            UserInformation _Sender = Subscriber.getUser(sender);
            UserInformation _Receiver = Subscriber.getUser(receiver);

            if (_Sender != null)
                _Sender.CommunicationCallback.FriendRemoved(receiver);
            if (_Receiver != null)
                _Receiver.CommunicationCallback.FriendRemoved(sender);

        }

        public void InviteToGroupConversation(string sender, string receiver, string groupName)
        {
            UserInformation user = Subscriber.getUser(receiver);
            if (user != null)
                user.CommunicationCallback.SendGroupConversationNotification(sender, groupName);
        }

        public bool AcceptGroupRequest(string sender, string groupName)
        {
            UserInformation user = Subscriber.getUser(sender);
            GroupConversation group = Subscriber.GetGroup(groupName);
            if (group != null)
            {
                if(!group.UserExits(sender))
                    group.UserJoined(sender);
                return true;
            }
            return false;
        }

        public void DeclineGroupRequest(string sender, string groupName)
        {
            UserInformation user = Subscriber.getUser(sender);
            GroupConversation group = Subscriber.GetGroup(groupName);
            if (group != null)
                group.UserDeclined(sender);
        }

        public void LeaveGroup(string sender , string groupName)
        {
            UserInformation user = Subscriber.getUser(sender);
            GroupConversation group = Subscriber.GetGroup(groupName);
            if (group != null)
            {
                group.UserLeft(sender);
                if (group.Members.Count == 0)
                    Subscriber.GroupConversations.Remove(group);
            }

        }
        public bool CreateGroupConversation(string creator,string groupName)
        {
            if(Subscriber.GroupConversations.Exists( x => x.Creator == creator && x.GroupName == groupName))
                return false;

            GroupConversation group = new GroupConversation(creator,groupName);
            Subscriber.GroupConversations.Add(group);
            return true;
        }

        public List<string> GetGroupMembers(string groupName)
        {
            List<string> list = new List<string>();
            GroupConversation group = Subscriber.GetGroup(groupName);
            foreach (UserInformation user in group.Members)
                list.Add(user.Username);
            return list;
        }

        public void SendGroupMessage(string sender,string groupName,string message)
        {
            GroupConversation group = Subscriber.GetGroup(groupName);
            if (group != null)
                group.SendMessage(sender, message);
        }
    }
}

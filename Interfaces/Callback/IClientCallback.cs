using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void Send(string meessage,string to);
        [OperationContract(IsOneWay = true)]
        void SendNotification(string username);
        [OperationContract(IsOneWay = true)]
        void UpdateListOfContacts(string username);
        [OperationContract(IsOneWay = true)]
        void SendAudioNotification(string username,bool isGroup);
        [OperationContract(IsOneWay = true)]
        void SendProfileInformation(string password, string email, byte[] image);
        [OperationContract(IsOneWay = true)]
        void AddFriendInFriendList(string username, byte[] image,bool status);
        [OperationContract(IsOneWay = true)]
        void FriendRemoved(string username);
        [OperationContract(IsOneWay = true)]
        void SendGroupConversationNotification(string sender, string groupName);
        [OperationContract(IsOneWay = true)]
        void UserJoinedGroup(string sender,string groupName);
        [OperationContract(IsOneWay = true)]
        void UserRefusedGroup(string sender, string groupName);
        [OperationContract(IsOneWay = true)]
        void UserLeft(string sender, string groupName);
        [OperationContract(IsOneWay = true)]
        void SendGroup(string sender, string groupName, string message);
    }
}

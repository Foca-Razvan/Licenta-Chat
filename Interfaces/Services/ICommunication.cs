using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract (CallbackContract = typeof(IClientCallback))]
    public interface ICommunication
    {
        [OperationContract]
        bool Logout();
        [OperationContract]
        bool Subscribe(string username);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string messagem,string to);
        [OperationContract]
        List<string> GetFriendList();
        [OperationContract(IsOneWay = true)]
        void AddFriend(string username);
        [OperationContract]
        bool CheckUserExistance(string username);
        [OperationContract(IsOneWay = true)]
        void AcceptFriendRequest(string username);
        [OperationContract(IsOneWay = true)]
        void DeclineFriendRequest(string username);
        [OperationContract(IsOneWay = true)]
        void GetNotifications();
        [OperationContract]
        bool IsFriendWith(string sender, string friend);
        [OperationContract]
        void GetInformation(string username);
        [OperationContract(IsOneWay = true)]
        void UpdateProfile(string username,string email, string password, byte[] image);
        [OperationContract]
        byte[] GetAvatarImage(string username);
    }
}

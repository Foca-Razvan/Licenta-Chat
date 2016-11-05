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
        void SendNotification(string username,byte[] image);
        [OperationContract(IsOneWay = true)]
        void UpdateListOfContacts(string username);
        [OperationContract(IsOneWay = true)]
        void SendAudioNotification(string username);
        [OperationContract(IsOneWay = true)]
        void SendProfileInformation(string password, string email, byte[] image);
    }
}

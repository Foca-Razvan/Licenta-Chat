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
        [OperationContract]
        void SendAudioNotification(string username,string conversationPartner);
    }
}

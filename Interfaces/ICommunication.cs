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
        [OperationContract(IsOneWay = true)]
        void Subscribe(string username);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string messagem,string to);
        [OperationContract]
        List<string> GetListOfContacts();
        [OperationContract]
        void InitAudio(string username);
    }
}

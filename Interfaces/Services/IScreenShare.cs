using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract (CallbackContract = typeof(IScreenShareCallback))]
    public interface IScreenShare
    {
        [OperationContract]
        bool Subscribe(string username);
        [OperationContract(IsOneWay = true)]
        void InitShareScreen(string client, string partner,string connectionString);
        [OperationContract(IsOneWay = true)]
        void RefuseShareScreen(string sender,string partner);
        [OperationContract(IsOneWay = true)]
        void EndShareScreen(string sender, string receiver);
        [OperationContract(IsOneWay = true)]
        void InitShareScreenGroup(string client, string Group, string connectionString);
        [OperationContract(IsOneWay = true)]
        void GroupEndShareScreen(string sender, string groupName);
        [OperationContract(IsOneWay = true)]
        void RefuseGroupShareScreen(string sender, string groupName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract(CallbackContract = typeof(IAudioCallback))]
    public interface IAudio
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(string username);
        [OperationContract(IsOneWay = true)]
        void Unsubscribe(string client);
        [OperationContract(IsOneWay = true)]
        void SendVoice(byte[] audio,int byteRecorded,string conversationPartner);
        [OperationContract(IsOneWay = true)]
        void InitCommunication(string username,string partner);
        [OperationContract(IsOneWay = true)]
        void Confirmation(string sender,string receiver,bool ok,bool isGroup);
        [OperationContract(IsOneWay = true)]
        void StopCall(string sender,string receiver,bool isGroup);
        [OperationContract(IsOneWay = true)]
        void InitCommunicationGroup(string sender, string groupName);
        [OperationContract(IsOneWay = true)]
        void SendVoiceGroup(byte[] audio, int byteRecorded, string groupName,string sender);
    }
}

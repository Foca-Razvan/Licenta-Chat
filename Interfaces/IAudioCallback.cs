using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract]
    public interface IAudioCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendVoiceCallback(byte[] audio, int byteRecored);
        [OperationContract(IsOneWay = true)]
        void ChannelAccepted(string receiver);
        [OperationContract(IsOneWay = true)]
        void ChannelDeclined(string receiver);
        [OperationContract(IsOneWay = true)]
        void StopCall(string receiver);
    }
}

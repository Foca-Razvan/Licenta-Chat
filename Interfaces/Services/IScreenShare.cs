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
        [OperationContract(IsOneWay = true)]
        void Subscribe(string username);
        [OperationContract(IsOneWay = true)]
        void InitShareScreen(string client, string partner,string connectionString);
    }
}

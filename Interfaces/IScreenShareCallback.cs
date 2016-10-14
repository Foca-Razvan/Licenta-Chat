using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace Interfaces
{
    [ServiceContract]
    public interface IScreenShareCallback
    {
        [OperationContract(IsOneWay = true)]
        void ShareScrennNotification(string from,string connectionString);
        [OperationContract(IsOneWay = true)]
        void SendScreenImage(string client, string partner, byte image);
    }
}

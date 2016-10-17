using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract]
    public interface IConnection
    {
        [OperationContract]
        bool Login(string username,string password);
        [OperationContract]
        int SignUp(string username, string password, string email);
    }
}

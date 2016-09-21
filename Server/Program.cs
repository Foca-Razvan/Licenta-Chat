using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Interfaces;
using DataBase;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceHost serviceHost = new ServiceHost(typeof(ConnectionService), new Uri("net.tcp://192.168.0.100:4444/ConnectionService"));
            serviceHost.AddServiceEndpoint(typeof(IConnection), new NetTcpBinding(), "");

            ServiceHost serviceCommunicationHost = new ServiceHost(typeof(CommunicationService), new Uri("net.tcp://192.168.0.100:4444/CommunicationService"));
            serviceCommunicationHost.AddServiceEndpoint(typeof(ICommunication), new NetTcpBinding(), "");

            ServiceHost serviceAudioHost = new ServiceHost(typeof(AudioService), new Uri("net.tcp://192.168.0.100:4444/AudioService"));
            serviceAudioHost.AddServiceEndpoint(typeof(IAudio), new NetTcpBinding(), "");

            serviceCommunicationHost.Open();
            serviceHost.Open();
            serviceAudioHost.Open();

            Console.WriteLine("Serverul a fost lansat.");
            Console.WriteLine("Apasati enter pentru a inchide serverul");

            using (DataBaseContainer context = new DataBaseContainer())
            {
                foreach(User user in context.Users)
                    Console.WriteLine(user.Username);
            }


            Console.ReadLine();

            serviceCommunicationHost.Close();
            serviceHost.Close();
            serviceAudioHost.Close();
        }
    }
}

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
            NetTcpBinding tcpConnection = new NetTcpBinding(SecurityMode.None);
            tcpConnection.ReceiveTimeout = new TimeSpan(10, 0, 0);
            serviceHost.AddServiceEndpoint(typeof(IConnection), tcpConnection, "");

            ServiceHost serviceCommunicationHost = new ServiceHost(typeof(CommunicationService), new Uri("net.tcp://192.168.0.100:4444/CommunicationService"));
            NetTcpBinding tcpCommunication = new NetTcpBinding(SecurityMode.None);
            tcpCommunication.MaxReceivedMessageSize = 20000000;
            tcpCommunication.ReceiveTimeout = new TimeSpan(10, 0, 0);
            serviceCommunicationHost.AddServiceEndpoint(typeof(ICommunication), tcpCommunication, "");

            ServiceHost serviceAudioHost = new ServiceHost(typeof(AudioService), new Uri("net.tcp://192.168.0.100:4444/AudioService"));
            NetTcpBinding tcpAudio = new NetTcpBinding(SecurityMode.None);
            tcpAudio.MaxReceivedMessageSize = 20000000;
            tcpAudio.ReceiveTimeout = new TimeSpan(10, 0, 0);
            serviceAudioHost.AddServiceEndpoint(typeof(IAudio), tcpAudio, "");

            ServiceHost serviceScreenShareHost = new ServiceHost(typeof(ScreenShareService), new Uri("net.tcp://192.168.0.100:4444/ScreenShareService"));
            NetTcpBinding tcpScreenShare = new NetTcpBinding(SecurityMode.None);
            tcpScreenShare.MaxReceivedMessageSize = 20000000;
            tcpScreenShare.ReceiveTimeout = new TimeSpan(10, 0, 0);
            serviceScreenShareHost.AddServiceEndpoint(typeof(IScreenShare), tcpScreenShare, "");

            serviceCommunicationHost.Open();
            serviceHost.Open();
            serviceAudioHost.Open();
            serviceScreenShareHost.Open();

            //test();

            Console.WriteLine("Serverul a fost lansat.");
            Console.WriteLine("Apasati enter pentru a inchide serverul");

            Console.ReadLine();

            serviceCommunicationHost.Close();
            serviceHost.Close();
            serviceScreenShareHost.Close();
            serviceAudioHost.Close();
        }


        private static void test()
        {
            using (DataBaseContainer context = new DataBaseContainer())
            {
                foreach(Request user in context.Requests)
                {
                    Console.WriteLine(user.FromUsername + " to " + user.User.Username);
                    if (user.User.Username == "Andrei")
                        context.Requests.Remove(user);
                }
                context.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Client
{
    static class ClientInformation
    {
        public static string IPAdressServer = "79.112.103.21";
        public static string Username { get; set; }
        public static Login Login { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static SignUp SignUpWindow { get; set; }
        public static Dictionary<string, CallingWindow> CallingWindows = new Dictionary<string, CallingWindow>();
        public static Dictionary<string, AnswerWindow> AnswerWindows = new Dictionary<string, AnswerWindow>();

        public static IConnection ConnectionService { get; set; }
        public static ICommunication CommunicationService { get; set; }
        public static IAudio AudioService { get; set; }
        public static IScreenShare ScreenShareService { get; set; }

        public static ScreenShareCallback scrrenShareCallback { get; set; }

    }
}

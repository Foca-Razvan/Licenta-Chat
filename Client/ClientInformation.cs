using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class ClientInformation
    {
        public static string Username { get; set; }
        public static Login Login { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static SignUp SignUpWindow { get; set; }
        public static Dictionary<string, CallingWindow> CallingWindows { get; set; }
        public static Dictionary<string, AnswerWindow> AnswerWindows { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Media.Imaging;

namespace Client
{
    static class ClientInformation
    {
        public static string IPAdressServer = "86.124.191.73";
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }
        public static byte[] Image { get; set; }
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



        public static BitmapImage ToImage(byte[] data)
        {
            if (data != null)
            {
                using (var ms = new System.IO.MemoryStream(data))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            return null;
        }
    }
}

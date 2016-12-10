using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Media.Imaging;
using Client.Windows;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace Client
{
    static class ClientInformation
    {
        public static string IPAdressServer = "86.124.154.205";
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }
        public static byte[] Image { get; set; }

        public static Login Login { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static SignUp SignUpWindow { get; set; }

        public static Dictionary<string, CallingWindow> CallingWindows = new Dictionary<string, CallingWindow>();
        public static Dictionary<string, AnswerWindow> AnswerWindows = new Dictionary<string, AnswerWindow>();
        public static Dictionary<string, ConversationWindow> ConversationsWindows = new Dictionary<string, ConversationWindow>();
        public static Dictionary<string, ScrenShareForm> ShareScreenWindows = new Dictionary<string, ScrenShareForm>();
        public static Dictionary<string, ShareScreenEnding> ShareScreenEndingWindows = new Dictionary<string, ShareScreenEnding>();
        public static Dictionary<string, GroupConversationWindow> GroupConversationWindows = new Dictionary<string, GroupConversationWindow>();

        public static IConnection ConnectionService { get; set; }
        public static ICommunication CommunicationService { get; set; }
        public static IAudio AudioService { get; set; }
        public static IScreenShare ScreenShareService { get; set; }

        public static ScreenShareCallback scrrenShareCallback { get; set; }

        public static int authNr = 1;
        public static int groupNr = 1;

        public static WaveFormat waveFormat = new WaveFormat(12000, 2);



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

        public static bool GetStatusFromFriendList(string username)
        {
            FriendData data = MainWindow.Friends.Items.ToList().Find(x => x.Username == username);
            if (data != null)
                return data.Status;            
            return false;
        }
        
        public static FriendData GetFriend(string username)
        {
            return MainWindow.Friends.Items.ToList().Find(x => x.Username == username);

        }
    }
}

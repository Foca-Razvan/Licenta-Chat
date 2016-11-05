using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.Data
{
    public class FriendData
    {
        public BitmapImage Image { get; set; }
        public string Username { get; set; }
        public BitmapImage Status { get; set; }

        public FriendData() { }

        public FriendData(string username, BitmapImage status, BitmapImage image)
        {
            Username = username;
            Status = status;
            Image = image;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Client
{
    public class FriendData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapImage _statusImage;

        public BitmapImage AvatarImage { get; set; }
        public string Username { get; set; }
        public BitmapImage StatusImage {
            get { return _statusImage; }
            set {
                _statusImage = value;
                OnPropertyChanged("StatusImage");
            }
        }
        public bool Status { get; set; }

        public FriendData() { }

        public FriendData(string username, BitmapImage statusImage, BitmapImage image,bool status)
        {
            Username = username;
            AvatarImage = statusImage;
            _statusImage = image;
            Status = status;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        } 
    }
}

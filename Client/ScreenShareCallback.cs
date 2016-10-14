using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Client
{
    public class ScreenShareCallback : IScreenShareCallback
    {
        public void ShareScrennNotification(string from,string connectionString)
        {
            ScrenShareForm screenShareForm = new ScrenShareForm(from,connectionString);
            screenShareForm.Show();
        }
    }
}

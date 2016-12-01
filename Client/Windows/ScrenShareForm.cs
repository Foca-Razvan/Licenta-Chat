using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ScrenShareForm : Form
    {

        private string ConnectionString { get; set; }
        private string Partner { get; set; }

        public ScrenShareForm(string from , string connectionString)
        {
            InitializeComponent();

            axRDPViewer1.Visible = false;
            label1.Text = from + " has offered to share his screen. Do you accept ?";

            ConnectionString = connectionString;
            Partner = from;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            buttonAccept.Visible = false;
            buttonDecline.Visible = false;
            label1.Visible = false;

            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;

            axRDPViewer1.Size =  new System.Drawing.Size(Width, Height);
            axRDPViewer1.Visible = true;

            axRDPViewer1.Connect(ConnectionString, Partner, "");
        }

        private void buttonDecline_Click(object sender, EventArgs e)
        {
            ClientInformation.ShareScreenWindows.Remove(Partner);
            ClientInformation.ScreenShareService.RefuseShareScreen(ClientInformation.Username, Partner);
            Close();
        }

        private void OnWindowClose(object sender, AxRDPCOMAPILib._IRDPSessionEvents_OnWindowCloseEvent e)
        {
            //ClientInformation.ScreenShareService.RefuseShareScreen(ClientInformation.Username, Partner);
            ClientInformation.ShareScreenWindows.Remove(Partner);
            axRDPViewer1.Disconnect();
        }

        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            ClientInformation.ScreenShareService.RefuseShareScreen(ClientInformation.Username, Partner);
            ClientInformation.ShareScreenWindows.Remove(Partner);
            axRDPViewer1.Disconnect();
        }
    }
}

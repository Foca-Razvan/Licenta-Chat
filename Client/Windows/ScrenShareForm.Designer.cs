namespace Client
{
    partial class ScrenShareForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrenShareForm));
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonDecline = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.axRDPViewer1 = new AxRDPCOMAPILib.AxRDPViewer();
            ((System.ComponentModel.ISupportInitialize)(this.axRDPViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(136, 80);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 0;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonDecline
            // 
            this.buttonDecline.Location = new System.Drawing.Point(318, 80);
            this.buttonDecline.Name = "buttonDecline";
            this.buttonDecline.Size = new System.Drawing.Size(75, 23);
            this.buttonDecline.TabIndex = 1;
            this.buttonDecline.Text = "Decline";
            this.buttonDecline.UseVisualStyleBackColor = true;
            this.buttonDecline.Click += new System.EventHandler(this.buttonDecline_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // axRDPViewer1
            // 
            this.axRDPViewer1.Enabled = true;
            this.axRDPViewer1.Location = new System.Drawing.Point(0, 0);
            this.axRDPViewer1.Name = "axRDPViewer1";
            this.axRDPViewer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axRDPViewer1.OcxState")));
            this.axRDPViewer1.Size = new System.Drawing.Size(192, 192);
            this.axRDPViewer1.TabIndex = 3;
            this.axRDPViewer1.OnWindowClose += new AxRDPCOMAPILib._IRDPSessionEvents_OnWindowCloseEventHandler(this.OnWindowClose);
            // 
            // ScrenShareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 129);
            this.Controls.Add(this.axRDPViewer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDecline);
            this.Controls.Add(this.buttonAccept);
            this.Name = "ScrenShareForm";
            this.Text = "ScrenShareForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClosingEvent);
            ((System.ComponentModel.ISupportInitialize)(this.axRDPViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonDecline;
        private System.Windows.Forms.Label label1;
        private AxRDPCOMAPILib.AxRDPViewer axRDPViewer1;
    }
}
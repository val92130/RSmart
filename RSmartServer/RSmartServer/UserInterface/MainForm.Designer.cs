namespace UserInterface
{
    partial class RSmartServer
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
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.routesTextBox = new System.Windows.Forms.RichTextBox();
            this.panelCamera = new System.Windows.Forms.Panel();
            this.pictureWebcam = new System.Windows.Forms.PictureBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelIp = new System.Windows.Forms.Label();
            this.labelIpTitle = new System.Windows.Forms.Label();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addARouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.labelUrl = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.pingButton = new System.Windows.Forms.Button();
            this.unsynchronizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWebcam)).BeginInit();
            this.panelTop.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(122)))), ((int)(((byte)(137)))));
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.Location = new System.Drawing.Point(12, 636);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(984, 82);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // routesTextBox
            // 
            this.routesTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.routesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.routesTextBox.Location = new System.Drawing.Point(700, 100);
            this.routesTextBox.Name = "routesTextBox";
            this.routesTextBox.ReadOnly = true;
            this.routesTextBox.Size = new System.Drawing.Size(290, 250);
            this.routesTextBox.TabIndex = 2;
            this.routesTextBox.Text = "";
            // 
            // panelCamera
            // 
            this.panelCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.panelCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCamera.Controls.Add(this.pictureWebcam);
            this.panelCamera.Location = new System.Drawing.Point(700, 368);
            this.panelCamera.Name = "panelCamera";
            this.panelCamera.Size = new System.Drawing.Size(290, 250);
            this.panelCamera.TabIndex = 3;
            // 
            // pictureWebcam
            // 
            this.pictureWebcam.Location = new System.Drawing.Point(-1, -1);
            this.pictureWebcam.Name = "pictureWebcam";
            this.pictureWebcam.Size = new System.Drawing.Size(290, 250);
            this.pictureWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureWebcam.TabIndex = 0;
            this.pictureWebcam.TabStop = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.labelIp);
            this.panelTop.Controls.Add(this.labelIpTitle);
            this.panelTop.Location = new System.Drawing.Point(12, 27);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(984, 39);
            this.panelTop.TabIndex = 4;
            // 
            // labelIp
            // 
            this.labelIp.AutoSize = true;
            this.labelIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIp.Location = new System.Drawing.Point(81, 10);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(0, 16);
            this.labelIp.TabIndex = 1;
            // 
            // labelIpTitle
            // 
            this.labelIpTitle.AutoSize = true;
            this.labelIpTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIpTitle.Location = new System.Drawing.Point(3, 10);
            this.labelIpTitle.Name = "labelIpTitle";
            this.labelIpTitle.Size = new System.Drawing.Size(72, 16);
            this.labelIpTitle.TabIndex = 0;
            this.labelIpTitle.Text = "Server IP : ";
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 100);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(672, 518);
            this.webBrowser.TabIndex = 5;
            this.webBrowser.Url = new System.Uri("http://localhost/", System.UriKind.Absolute);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronizeToolStripMenuItem,
            this.unsynchronizeToolStripMenuItem,
            this.pauseServerToolStripMenuItem,
            this.resumeServerToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // synchronizeToolStripMenuItem
            // 
            this.synchronizeToolStripMenuItem.Name = "synchronizeToolStripMenuItem";
            this.synchronizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.synchronizeToolStripMenuItem.Text = "Synchronize";
            this.synchronizeToolStripMenuItem.Click += new System.EventHandler(this.synchronizeToolStripMenuItem_Click);
            // 
            // pauseServerToolStripMenuItem
            // 
            this.pauseServerToolStripMenuItem.Name = "pauseServerToolStripMenuItem";
            this.pauseServerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pauseServerToolStripMenuItem.Text = "Pause Server";
            this.pauseServerToolStripMenuItem.Click += new System.EventHandler(this.pauseServerToolStripMenuItem_Click);
            // 
            // resumeServerToolStripMenuItem
            // 
            this.resumeServerToolStripMenuItem.Name = "resumeServerToolStripMenuItem";
            this.resumeServerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resumeServerToolStripMenuItem.Text = "Resume Server";
            this.resumeServerToolStripMenuItem.Click += new System.EventHandler(this.resumeServerToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addARouteToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // addARouteToolStripMenuItem
            // 
            this.addARouteToolStripMenuItem.Name = "addARouteToolStripMenuItem";
            this.addARouteToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.addARouteToolStripMenuItem.Text = "Add a route";
            this.addARouteToolStripMenuItem.Click += new System.EventHandler(this.addARouteToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(53, 74);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(180, 20);
            this.textBoxUrl.TabIndex = 7;
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.BackColor = System.Drawing.Color.Transparent;
            this.labelUrl.ForeColor = System.Drawing.Color.White;
            this.labelUrl.Location = new System.Drawing.Point(12, 77);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(26, 13);
            this.labelUrl.TabIndex = 8;
            this.labelUrl.Text = "Url :";
            // 
            // buttonGo
            // 
            this.buttonGo.BackColor = System.Drawing.Color.White;
            this.buttonGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGo.Location = new System.Drawing.Point(239, 74);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(36, 20);
            this.buttonGo.TabIndex = 9;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = false;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // pingButton
            // 
            this.pingButton.Location = new System.Drawing.Point(915, 71);
            this.pingButton.Name = "pingButton";
            this.pingButton.Size = new System.Drawing.Size(75, 23);
            this.pingButton.TabIndex = 10;
            this.pingButton.Text = "Ping Robot";
            this.pingButton.UseVisualStyleBackColor = true;
            this.pingButton.Click += new System.EventHandler(this.pingButton_Click);
            // 
            // unsynchronizeToolStripMenuItem
            // 
            this.unsynchronizeToolStripMenuItem.Name = "unsynchronizeToolStripMenuItem";
            this.unsynchronizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.unsynchronizeToolStripMenuItem.Text = "Unsynchronize";
            this.unsynchronizeToolStripMenuItem.Click += new System.EventHandler(this.unsynchronizeToolStripMenuItem_Click);
            // 
            // RSmartServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.pingButton);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelCamera);
            this.Controls.Add(this.routesTextBox);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RSmartServer";
            this.Text = "RSmart Server";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RSmartServer_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RSmartServer_KeyUp);
            this.panelCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureWebcam)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.RichTextBox routesTextBox;
        private System.Windows.Forms.Panel panelCamera;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelIpTitle;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.PictureBox pictureWebcam;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.ToolStripMenuItem pauseServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumeServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addARouteToolStripMenuItem;
        private System.Windows.Forms.Button pingButton;
        private System.Windows.Forms.ToolStripMenuItem unsynchronizeToolStripMenuItem;
    }
}


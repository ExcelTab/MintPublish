namespace Mint
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            Lbl_LoggedAs = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            changeAccountToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem_Disconnect = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            changeEmailSettingsToolStripMenuItem = new ToolStripMenuItem();
            Grp_Apps = new GroupBox();
            label1 = new Label();
            Grp_Forms = new GroupBox();
            pictureBox_AppLoading = new PictureBox();
            pictureBox1 = new PictureBox();
            Lbl_NotConnected = new Label();
            Btn_HomeConnect = new Button();
            Lbl_Version = new Label();
            label_status = new Label();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_AppLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton2 });
            toolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RightToLeft = RightToLeft.Yes;
            toolStrip1.Size = new Size(1284, 23);
            toolStrip1.TabIndex = 8;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { Lbl_LoggedAs, toolStripSeparator1, changeAccountToolStripMenuItem, ToolStripMenuItem_Disconnect, toolStripSeparator2, changeEmailSettingsToolStripMenuItem });
            toolStripDropDownButton2.Image = Properties.Resources.account;
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(29, 20);
            toolStripDropDownButton2.Text = "toolStripDropDownButton2";
            toolStripDropDownButton2.Click += toolStripDropDownButton2_Click;
            // 
            // Lbl_LoggedAs
            // 
            Lbl_LoggedAs.AutoSize = false;
            Lbl_LoggedAs.DisplayStyle = ToolStripItemDisplayStyle.Text;
            Lbl_LoggedAs.Name = "Lbl_LoggedAs";
            Lbl_LoggedAs.RightToLeft = RightToLeft.Yes;
            Lbl_LoggedAs.Size = new Size(180, 20);
            Lbl_LoggedAs.Text = "Not logged in";
            Lbl_LoggedAs.TextAlign = ContentAlignment.BottomCenter;
            Lbl_LoggedAs.TextImageRelation = TextImageRelation.Overlay;
            Lbl_LoggedAs.Click += Lbl_LoggedAs_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(188, 6);
            // 
            // changeAccountToolStripMenuItem
            // 
            changeAccountToolStripMenuItem.Name = "changeAccountToolStripMenuItem";
            changeAccountToolStripMenuItem.Size = new Size(191, 22);
            changeAccountToolStripMenuItem.Text = "Change account";
            changeAccountToolStripMenuItem.Click += Open_Login_Form;
            // 
            // ToolStripMenuItem_Disconnect
            // 
            ToolStripMenuItem_Disconnect.Enabled = false;
            ToolStripMenuItem_Disconnect.Name = "ToolStripMenuItem_Disconnect";
            ToolStripMenuItem_Disconnect.Size = new Size(191, 22);
            ToolStripMenuItem_Disconnect.Text = "Disconnect";
            ToolStripMenuItem_Disconnect.Click += ToolStripMenuItem_Disconnect_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(188, 6);
            // 
            // changeEmailSettingsToolStripMenuItem
            // 
            changeEmailSettingsToolStripMenuItem.Name = "changeEmailSettingsToolStripMenuItem";
            changeEmailSettingsToolStripMenuItem.Size = new Size(191, 22);
            changeEmailSettingsToolStripMenuItem.Text = "Change email settings";
            changeEmailSettingsToolStripMenuItem.Click += changeEmailSettingsToolStripMenuItem_Click;
            // 
            // Grp_Apps
            // 
            Grp_Apps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Grp_Apps.Location = new Point(0, 52);
            Grp_Apps.Name = "Grp_Apps";
            Grp_Apps.Size = new Size(200, 610);
            Grp_Apps.TabIndex = 10;
            Grp_Apps.TabStop = false;
            Grp_Apps.Text = "Applications";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Green;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(188, 25);
            label1.TabIndex = 11;
            label1.Text = "Mint ERP for Hexoa";
            // 
            // Grp_Forms
            // 
            Grp_Forms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Grp_Forms.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Grp_Forms.BackgroundImageLayout = ImageLayout.None;
            Grp_Forms.FlatStyle = FlatStyle.Flat;
            Grp_Forms.Location = new Point(217, 23);
            Grp_Forms.Margin = new Padding(0);
            Grp_Forms.Name = "Grp_Forms";
            Grp_Forms.Padding = new Padding(0);
            Grp_Forms.Size = new Size(1067, 639);
            Grp_Forms.TabIndex = 12;
            Grp_Forms.TabStop = false;
            Grp_Forms.Visible = false;
            // 
            // pictureBox_AppLoading
            // 
            pictureBox_AppLoading.Anchor = AnchorStyles.None;
            pictureBox_AppLoading.BorderStyle = BorderStyle.Fixed3D;
            pictureBox_AppLoading.Cursor = Cursors.AppStarting;
            pictureBox_AppLoading.Image = (Image)resources.GetObject("pictureBox_AppLoading.Image");
            pictureBox_AppLoading.Location = new Point(571, 216);
            pictureBox_AppLoading.Name = "pictureBox_AppLoading";
            pictureBox_AppLoading.Size = new Size(315, 200);
            pictureBox_AppLoading.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_AppLoading.TabIndex = 18;
            pictureBox_AppLoading.TabStop = false;
            pictureBox_AppLoading.Visible = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(217, 60);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1064, 599);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // Lbl_NotConnected
            // 
            Lbl_NotConnected.Anchor = AnchorStyles.Top;
            Lbl_NotConnected.BackColor = Color.Transparent;
            Lbl_NotConnected.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Lbl_NotConnected.ForeColor = Color.FromArgb(0, 64, 0);
            Lbl_NotConnected.Location = new Point(591, 229);
            Lbl_NotConnected.Name = "Lbl_NotConnected";
            Lbl_NotConnected.Size = new Size(305, 84);
            Lbl_NotConnected.TabIndex = 15;
            Lbl_NotConnected.Text = "Pour utilliser l'outil, commencez par vous connecter :";
            Lbl_NotConnected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Btn_HomeConnect
            // 
            Btn_HomeConnect.Anchor = AnchorStyles.Top;
            Btn_HomeConnect.BackColor = Color.RoyalBlue;
            Btn_HomeConnect.FlatStyle = FlatStyle.Popup;
            Btn_HomeConnect.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            Btn_HomeConnect.ForeColor = SystemColors.ButtonHighlight;
            Btn_HomeConnect.Location = new Point(669, 342);
            Btn_HomeConnect.Name = "Btn_HomeConnect";
            Btn_HomeConnect.Size = new Size(148, 57);
            Btn_HomeConnect.TabIndex = 16;
            Btn_HomeConnect.Text = "Se connecter";
            Btn_HomeConnect.UseVisualStyleBackColor = false;
            Btn_HomeConnect.Click += Open_Login_Form;
            // 
            // Lbl_Version
            // 
            Lbl_Version.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
            Lbl_Version.ForeColor = Color.FromArgb(128, 64, 0);
            Lbl_Version.Location = new Point(12, 36);
            Lbl_Version.Name = "Lbl_Version";
            Lbl_Version.Size = new Size(188, 13);
            Lbl_Version.TabIndex = 0;
            Lbl_Version.Text = "version alpha 0.99";
            Lbl_Version.TextAlign = ContentAlignment.MiddleRight;
            Lbl_Version.Click += Lbl_Version_Click;
            // 
            // label_status
            // 
            label_status.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_status.BackColor = Color.Yellow;
            label_status.BorderStyle = BorderStyle.FixedSingle;
            label_status.Location = new Point(0, 645);
            label_status.Name = "label_status";
            label_status.Size = new Size(1284, 23);
            label_status.TabIndex = 17;
            label_status.Text = "Mise à jour des données ...";
            label_status.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1284, 667);
            Controls.Add(pictureBox_AppLoading);
            Controls.Add(label_status);
            Controls.Add(Lbl_Version);
            Controls.Add(Grp_Forms);
            Controls.Add(Btn_HomeConnect);
            Controls.Add(Lbl_NotConnected);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(Grp_Apps);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(600, 500);
            Name = "MainForm";
            Text = "Mint";
            WindowState = FormWindowState.Maximized;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_AppLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem changeAccountToolStripMenuItem;
        private GroupBox Grp_Apps;
        private Label label1;
        private GroupBox Grp_Forms;
        private ToolStripMenuItem Lbl_LoggedAs;
        private PictureBox pictureBox1;
        private Label Lbl_NotConnected;
        private Button Btn_HomeConnect;
        private Label Lbl_Version;
        private ToolStripMenuItem ToolStripMenuItem_Disconnect;
        private Label label_status;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem changeEmailSettingsToolStripMenuItem;
        private PictureBox pictureBox_AppLoading;
    }
}
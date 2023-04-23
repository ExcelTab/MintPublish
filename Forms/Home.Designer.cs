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
            Grp_Apps = new GroupBox();
            label1 = new Label();
            Grp_Forms = new GroupBox();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            Lbl_NotConnected = new Label();
            Btn_HomeConnect = new Button();
            label3 = new Label();
            toolStrip1.SuspendLayout();
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
            toolStrip1.Size = new Size(1331, 23);
            toolStrip1.TabIndex = 8;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { Lbl_LoggedAs, toolStripSeparator1, changeAccountToolStripMenuItem, ToolStripMenuItem_Disconnect });
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
            toolStripSeparator1.Size = new Size(158, 6);
            // 
            // changeAccountToolStripMenuItem
            // 
            changeAccountToolStripMenuItem.Name = "changeAccountToolStripMenuItem";
            changeAccountToolStripMenuItem.Size = new Size(161, 22);
            changeAccountToolStripMenuItem.Text = "Change account";
            changeAccountToolStripMenuItem.Click += Open_Login_Form;
            // 
            // ToolStripMenuItem_Disconnect
            // 
            ToolStripMenuItem_Disconnect.Enabled = false;
            ToolStripMenuItem_Disconnect.Name = "ToolStripMenuItem_Disconnect";
            ToolStripMenuItem_Disconnect.Size = new Size(161, 22);
            ToolStripMenuItem_Disconnect.Text = "Disconnect";
            ToolStripMenuItem_Disconnect.Click += ToolStripMenuItem_Disconnect_Click;
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
            Grp_Forms.Location = new Point(217, 52);
            Grp_Forms.Margin = new Padding(0);
            Grp_Forms.Name = "Grp_Forms";
            Grp_Forms.Padding = new Padding(0);
            Grp_Forms.Size = new Size(1114, 610);
            Grp_Forms.TabIndex = 12;
            Grp_Forms.TabStop = false;
            Grp_Forms.Visible = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = Properties.Resources.BackGround_Blue;
            pictureBox1.Location = new Point(217, 60);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1111, 599);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(0, 64, 0);
            label2.Location = new Point(648, 92);
            label2.Name = "label2";
            label2.Size = new Size(237, 32);
            label2.TabIndex = 14;
            label2.Text = "Bienvenue sur Mint";
            // 
            // Lbl_NotConnected
            // 
            Lbl_NotConnected.Anchor = AnchorStyles.Top;
            Lbl_NotConnected.BackColor = Color.Transparent;
            Lbl_NotConnected.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Lbl_NotConnected.ForeColor = Color.FromArgb(0, 64, 0);
            Lbl_NotConnected.Location = new Point(614, 229);
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
            Btn_HomeConnect.Location = new Point(692, 342);
            Btn_HomeConnect.Name = "Btn_HomeConnect";
            Btn_HomeConnect.Size = new Size(148, 57);
            Btn_HomeConnect.TabIndex = 16;
            Btn_HomeConnect.Text = "Se connecter";
            Btn_HomeConnect.UseVisualStyleBackColor = false;
            Btn_HomeConnect.Click += Open_Login_Form;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(128, 64, 0);
            label3.Location = new Point(113, 34);
            label3.Name = "label3";
            label3.Size = new Size(87, 13);
            label3.TabIndex = 0;
            label3.Text = "version alpha 0.1";
            label3.Click += label3_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1331, 667);
            Controls.Add(label3);
            Controls.Add(Grp_Forms);
            Controls.Add(Btn_HomeConnect);
            Controls.Add(Lbl_NotConnected);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(Grp_Apps);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(600, 500);
            Name = "MainForm";
            Text = "Mint";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
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
        private Label label2;
        private Label Lbl_NotConnected;
        private Button Btn_HomeConnect;
        private Label label3;
        private ToolStripMenuItem ToolStripMenuItem_Disconnect;
    }
}
namespace Mint
{
    partial class EmailConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailConfig));
            BtnEmailOK = new Button();
            BtnEmailCancel = new Button();
            TB_EmailUser = new TextBox();
            TB_EmailPassword = new TextBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            TB_EmailSMTP = new TextBox();
            label4 = new Label();
            TB_EmailPort = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // BtnEmailOK
            // 
            BtnEmailOK.BackColor = Color.FromArgb(63, 81, 181);
            BtnEmailOK.FlatAppearance.BorderSize = 0;
            BtnEmailOK.FlatStyle = FlatStyle.Flat;
            BtnEmailOK.ForeColor = Color.White;
            BtnEmailOK.Location = new Point(235, 387);
            BtnEmailOK.Name = "BtnEmailOK";
            BtnEmailOK.Size = new Size(75, 35);
            BtnEmailOK.TabIndex = 1;
            BtnEmailOK.Text = "Save";
            BtnEmailOK.UseVisualStyleBackColor = false;
            BtnEmailOK.Click += BtnEmailOK_Click;
            // 
            // BtnEmailCancel
            // 
            BtnEmailCancel.BackColor = Color.FromArgb(244, 67, 54);
            BtnEmailCancel.FlatAppearance.BorderSize = 0;
            BtnEmailCancel.FlatStyle = FlatStyle.Flat;
            BtnEmailCancel.ForeColor = Color.White;
            BtnEmailCancel.Location = new Point(12, 387);
            BtnEmailCancel.Name = "BtnEmailCancel";
            BtnEmailCancel.Size = new Size(75, 35);
            BtnEmailCancel.TabIndex = 2;
            BtnEmailCancel.Text = "Cancel";
            BtnEmailCancel.UseVisualStyleBackColor = false;
            BtnEmailCancel.Click += BtnEmailCancel_Click;
            // 
            // TB_EmailUser
            // 
            TB_EmailUser.BackColor = Color.White;
            TB_EmailUser.BorderStyle = BorderStyle.None;
            TB_EmailUser.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_EmailUser.ForeColor = Color.FromArgb(102, 102, 102);
            TB_EmailUser.Location = new Point(11, 203);
            TB_EmailUser.Name = "TB_EmailUser";
            TB_EmailUser.Size = new Size(298, 22);
            TB_EmailUser.TabIndex = 4;
            TB_EmailUser.Text = "Ex : me@gmail.com";
            // 
            // TB_EmailPassword
            // 
            TB_EmailPassword.BackColor = Color.White;
            TB_EmailPassword.BorderStyle = BorderStyle.None;
            TB_EmailPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_EmailPassword.ForeColor = Color.FromArgb(102, 102, 102);
            TB_EmailPassword.Location = new Point(11, 250);
            TB_EmailPassword.Name = "TB_EmailPassword";
            TB_EmailPassword.Size = new Size(298, 22);
            TB_EmailPassword.TabIndex = 5;
            TB_EmailPassword.Text = "Ex: MyPassword1234";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 184);
            label1.Name = "label1";
            label1.Size = new Size(30, 15);
            label1.TabIndex = 6;
            label1.Text = "User";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 234);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(86, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(169, 156);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 284);
            label3.Name = "label3";
            label3.Size = new Size(37, 15);
            label3.TabIndex = 10;
            label3.Text = "SMTP";
            // 
            // TB_EmailSMTP
            // 
            TB_EmailSMTP.BackColor = Color.White;
            TB_EmailSMTP.BorderStyle = BorderStyle.None;
            TB_EmailSMTP.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_EmailSMTP.ForeColor = Color.FromArgb(102, 102, 102);
            TB_EmailSMTP.Location = new Point(13, 303);
            TB_EmailSMTP.Name = "TB_EmailSMTP";
            TB_EmailSMTP.PlaceholderText = "Ex : smtp.gmail.com";
            TB_EmailSMTP.Size = new Size(298, 22);
            TB_EmailSMTP.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 335);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 12;
            label4.Text = "Port";
            // 
            // TB_EmailPort
            // 
            TB_EmailPort.BackColor = Color.White;
            TB_EmailPort.BorderStyle = BorderStyle.None;
            TB_EmailPort.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_EmailPort.ForeColor = Color.FromArgb(102, 102, 102);
            TB_EmailPort.Location = new Point(13, 354);
            TB_EmailPort.Name = "TB_EmailPort";
            TB_EmailPort.PlaceholderText = "Ex : 465";
            TB_EmailPort.Size = new Size(298, 22);
            TB_EmailPort.TabIndex = 11;
            // 
            // EmailConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 434);
            Controls.Add(label4);
            Controls.Add(TB_EmailPort);
            Controls.Add(label3);
            Controls.Add(TB_EmailSMTP);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TB_EmailPassword);
            Controls.Add(TB_EmailUser);
            Controls.Add(BtnEmailCancel);
            Controls.Add(BtnEmailOK);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EmailConfig";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Login Screen";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnEmailOK;
        private Button BtnEmailCancel;
        private TextBox TB_EmailUser;
        private TextBox TB_EmailPassword;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private Label label3;
        private TextBox TB_EmailSMTP;
        private Label label4;
        private TextBox TB_EmailPort;
    }
}
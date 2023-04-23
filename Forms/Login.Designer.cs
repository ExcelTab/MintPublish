namespace Mint
{
    partial class Login
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
            BtnLoginOK = new Button();
            BtnLoginCancel = new Button();
            TB_Login = new TextBox();
            TB_Password = new TextBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // BtnLoginOK
            // 
            BtnLoginOK.BackColor = Color.FromArgb(63, 81, 181);
            BtnLoginOK.FlatAppearance.BorderSize = 0;
            BtnLoginOK.FlatStyle = FlatStyle.Flat;
            BtnLoginOK.ForeColor = Color.White;
            BtnLoginOK.Location = new Point(237, 269);
            BtnLoginOK.Name = "BtnLoginOK";
            BtnLoginOK.Size = new Size(75, 35);
            BtnLoginOK.TabIndex = 1;
            BtnLoginOK.Text = "Login";
            BtnLoginOK.UseVisualStyleBackColor = false;
            BtnLoginOK.Click += BtnLoginOK_Click;
            // 
            // BtnLoginCancel
            // 
            BtnLoginCancel.BackColor = Color.FromArgb(244, 67, 54);
            BtnLoginCancel.FlatAppearance.BorderSize = 0;
            BtnLoginCancel.FlatStyle = FlatStyle.Flat;
            BtnLoginCancel.ForeColor = Color.White;
            BtnLoginCancel.Location = new Point(18, 269);
            BtnLoginCancel.Name = "BtnLoginCancel";
            BtnLoginCancel.Size = new Size(75, 35);
            BtnLoginCancel.TabIndex = 2;
            BtnLoginCancel.Text = "Cancel";
            BtnLoginCancel.UseVisualStyleBackColor = false;
            BtnLoginCancel.Click += BtnLoginCancel_Click;
            // 
            // TB_Login
            // 
            TB_Login.BackColor = Color.White;
            TB_Login.BorderStyle = BorderStyle.None;
            TB_Login.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_Login.ForeColor = Color.FromArgb(102, 102, 102);
            TB_Login.Location = new Point(14, 179);
            TB_Login.Name = "TB_Login";
            TB_Login.Size = new Size(298, 22);
            TB_Login.TabIndex = 4;
            TB_Login.Text = "Enter your login";
            TB_Login.Enter += TB_Login_Enter;
            TB_Login.Leave += TB_Login_Leave;
            // 
            // TB_Password
            // 
            TB_Password.BackColor = Color.White;
            TB_Password.BorderStyle = BorderStyle.None;
            TB_Password.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TB_Password.ForeColor = Color.FromArgb(102, 102, 102);
            TB_Password.Location = new Point(14, 226);
            TB_Password.Name = "TB_Password";
            TB_Password.Size = new Size(298, 22);
            TB_Password.TabIndex = 5;
            TB_Password.Text = "Enter your password";
            TB_Password.Enter += TB_Password_Enter;
            TB_Password.Leave += TB_Password_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 160);
            label1.Name = "label1";
            label1.Size = new Size(37, 15);
            label1.TabIndex = 6;
            label1.Text = "Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 210);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.account;
            pictureBox1.Location = new Point(86, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(169, 156);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 320);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TB_Password);
            Controls.Add(TB_Login);
            Controls.Add(BtnLoginCancel);
            Controls.Add(BtnLoginOK);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Login";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Login Screen";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnLoginOK;
        private Button BtnLoginCancel;
        private TextBox TB_Login;
        private TextBox TB_Password;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
    }
}
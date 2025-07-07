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
            BtnEmailOK = new Button();
            BtnEmailCancel = new Button();
            this.TB_Login = new TextBox();
            this.TB_Password = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // BtnEmailOK
            // 
            BtnEmailOK.BackColor = Color.FromArgb(63, 81, 181);
            BtnEmailOK.FlatAppearance.BorderSize = 0;
            BtnEmailOK.FlatStyle = FlatStyle.Flat;
            BtnEmailOK.ForeColor = Color.White;
            BtnEmailOK.Location = new Point(237, 331);
            BtnEmailOK.Name = "BtnEmailOK";
            BtnEmailOK.Size = new Size(75, 35);
            BtnEmailOK.TabIndex = 1;
            BtnEmailOK.Text = "Save";
            BtnEmailOK.UseVisualStyleBackColor = false;
            BtnEmailOK.Click += this.BtnLoginOK_Click;
            // 
            // BtnEmailCancel
            // 
            BtnEmailCancel.BackColor = Color.FromArgb(244, 67, 54);
            BtnEmailCancel.FlatAppearance.BorderSize = 0;
            BtnEmailCancel.FlatStyle = FlatStyle.Flat;
            BtnEmailCancel.ForeColor = Color.White;
            BtnEmailCancel.Location = new Point(18, 331);
            BtnEmailCancel.Name = "BtnEmailCancel";
            BtnEmailCancel.Size = new Size(75, 35);
            BtnEmailCancel.TabIndex = 2;
            BtnEmailCancel.Text = "Cancel";
            BtnEmailCancel.UseVisualStyleBackColor = false;
            BtnEmailCancel.Click += this.BtnLoginCancel_Click;
            // 
            // TB_Login
            // 
            this.TB_Login.BackColor = Color.White;
            this.TB_Login.BorderStyle = BorderStyle.None;
            this.TB_Login.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.TB_Login.ForeColor = Color.FromArgb(102, 102, 102);
            this.TB_Login.Location = new Point(14, 179);
            this.TB_Login.Name = "TB_Login";
            this.TB_Login.Size = new Size(298, 22);
            this.TB_Login.TabIndex = 4;
            this.TB_Login.Text = "Enter your login";
            this.TB_Login.Enter += this.TB_Login_Enter;
            this.TB_Login.Leave += this.TB_Login_Leave;
            // 
            // TB_Password
            // 
            this.TB_Password.BackColor = Color.White;
            this.TB_Password.BorderStyle = BorderStyle.None;
            this.TB_Password.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.TB_Password.ForeColor = Color.FromArgb(102, 102, 102);
            this.TB_Password.Location = new Point(14, 226);
            this.TB_Password.Name = "TB_Password";
            this.TB_Password.Size = new Size(298, 22);
            this.TB_Password.TabIndex = 5;
            this.TB_Password.Text = "Enter your password";
            this.TB_Password.Enter += this.TB_Password_Enter;
            this.TB_Password.Leave += this.TB_Password_Leave;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 160);
            this.label1.Name = "label1";
            this.label1.Size = new Size(37, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Login";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(17, 210);
            this.label2.Name = "label2";
            this.label2.Size = new Size(57, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = Properties.Resources.account;
            this.pictureBox1.Location = new Point(86, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(169, 156);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 378);
            Controls.Add(this.pictureBox1);
            Controls.Add(this.label2);
            Controls.Add(this.label1);
            Controls.Add(this.TB_Password);
            Controls.Add(this.TB_Login);
            Controls.Add(BtnEmailCancel);
            Controls.Add(BtnEmailOK);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Login";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Login Screen";
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnEmailOK;
        private Button BtnEmailCancel;
        private TextBox TB_Login;
        private TextBox TB_Password;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
    }
}
namespace Mint.Forms
{
    partial class App_Orders
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
            label1 = new Label();
            Lbl_User = new Label();
            Lvi_Orders = new ListView();
            label2 = new Label();
            Cbx_GOF = new CheckBox();
            Cbx_EPP = new CheckBox();
            Cbx_EXP = new CheckBox();
            Cbx_CAN = new CheckBox();
            Cbx_NEW = new CheckBox();
            Cbx_OFP = new CheckBox();
            Groupbox_Status = new GroupBox();
            Button_Refresh = new Button();
            label3 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            Groupbox_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 21);
            label1.Name = "label1";
            label1.Size = new Size(279, 15);
            label1.TabIndex = 14;
            label1.Text = "Affichage des commandes visibles par l'utilisateur : ";
            // 
            // Lbl_User
            // 
            Lbl_User.AutoSize = true;
            Lbl_User.Location = new Point(304, 21);
            Lbl_User.Name = "Lbl_User";
            Lbl_User.Size = new Size(38, 15);
            Lbl_User.TabIndex = 15;
            Lbl_User.Text = "label2";
            // 
            // Lvi_Orders
            // 
            Lvi_Orders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Lvi_Orders.Location = new Point(12, 240);
            Lvi_Orders.Name = "Lvi_Orders";
            Lvi_Orders.Size = new Size(422, 290);
            Lvi_Orders.TabIndex = 16;
            Lvi_Orders.UseCompatibleStateImageBehavior = false;
            Lvi_Orders.SelectedIndexChanged += Lvi_Orders_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.BackColor = Color.FromArgb(255, 255, 192);
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(446, 9);
            label2.Name = "label2";
            label2.Size = new Size(538, 521);
            label2.TabIndex = 17;
            label2.Text = "La vue OF de la commande apparaitra ici";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Cbx_GOF
            // 
            Cbx_GOF.AutoSize = true;
            Cbx_GOF.Checked = true;
            Cbx_GOF.CheckState = CheckState.Checked;
            Cbx_GOF.Location = new Point(107, 13);
            Cbx_GOF.Name = "Cbx_GOF";
            Cbx_GOF.Size = new Size(49, 19);
            Cbx_GOF.TabIndex = 18;
            Cbx_GOF.Text = "GOF";
            Cbx_GOF.UseVisualStyleBackColor = true;
            // 
            // Cbx_EPP
            // 
            Cbx_EPP.AutoSize = true;
            Cbx_EPP.Checked = true;
            Cbx_EPP.CheckState = CheckState.Checked;
            Cbx_EPP.Location = new Point(159, 13);
            Cbx_EPP.Name = "Cbx_EPP";
            Cbx_EPP.Size = new Size(46, 19);
            Cbx_EPP.TabIndex = 19;
            Cbx_EPP.Text = "EPP";
            Cbx_EPP.UseVisualStyleBackColor = true;
            // 
            // Cbx_EXP
            // 
            Cbx_EXP.AutoSize = true;
            Cbx_EXP.Location = new Point(211, 13);
            Cbx_EXP.Name = "Cbx_EXP";
            Cbx_EXP.Size = new Size(46, 19);
            Cbx_EXP.TabIndex = 20;
            Cbx_EXP.Text = "EXP";
            Cbx_EXP.UseVisualStyleBackColor = true;
            // 
            // Cbx_CAN
            // 
            Cbx_CAN.AutoSize = true;
            Cbx_CAN.Location = new Point(263, 13);
            Cbx_CAN.Name = "Cbx_CAN";
            Cbx_CAN.Size = new Size(51, 19);
            Cbx_CAN.TabIndex = 21;
            Cbx_CAN.Text = "CAN";
            Cbx_CAN.UseVisualStyleBackColor = true;
            // 
            // Cbx_NEW
            // 
            Cbx_NEW.AutoSize = true;
            Cbx_NEW.Checked = true;
            Cbx_NEW.CheckState = CheckState.Checked;
            Cbx_NEW.Location = new Point(3, 13);
            Cbx_NEW.Name = "Cbx_NEW";
            Cbx_NEW.Size = new Size(52, 19);
            Cbx_NEW.TabIndex = 22;
            Cbx_NEW.Text = "NEW";
            Cbx_NEW.UseVisualStyleBackColor = true;
            Cbx_NEW.CheckedChanged += Cbx_NEW_CheckedChanged;
            // 
            // Cbx_OFP
            // 
            Cbx_OFP.AutoSize = true;
            Cbx_OFP.Checked = true;
            Cbx_OFP.CheckState = CheckState.Checked;
            Cbx_OFP.Location = new Point(55, 13);
            Cbx_OFP.Name = "Cbx_OFP";
            Cbx_OFP.Size = new Size(48, 19);
            Cbx_OFP.TabIndex = 23;
            Cbx_OFP.Text = "OFP";
            Cbx_OFP.UseVisualStyleBackColor = true;
            // 
            // Groupbox_Status
            // 
            Groupbox_Status.Controls.Add(Cbx_OFP);
            Groupbox_Status.Controls.Add(Cbx_GOF);
            Groupbox_Status.Controls.Add(Cbx_NEW);
            Groupbox_Status.Controls.Add(Cbx_EPP);
            Groupbox_Status.Controls.Add(Cbx_CAN);
            Groupbox_Status.Controls.Add(Cbx_EXP);
            Groupbox_Status.Location = new Point(19, 194);
            Groupbox_Status.Name = "Groupbox_Status";
            Groupbox_Status.Size = new Size(328, 40);
            Groupbox_Status.TabIndex = 24;
            Groupbox_Status.TabStop = false;
            // 
            // Button_Refresh
            // 
            Button_Refresh.Location = new Point(353, 207);
            Button_Refresh.Name = "Button_Refresh";
            Button_Refresh.Size = new Size(75, 23);
            Button_Refresh.TabIndex = 25;
            Button_Refresh.Text = "Refresh";
            Button_Refresh.UseVisualStyleBackColor = true;
            Button_Refresh.Click += Button_Refresh_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 189);
            label3.Name = "label3";
            label3.Size = new Size(229, 15);
            label3.TabIndex = 26;
            label3.Text = "Ou choisissez une commande dans la liste";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 50);
            label4.Name = "label4";
            label4.Size = new Size(206, 15);
            label4.TabIndex = 27;
            label4.Text = "Encodez ou scannez une commande :";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(109, 88);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(325, 39);
            textBox1.TabIndex = 28;
            // 
            // button1
            // 
            button1.Location = new Point(399, 133);
            button1.Name = "button1";
            button1.Size = new Size(35, 23);
            button1.TabIndex = 29;
            button1.Text = "Go";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 88);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 39);
            pictureBox1.TabIndex = 30;
            pictureBox1.TabStop = false;
            // 
            // App_Orders
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(996, 542);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(Button_Refresh);
            Controls.Add(Groupbox_Status);
            Controls.Add(label2);
            Controls.Add(Lvi_Orders);
            Controls.Add(Lbl_User);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "App_Orders";
            StartPosition = FormStartPosition.Manual;
            Text = "App_Orders";
            Groupbox_Status.ResumeLayout(false);
            Groupbox_Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label Lbl_User;
        private ListView Lvi_Orders;
        private Label label2;
        private CheckBox Cbx_GOF;
        private CheckBox Cbx_EPP;
        private CheckBox Cbx_EXP;
        private CheckBox Cbx_CAN;
        private CheckBox Cbx_NEW;
        private CheckBox Cbx_OFP;
        private GroupBox Groupbox_Status;
        private Button Button_Refresh;
        private Label label3;
        private Label label4;
        private TextBox textBox1;
        private Button button1;
        private PictureBox pictureBox1;
    }
}
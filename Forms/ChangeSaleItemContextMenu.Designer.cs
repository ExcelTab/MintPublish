namespace Mint.Forms
{
    partial class ChangeSaleItemContextMenu
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
            label_id_order_detail = new Label();
            label2 = new Label();
            comboBox_Atelier = new ComboBox();
            label1 = new Label();
            label3 = new Label();
            comboBox_Article = new ComboBox();
            textBox_TotalCost = new TextBox();
            label4 = new Label();
            button1 = new Button();
            button2 = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label_id_order_detail
            // 
            label_id_order_detail.Location = new Point(12, 18);
            label_id_order_detail.Name = "label_id_order_detail";
            label_id_order_detail.Size = new Size(175, 23);
            label_id_order_detail.TabIndex = 0;
            label_id_order_detail.Text = "label_id_order_detail";
            label_id_order_detail.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 3);
            label2.Name = "label2";
            label2.Size = new Size(175, 15);
            label2.TabIndex = 1;
            label2.Text = "Changer les données de l'objet :";
            // 
            // comboBox_Atelier
            // 
            comboBox_Atelier.FormattingEnabled = true;
            comboBox_Atelier.Location = new Point(79, 42);
            comboBox_Atelier.Name = "comboBox_Atelier";
            comboBox_Atelier.Size = new Size(106, 23);
            comboBox_Atelier.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 45);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 3;
            label1.Text = "Atelier";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 74);
            label3.Name = "label3";
            label3.Size = new Size(41, 15);
            label3.TabIndex = 5;
            label3.Text = "Article";
            // 
            // comboBox_Article
            // 
            comboBox_Article.FormattingEnabled = true;
            comboBox_Article.Location = new Point(79, 71);
            comboBox_Article.Name = "comboBox_Article";
            comboBox_Article.Size = new Size(106, 23);
            comboBox_Article.TabIndex = 4;
            // 
            // textBox_TotalCost
            // 
            textBox_TotalCost.Location = new Point(79, 100);
            textBox_TotalCost.Name = "textBox_TotalCost";
            textBox_TotalCost.Size = new Size(106, 23);
            textBox_TotalCost.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 103);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 7;
            label4.Text = "Coût prod";
            // 
            // button1
            // 
            button1.Location = new Point(110, 129);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 8;
            button1.Text = "Sauver";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(11, 129);
            button2.Name = "button2";
            button2.Size = new Size(68, 23);
            button2.TabIndex = 9;
            button2.Text = "Annuler";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label_id_order_detail);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(comboBox_Atelier);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox_TotalCost);
            panel1.Controls.Add(comboBox_Article);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(204, 165);
            panel1.TabIndex = 10;
            // 
            // ChangeSaleItemContextMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(204, 165);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ChangeSaleItemContextMenu";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "ChangeSaleItemContextMenu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label_id_order_detail;
        private Label label2;
        private ComboBox comboBox_Atelier;
        private Label label1;
        private Label label3;
        private ComboBox comboBox_Article;
        private TextBox textBox_TotalCost;
        private Label label4;
        private Button button1;
        private Button button2;
        private Panel panel1;
    }
}
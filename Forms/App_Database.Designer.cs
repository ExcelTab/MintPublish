namespace Mint.Forms
{
    partial class App_Database
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
            Btn_Save = new Button();
            Dgv_Main = new Controls.DoubleBufferedDGV();
            Btn_Refresh = new Button();
            Cbb_Database = new ComboBox();
            label2 = new Label();
            Lbo_Tables = new ListBox();
            Lbl_Charging = new Label();
            button1 = new Button();
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)Dgv_Main).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // Btn_Save
            // 
            Btn_Save.Location = new Point(331, 12);
            Btn_Save.Name = "Btn_Save";
            Btn_Save.Size = new Size(75, 23);
            Btn_Save.TabIndex = 12;
            Btn_Save.Text = "Save";
            Btn_Save.UseVisualStyleBackColor = true;
            Btn_Save.Click += Save_Datatable;
            // 
            // Dgv_Main
            // 
            Dgv_Main.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Dgv_Main.Dock = DockStyle.Fill;
            Dgv_Main.Location = new Point(0, 0);
            Dgv_Main.Name = "Dgv_Main";
            Dgv_Main.RowTemplate.Height = 25;
            Dgv_Main.Size = new Size(718, 385);
            Dgv_Main.TabIndex = 11;
            Dgv_Main.VirtualMode = true;
            Dgv_Main.CellValueNeeded += Dgv_Main_CellValueNeeded;
            // 
            // Btn_Refresh
            // 
            Btn_Refresh.Location = new Point(250, 12);
            Btn_Refresh.Name = "Btn_Refresh";
            Btn_Refresh.Size = new Size(75, 23);
            Btn_Refresh.TabIndex = 10;
            Btn_Refresh.Text = "Refresh";
            Btn_Refresh.UseVisualStyleBackColor = true;
            Btn_Refresh.Click += Refresh_Datatable;
            // 
            // Cbb_Database
            // 
            Cbb_Database.FormattingEnabled = true;
            Cbb_Database.Location = new Point(78, 12);
            Cbb_Database.Name = "Cbb_Database";
            Cbb_Database.Size = new Size(166, 23);
            Cbb_Database.TabIndex = 13;
            Cbb_Database.SelectedIndexChanged += Load_Datatables;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 15);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 16;
            label2.Text = "Database :";
            // 
            // Lbo_Tables
            // 
            Lbo_Tables.Dock = DockStyle.Fill;
            Lbo_Tables.FormattingEnabled = true;
            Lbo_Tables.ItemHeight = 15;
            Lbo_Tables.Location = new Point(0, 0);
            Lbo_Tables.Name = "Lbo_Tables";
            Lbo_Tables.Size = new Size(200, 385);
            Lbo_Tables.TabIndex = 17;
            Lbo_Tables.SelectedIndexChanged += Refresh_Datatable;
            // 
            // Lbl_Charging
            // 
            Lbl_Charging.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Lbl_Charging.AutoSize = true;
            Lbl_Charging.BackColor = Color.FromArgb(255, 255, 128);
            Lbl_Charging.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point);
            Lbl_Charging.Location = new Point(426, 12);
            Lbl_Charging.Name = "Lbl_Charging";
            Lbl_Charging.Size = new Size(320, 21);
            Lbl_Charging.TabIndex = 18;
            Lbl_Charging.Text = "Chargement des données... Veuillez patienter";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(255, 192, 128);
            button1.Location = new Point(752, 12);
            button1.Name = "button1";
            button1.Size = new Size(156, 23);
            button1.TabIndex = 19;
            button1.Text = "Update From Buffer";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Refresh_Buffer;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(Btn_Refresh);
            panel1.Controls.Add(Lbl_Charging);
            panel1.Controls.Add(Btn_Save);
            panel1.Controls.Add(Cbb_Database);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(922, 46);
            panel1.TabIndex = 20;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 46);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Lbo_Tables);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(Dgv_Main);
            splitContainer1.Size = new Size(922, 385);
            splitContainer1.SplitterDistance = 200;
            splitContainer1.TabIndex = 21;
            // 
            // App_Database
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 431);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "App_Database";
            StartPosition = FormStartPosition.Manual;
            Text = "App_Database";
            ((System.ComponentModel.ISupportInitialize)Dgv_Main).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button Btn_Save;
        private Mint.Controls.DoubleBufferedDGV Dgv_Main;
        private Button Btn_Refresh;
        private ComboBox Cbb_Database;
        private Label label2;
        private ListBox Lbo_Tables;
        private Label Lbl_Charging;
        private Button button1;
        private Panel panel1;
        private SplitContainer splitContainer1;
    }
}
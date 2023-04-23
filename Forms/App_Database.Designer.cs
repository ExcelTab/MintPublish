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
            Dgv_Main = new DataGridView();
            Btn_Refresh = new Button();
            Cbb_Database = new ComboBox();
            label2 = new Label();
            Lbo_Tables = new ListBox();
            Lbl_Charging = new Label();
            ((System.ComponentModel.ISupportInitialize)Dgv_Main).BeginInit();
            SuspendLayout();
            // 
            // Btn_Save
            // 
            Btn_Save.Location = new Point(273, 12);
            Btn_Save.Name = "Btn_Save";
            Btn_Save.Size = new Size(75, 23);
            Btn_Save.TabIndex = 12;
            Btn_Save.Text = "Save";
            Btn_Save.UseVisualStyleBackColor = true;
            Btn_Save.Click += Save_Datatable;
            // 
            // Dgv_Main
            // 
            Dgv_Main.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Dgv_Main.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Dgv_Main.Location = new Point(191, 41);
            Dgv_Main.Name = "Dgv_Main";
            Dgv_Main.RowTemplate.Height = 25;
            Dgv_Main.Size = new Size(793, 484);
            Dgv_Main.TabIndex = 11;
            Dgv_Main.VirtualMode = true;
            Dgv_Main.CellValueNeeded += Dgv_Main_CellValueNeeded;
            // 
            // Btn_Refresh
            // 
            Btn_Refresh.Location = new Point(192, 12);
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
            Cbb_Database.Location = new Point(80, 13);
            Cbb_Database.Name = "Cbb_Database";
            Cbb_Database.Size = new Size(105, 23);
            Cbb_Database.TabIndex = 13;
            Cbb_Database.SelectedIndexChanged += Load_Datatables;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 16);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 16;
            label2.Text = "Database :";
            // 
            // Lbo_Tables
            // 
            Lbo_Tables.FormattingEnabled = true;
            Lbo_Tables.ItemHeight = 15;
            Lbo_Tables.Location = new Point(12, 41);
            Lbo_Tables.Name = "Lbo_Tables";
            Lbo_Tables.Size = new Size(173, 484);
            Lbo_Tables.TabIndex = 17;
            Lbo_Tables.SelectedIndexChanged += Refresh_Datatable;
            // 
            // Lbl_Charging
            // 
            Lbl_Charging.AutoSize = true;
            Lbl_Charging.BackColor = Color.FromArgb(255, 255, 128);
            Lbl_Charging.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point);
            Lbl_Charging.Location = new Point(376, 13);
            Lbl_Charging.Name = "Lbl_Charging";
            Lbl_Charging.Size = new Size(320, 21);
            Lbl_Charging.TabIndex = 18;
            Lbl_Charging.Text = "Chargement des données... Veuillez patienter";
            // 
            // App_Database
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(996, 542);
            Controls.Add(Lbl_Charging);
            Controls.Add(Lbo_Tables);
            Controls.Add(label2);
            Controls.Add(Cbb_Database);
            Controls.Add(Btn_Save);
            Controls.Add(Btn_Refresh);
            Controls.Add(Dgv_Main);
            FormBorderStyle = FormBorderStyle.None;
            Name = "App_Database";
            StartPosition = FormStartPosition.Manual;
            Text = "App_Database";
            ((System.ComponentModel.ISupportInitialize)Dgv_Main).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Btn_Save;
        private DataGridView Dgv_Main;
        private Button Btn_Refresh;
        private ComboBox Cbb_Database;
        private Label label2;
        private ListBox Lbo_Tables;
        private Label Lbl_Charging;
    }
}
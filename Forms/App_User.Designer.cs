namespace Mint.Forms
{
    partial class App_User
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App_User));
            tabControl1 = new TabControl();
            tabPage_Users = new TabPage();
            dataGridView_Users = new Controls.DoubleBufferedDGV();
            panel9 = new Panel();
            button_RefreshUsers = new Button();
            button_SaveUsers = new Button();
            tabPage_Ateliers = new TabPage();
            dataGridView_Ateliers = new Controls.DoubleBufferedDGV();
            panel1 = new Panel();
            button_RefreshAteliers = new Button();
            button_SaveAteliers = new Button();
            tabPage_Artistes = new TabPage();
            dataGridView_Artistes = new Controls.DoubleBufferedDGV();
            panel2 = new Panel();
            button_RefreshArtistes = new Button();
            button_SaveArtistes = new Button();
            imageList1 = new ImageList(components);
            tabControl1.SuspendLayout();
            tabPage_Users.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Users).BeginInit();
            panel9.SuspendLayout();
            tabPage_Ateliers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Ateliers).BeginInit();
            panel1.SuspendLayout();
            tabPage_Artistes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Artistes).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage_Users);
            tabControl1.Controls.Add(tabPage_Ateliers);
            tabControl1.Controls.Add(tabPage_Artistes);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ImageList = imageList1;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage_Users
            // 
            tabPage_Users.Controls.Add(dataGridView_Users);
            tabPage_Users.Controls.Add(panel9);
            tabPage_Users.ImageIndex = 3;
            tabPage_Users.Location = new Point(4, 24);
            tabPage_Users.Name = "tabPage_Users";
            tabPage_Users.Padding = new Padding(3);
            tabPage_Users.Size = new Size(792, 422);
            tabPage_Users.TabIndex = 0;
            tabPage_Users.Text = "Utilisateurs du Mint";
            tabPage_Users.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Users
            // 
            dataGridView_Users.AllowDrop = true;
            dataGridView_Users.AllowUserToDeleteRows = false;
            dataGridView_Users.AllowUserToOrderColumns = true;
            dataGridView_Users.AllowUserToResizeRows = false;
            dataGridView_Users.BackgroundColor = Color.White;
            dataGridView_Users.CausesValidation = false;
            dataGridView_Users.ColumnHeadersHeight = 40;
            dataGridView_Users.Dock = DockStyle.Fill;
            dataGridView_Users.GridColor = Color.Gray;
            dataGridView_Users.Location = new Point(3, 33);
            dataGridView_Users.Name = "dataGridView_Users";
            dataGridView_Users.RowHeadersVisible = false;
            dataGridView_Users.RowTemplate.Height = 25;
            dataGridView_Users.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView_Users.ShowCellToolTips = false;
            dataGridView_Users.Size = new Size(786, 386);
            dataGridView_Users.TabIndex = 10;
            dataGridView_Users.CellDoubleClick += doubleClickDGVCell;
            dataGridView_Users.CellValueChanged += changeDGVCell;
            dataGridView_Users.DataError += DGV_DataError;
            dataGridView_Users.SelectionChanged += selectDGVRow;
            dataGridView_Users.KeyDown += DGV_KeyDown;
            // 
            // panel9
            // 
            panel9.Controls.Add(button_RefreshUsers);
            panel9.Controls.Add(button_SaveUsers);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(3, 3);
            panel9.Name = "panel9";
            panel9.Size = new Size(786, 30);
            panel9.TabIndex = 11;
            // 
            // button_RefreshUsers
            // 
            button_RefreshUsers.Location = new Point(3, 3);
            button_RefreshUsers.Name = "button_RefreshUsers";
            button_RefreshUsers.Size = new Size(126, 23);
            button_RefreshUsers.TabIndex = 7;
            button_RefreshUsers.Text = "Rafraichir la liste";
            button_RefreshUsers.UseVisualStyleBackColor = true;
            button_RefreshUsers.Click += button_Refresh_Click;
            // 
            // button_SaveUsers
            // 
            button_SaveUsers.Location = new Point(135, 3);
            button_SaveUsers.Name = "button_SaveUsers";
            button_SaveUsers.Size = new Size(126, 23);
            button_SaveUsers.TabIndex = 8;
            button_SaveUsers.Text = "Sauver la liste";
            button_SaveUsers.UseVisualStyleBackColor = true;
            button_SaveUsers.Click += button_SaveList_Click;
            // 
            // tabPage_Ateliers
            // 
            tabPage_Ateliers.Controls.Add(dataGridView_Ateliers);
            tabPage_Ateliers.Controls.Add(panel1);
            tabPage_Ateliers.ImageIndex = 0;
            tabPage_Ateliers.Location = new Point(4, 24);
            tabPage_Ateliers.Name = "tabPage_Ateliers";
            tabPage_Ateliers.Padding = new Padding(3);
            tabPage_Ateliers.Size = new Size(792, 422);
            tabPage_Ateliers.TabIndex = 1;
            tabPage_Ateliers.Text = "Ateliers de production";
            tabPage_Ateliers.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Ateliers
            // 
            dataGridView_Ateliers.AllowDrop = true;
            dataGridView_Ateliers.AllowUserToDeleteRows = false;
            dataGridView_Ateliers.AllowUserToOrderColumns = true;
            dataGridView_Ateliers.AllowUserToResizeRows = false;
            dataGridView_Ateliers.BackgroundColor = Color.White;
            dataGridView_Ateliers.CausesValidation = false;
            dataGridView_Ateliers.ColumnHeadersHeight = 40;
            dataGridView_Ateliers.Dock = DockStyle.Fill;
            dataGridView_Ateliers.GridColor = Color.Gray;
            dataGridView_Ateliers.Location = new Point(3, 33);
            dataGridView_Ateliers.Name = "dataGridView_Ateliers";
            dataGridView_Ateliers.RowHeadersVisible = false;
            dataGridView_Ateliers.RowTemplate.Height = 25;
            dataGridView_Ateliers.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView_Ateliers.ShowCellToolTips = false;
            dataGridView_Ateliers.Size = new Size(786, 386);
            dataGridView_Ateliers.TabIndex = 10;
            dataGridView_Ateliers.CellDoubleClick += doubleClickDGVCell;
            dataGridView_Ateliers.CellValueChanged += changeDGVCell;
            dataGridView_Ateliers.DataError += DGV_DataError;
            dataGridView_Ateliers.SelectionChanged += selectDGVRow;
            dataGridView_Ateliers.KeyDown += DGV_KeyDown;
            // 
            // panel1
            // 
            panel1.Controls.Add(button_RefreshAteliers);
            panel1.Controls.Add(button_SaveAteliers);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(786, 30);
            panel1.TabIndex = 11;
            // 
            // button_RefreshAteliers
            // 
            button_RefreshAteliers.Location = new Point(3, 3);
            button_RefreshAteliers.Name = "button_RefreshAteliers";
            button_RefreshAteliers.Size = new Size(126, 23);
            button_RefreshAteliers.TabIndex = 7;
            button_RefreshAteliers.Text = "Rafraichir la liste";
            button_RefreshAteliers.UseVisualStyleBackColor = true;
            button_RefreshAteliers.Click += button_Refresh_Click;
            // 
            // button_SaveAteliers
            // 
            button_SaveAteliers.Location = new Point(135, 3);
            button_SaveAteliers.Name = "button_SaveAteliers";
            button_SaveAteliers.Size = new Size(126, 23);
            button_SaveAteliers.TabIndex = 8;
            button_SaveAteliers.Text = "Sauver la liste";
            button_SaveAteliers.UseVisualStyleBackColor = true;
            button_SaveAteliers.Click += button_SaveList_Click;
            // 
            // tabPage_Artistes
            // 
            tabPage_Artistes.Controls.Add(dataGridView_Artistes);
            tabPage_Artistes.Controls.Add(panel2);
            tabPage_Artistes.ImageIndex = 2;
            tabPage_Artistes.Location = new Point(4, 24);
            tabPage_Artistes.Name = "tabPage_Artistes";
            tabPage_Artistes.Padding = new Padding(3);
            tabPage_Artistes.Size = new Size(792, 422);
            tabPage_Artistes.TabIndex = 2;
            tabPage_Artistes.Text = "Artistes";
            tabPage_Artistes.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Artistes
            // 
            dataGridView_Artistes.AllowDrop = true;
            dataGridView_Artistes.AllowUserToDeleteRows = false;
            dataGridView_Artistes.AllowUserToOrderColumns = true;
            dataGridView_Artistes.AllowUserToResizeRows = false;
            dataGridView_Artistes.BackgroundColor = Color.White;
            dataGridView_Artistes.CausesValidation = false;
            dataGridView_Artistes.ColumnHeadersHeight = 40;
            dataGridView_Artistes.Dock = DockStyle.Fill;
            dataGridView_Artistes.GridColor = Color.Gray;
            dataGridView_Artistes.Location = new Point(3, 33);
            dataGridView_Artistes.Name = "dataGridView_Artistes";
            dataGridView_Artistes.RowHeadersVisible = false;
            dataGridView_Artistes.RowTemplate.Height = 25;
            dataGridView_Artistes.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView_Artistes.ShowCellToolTips = false;
            dataGridView_Artistes.Size = new Size(786, 386);
            dataGridView_Artistes.TabIndex = 10;
            dataGridView_Artistes.CellDoubleClick += doubleClickDGVCell;
            dataGridView_Artistes.CellValueChanged += changeDGVCell;
            dataGridView_Artistes.DataError += DGV_DataError;
            dataGridView_Artistes.SelectionChanged += selectDGVRow;
            dataGridView_Artistes.KeyDown += DGV_KeyDown;
            // 
            // panel2
            // 
            panel2.Controls.Add(button_RefreshArtistes);
            panel2.Controls.Add(button_SaveArtistes);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(786, 30);
            panel2.TabIndex = 11;
            // 
            // button_RefreshArtistes
            // 
            button_RefreshArtistes.Location = new Point(3, 3);
            button_RefreshArtistes.Name = "button_RefreshArtistes";
            button_RefreshArtistes.Size = new Size(126, 23);
            button_RefreshArtistes.TabIndex = 7;
            button_RefreshArtistes.Text = "Rafraichir la liste";
            button_RefreshArtistes.UseVisualStyleBackColor = true;
            button_RefreshArtistes.Click += button_Refresh_Click;
            // 
            // button_SaveArtistes
            // 
            button_SaveArtistes.Location = new Point(135, 3);
            button_SaveArtistes.Name = "button_SaveArtistes";
            button_SaveArtistes.Size = new Size(126, 23);
            button_SaveArtistes.TabIndex = 8;
            button_SaveArtistes.Text = "Sauver la liste";
            button_SaveArtistes.UseVisualStyleBackColor = true;
            button_SaveArtistes.Click += button_SaveList_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "Profil1.jpg");
            imageList1.Images.SetKeyName(1, "Profil2.jpg");
            imageList1.Images.SetKeyName(2, "Profil3.jpg");
            imageList1.Images.SetKeyName(3, "Profil4.jpg");
            // 
            // App_User
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "App_User";
            Text = "App_User";
            tabControl1.ResumeLayout(false);
            tabPage_Users.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_Users).EndInit();
            panel9.ResumeLayout(false);
            tabPage_Ateliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_Ateliers).EndInit();
            panel1.ResumeLayout(false);
            tabPage_Artistes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_Artistes).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage_Users;
        private TabPage tabPage_Ateliers;
        private TabPage tabPage_Artistes;
        private ImageList imageList1;
        private Controls.DoubleBufferedDGV dataGridView_Users;
        private Panel panel9;
        private Button button_RefreshUsers;
        private Button button_SaveUsers;
        private Controls.DoubleBufferedDGV dataGridView_Ateliers;
        private Panel panel1;
        private Button button_RefreshAteliers;
        private Button button_SaveAteliers;
        private Controls.DoubleBufferedDGV dataGridView_Artistes;
        private Panel panel2;
        private Button button_RefreshArtistes;
        private Button button_SaveArtistes;
    }
}
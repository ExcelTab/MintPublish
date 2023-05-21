namespace Mint.Forms
{
    partial class App_Products
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
            Btn_Add = new Button();
            Btn_Modify = new Button();
            Btn_Save = new Button();
            Btn_Extract = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            Txt_Reference = new TextBox();
            Txt_Name = new TextBox();
            Txt_EAN13 = new TextBox();
            Img_Product = new PictureBox();
            label5 = new Label();
            Txt_Artist = new TextBox();
            label6 = new Label();
            Btn_SelectArtist = new Button();
            Grp_Product = new GroupBox();
            Btn_SelectCategory = new Button();
            Txt_MainCategory = new TextBox();
            label7 = new Label();
            Tab_Features = new TabControl();
            tabPage1 = new TabPage();
            Trv_Category = new TreeView();
            Pan_Declinaisons = new Panel();
            ((System.ComponentModel.ISupportInitialize)Img_Product).BeginInit();
            Grp_Product.SuspendLayout();
            Tab_Features.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // Btn_Add
            // 
            Btn_Add.Location = new Point(12, 12);
            Btn_Add.Name = "Btn_Add";
            Btn_Add.Size = new Size(146, 23);
            Btn_Add.TabIndex = 0;
            Btn_Add.Text = "Nouveau Produit";
            Btn_Add.UseVisualStyleBackColor = true;
            Btn_Add.Click += Btn_Add_Click;
            // 
            // Btn_Modify
            // 
            Btn_Modify.Location = new Point(179, 12);
            Btn_Modify.Name = "Btn_Modify";
            Btn_Modify.Size = new Size(146, 23);
            Btn_Modify.TabIndex = 1;
            Btn_Modify.Text = "Modifier un Produit";
            Btn_Modify.UseVisualStyleBackColor = true;
            Btn_Modify.Click += Btn_Modify_Click;
            // 
            // Btn_Save
            // 
            Btn_Save.Location = new Point(348, 12);
            Btn_Save.Name = "Btn_Save";
            Btn_Save.Size = new Size(146, 23);
            Btn_Save.TabIndex = 2;
            Btn_Save.Text = "Sauver";
            Btn_Save.UseVisualStyleBackColor = true;
            Btn_Save.Click += Btn_Save_Click;
            // 
            // Btn_Extract
            // 
            Btn_Extract.Location = new Point(512, 12);
            Btn_Extract.Name = "Btn_Extract";
            Btn_Extract.Size = new Size(256, 24);
            Btn_Extract.TabIndex = 3;
            Btn_Extract.Text = "Sortir le fichier StoreCommander";
            Btn_Extract.UseVisualStyleBackColor = true;
            Btn_Extract.Click += Btn_Extract_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 31);
            label2.Name = "label2";
            label2.Size = new Size(59, 15);
            label2.TabIndex = 5;
            label2.Text = "Référence";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 89);
            label3.Name = "label3";
            label3.Size = new Size(42, 15);
            label3.TabIndex = 6;
            label3.Text = "EAN13";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 60);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 7;
            label4.Text = "Nom du produit";
            // 
            // Txt_Reference
            // 
            Txt_Reference.Location = new Point(170, 23);
            Txt_Reference.Name = "Txt_Reference";
            Txt_Reference.Size = new Size(245, 23);
            Txt_Reference.TabIndex = 8;
            // 
            // Txt_Name
            // 
            Txt_Name.Location = new Point(170, 52);
            Txt_Name.Name = "Txt_Name";
            Txt_Name.Size = new Size(245, 23);
            Txt_Name.TabIndex = 9;
            // 
            // Txt_EAN13
            // 
            Txt_EAN13.Location = new Point(170, 81);
            Txt_EAN13.Name = "Txt_EAN13";
            Txt_EAN13.Size = new Size(245, 23);
            Txt_EAN13.TabIndex = 11;
            // 
            // Img_Product
            // 
            Img_Product.Location = new Point(22, 183);
            Img_Product.Name = "Img_Product";
            Img_Product.Size = new Size(396, 168);
            Img_Product.SizeMode = PictureBoxSizeMode.StretchImage;
            Img_Product.TabIndex = 12;
            Img_Product.TabStop = false;
            Img_Product.Click += PictureBox_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 118);
            label5.Name = "label5";
            label5.Size = new Size(41, 15);
            label5.TabIndex = 13;
            label5.Text = "Artiste";
            // 
            // Txt_Artist
            // 
            Txt_Artist.Location = new Point(170, 110);
            Txt_Artist.Name = "Txt_Artist";
            Txt_Artist.Size = new Size(226, 23);
            Txt_Artist.TabIndex = 14;
            Txt_Artist.TextChanged += Txt_Artist_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(186, 174);
            label6.Name = "label6";
            label6.Size = new Size(63, 15);
            label6.TabIndex = 15;
            label6.Text = "Illustration";
            // 
            // Btn_SelectArtist
            // 
            Btn_SelectArtist.Location = new Point(383, 109);
            Btn_SelectArtist.Name = "Btn_SelectArtist";
            Btn_SelectArtist.Size = new Size(32, 24);
            Btn_SelectArtist.TabIndex = 16;
            Btn_SelectArtist.Text = "...";
            Btn_SelectArtist.UseVisualStyleBackColor = true;
            Btn_SelectArtist.Click += Load_Artist_List;
            // 
            // Grp_Product
            // 
            Grp_Product.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Grp_Product.Controls.Add(Btn_SelectCategory);
            Grp_Product.Controls.Add(Txt_MainCategory);
            Grp_Product.Controls.Add(label7);
            Grp_Product.Controls.Add(Tab_Features);
            Grp_Product.Controls.Add(label4);
            Grp_Product.Controls.Add(Btn_SelectArtist);
            Grp_Product.Controls.Add(label6);
            Grp_Product.Controls.Add(label2);
            Grp_Product.Controls.Add(Txt_Artist);
            Grp_Product.Controls.Add(label3);
            Grp_Product.Controls.Add(label5);
            Grp_Product.Controls.Add(Txt_Reference);
            Grp_Product.Controls.Add(Img_Product);
            Grp_Product.Controls.Add(Txt_Name);
            Grp_Product.Controls.Add(Txt_EAN13);
            Grp_Product.Location = new Point(12, 54);
            Grp_Product.Name = "Grp_Product";
            Grp_Product.Size = new Size(431, 614);
            Grp_Product.TabIndex = 17;
            Grp_Product.TabStop = false;
            Grp_Product.Text = "Parent";
            // 
            // Btn_SelectCategory
            // 
            Btn_SelectCategory.Location = new Point(383, 139);
            Btn_SelectCategory.Name = "Btn_SelectCategory";
            Btn_SelectCategory.Size = new Size(32, 24);
            Btn_SelectCategory.TabIndex = 20;
            Btn_SelectCategory.Text = "...";
            Btn_SelectCategory.UseVisualStyleBackColor = true;
            Btn_SelectCategory.Click += Load_Main_Category_List;
            // 
            // Txt_MainCategory
            // 
            Txt_MainCategory.Location = new Point(170, 139);
            Txt_MainCategory.Name = "Txt_MainCategory";
            Txt_MainCategory.Size = new Size(226, 23);
            Txt_MainCategory.TabIndex = 19;
            Txt_MainCategory.TextChanged += Txt_MainCategory_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 147);
            label7.Name = "label7";
            label7.Size = new Size(101, 15);
            label7.TabIndex = 18;
            label7.Text = "Catégorie de base";
            // 
            // Tab_Features
            // 
            Tab_Features.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Tab_Features.Controls.Add(tabPage1);
            Tab_Features.Location = new Point(15, 357);
            Tab_Features.Name = "Tab_Features";
            Tab_Features.SelectedIndex = 0;
            Tab_Features.Size = new Size(400, 251);
            Tab_Features.TabIndex = 17;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(Trv_Category);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(392, 223);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Catégorie";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // Trv_Category
            // 
            Trv_Category.CheckBoxes = true;
            Trv_Category.Dock = DockStyle.Fill;
            Trv_Category.FullRowSelect = true;
            Trv_Category.Location = new Point(3, 3);
            Trv_Category.Name = "Trv_Category";
            Trv_Category.Size = new Size(386, 217);
            Trv_Category.TabIndex = 0;
            // 
            // Pan_Declinaisons
            // 
            Pan_Declinaisons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Pan_Declinaisons.AutoScroll = true;
            Pan_Declinaisons.Location = new Point(461, 64);
            Pan_Declinaisons.Name = "Pan_Declinaisons";
            Pan_Declinaisons.Size = new Size(530, 604);
            Pan_Declinaisons.TabIndex = 18;
            // 
            // App_Products
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1003, 680);
            Controls.Add(Pan_Declinaisons);
            Controls.Add(Grp_Product);
            Controls.Add(Btn_Extract);
            Controls.Add(Btn_Save);
            Controls.Add(Btn_Modify);
            Controls.Add(Btn_Add);
            Name = "App_Products";
            Text = "Gestion des Produits";
            ((System.ComponentModel.ISupportInitialize)Img_Product).EndInit();
            Grp_Product.ResumeLayout(false);
            Grp_Product.PerformLayout();
            Tab_Features.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button Btn_Add;
        private Button Btn_Modify;
        private Button Btn_Save;
        private Button Btn_Extract;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox Txt_Reference;
        private TextBox Txt_Name;
        private TextBox Txt_EAN13;
        private PictureBox Img_Product;
        private Label label5;
        private TextBox Txt_Artist;
        private Label label6;
        private Button Btn_SelectArtist;
        private GroupBox Grp_Product;
        private TabControl Tab_Features;
        private TabPage tabPage1;
        private TreeView Trv_Category;
        private Button Btn_SelectCategory;
        private TextBox Txt_MainCategory;
        private Label label7;
        private Panel Pan_Declinaisons;
    }
}
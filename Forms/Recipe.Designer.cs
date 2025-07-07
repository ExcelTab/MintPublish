namespace Mint.Forms
{
    partial class Recipe
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
            label_nameArticle = new Label();
            label_idArticle = new Label();
            groupBox_Surface = new GroupBox();
            comboBox_Surface = new ComboBox();
            label_meters = new Label();
            label_dimension = new Label();
            button_Cancel = new Button();
            button_Save = new Button();
            checkBox_Surface = new CheckBox();
            checkBox_OtherRawMat = new CheckBox();
            groupBox_OtherRawMat = new GroupBox();
            dataGridView_OtherRawMat = new Controls.DoubleBufferedDGV();
            Quantity = new DataGridViewTextBoxColumn();
            RawMat = new DataGridViewComboBoxColumn();
            comboBox_Extra = new ComboBox();
            checkBox_Extra = new CheckBox();
            comboBox_ProduitSimple = new ComboBox();
            checkBox_ProduitSimple = new CheckBox();
            groupBox_Surface.SuspendLayout();
            groupBox_OtherRawMat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_OtherRawMat).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(155, 15);
            label1.TabIndex = 0;
            label1.Text = "Fiche Recette pour l'article : ";
            // 
            // label_nameArticle
            // 
            label_nameArticle.AutoSize = true;
            label_nameArticle.Location = new Point(173, 19);
            label_nameArticle.Name = "label_nameArticle";
            label_nameArticle.Size = new Size(50, 15);
            label_nameArticle.TabIndex = 1;
            label_nameArticle.Text = "ARTICLE";
            // 
            // label_idArticle
            // 
            label_idArticle.AutoSize = true;
            label_idArticle.Location = new Point(416, -1);
            label_idArticle.Name = "label_idArticle";
            label_idArticle.Size = new Size(18, 15);
            label_idArticle.TabIndex = 2;
            label_idArticle.Text = "ID";
            label_idArticle.Visible = false;
            // 
            // groupBox_Surface
            // 
            groupBox_Surface.Controls.Add(comboBox_Surface);
            groupBox_Surface.Controls.Add(label_meters);
            groupBox_Surface.Controls.Add(label_dimension);
            groupBox_Surface.Location = new Point(12, 82);
            groupBox_Surface.Name = "groupBox_Surface";
            groupBox_Surface.Size = new Size(411, 90);
            groupBox_Surface.TabIndex = 3;
            groupBox_Surface.TabStop = false;
            // 
            // comboBox_Surface
            // 
            comboBox_Surface.FormattingEnabled = true;
            comboBox_Surface.Location = new Point(134, 54);
            comboBox_Surface.Name = "comboBox_Surface";
            comboBox_Surface.Size = new Size(262, 23);
            comboBox_Surface.TabIndex = 2;
            // 
            // label_meters
            // 
            label_meters.AutoSize = true;
            label_meters.Location = new Point(11, 57);
            label_meters.Name = "label_meters";
            label_meters.Size = new Size(117, 15);
            label_meters.TabIndex = 1;
            label_meters.Text = "soit 20983,2033m² de";
            // 
            // label_dimension
            // 
            label_dimension.AutoSize = true;
            label_dimension.Location = new Point(11, 25);
            label_dimension.Name = "label_dimension";
            label_dimension.Size = new Size(93, 15);
            label_dimension.TabIndex = 0;
            label_dimension.Text = "label_dimension";
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new Point(9, 450);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 23);
            button_Cancel.TabIndex = 4;
            button_Cancel.Text = "Annuler";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // button_Save
            // 
            button_Save.Location = new Point(345, 450);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(75, 23);
            button_Save.TabIndex = 5;
            button_Save.Text = "Sauver";
            button_Save.UseVisualStyleBackColor = true;
            button_Save.Click += button_Save_Click;
            // 
            // checkBox_Surface
            // 
            checkBox_Surface.AutoSize = true;
            checkBox_Surface.Checked = true;
            checkBox_Surface.CheckState = CheckState.Checked;
            checkBox_Surface.Location = new Point(12, 71);
            checkBox_Surface.Name = "checkBox_Surface";
            checkBox_Surface.Size = new Size(136, 19);
            checkBox_Surface.TabIndex = 6;
            checkBox_Surface.Text = "Contient une surface";
            checkBox_Surface.UseVisualStyleBackColor = true;
            checkBox_Surface.CheckedChanged += checkBox_Surface_CheckedChanged;
            // 
            // checkBox_OtherRawMat
            // 
            checkBox_OtherRawMat.AutoSize = true;
            checkBox_OtherRawMat.Checked = true;
            checkBox_OtherRawMat.CheckState = CheckState.Checked;
            checkBox_OtherRawMat.Location = new Point(12, 178);
            checkBox_OtherRawMat.Name = "checkBox_OtherRawMat";
            checkBox_OtherRawMat.Size = new Size(220, 19);
            checkBox_OtherRawMat.TabIndex = 7;
            checkBox_OtherRawMat.Text = "Contient d'autres matières premières";
            checkBox_OtherRawMat.UseVisualStyleBackColor = true;
            checkBox_OtherRawMat.CheckedChanged += checkBox_OtherRawMat_CheckedChanged;
            // 
            // groupBox_OtherRawMat
            // 
            groupBox_OtherRawMat.Controls.Add(dataGridView_OtherRawMat);
            groupBox_OtherRawMat.Location = new Point(12, 191);
            groupBox_OtherRawMat.Name = "groupBox_OtherRawMat";
            groupBox_OtherRawMat.Size = new Size(411, 219);
            groupBox_OtherRawMat.TabIndex = 8;
            groupBox_OtherRawMat.TabStop = false;
            // 
            // dataGridView_OtherRawMat
            // 
            dataGridView_OtherRawMat.AllowUserToResizeColumns = false;
            dataGridView_OtherRawMat.AllowUserToResizeRows = false;
            dataGridView_OtherRawMat.BackgroundColor = SystemColors.Control;
            dataGridView_OtherRawMat.BorderStyle = BorderStyle.None;
            dataGridView_OtherRawMat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_OtherRawMat.Columns.AddRange(new DataGridViewColumn[] { Quantity, RawMat });
            dataGridView_OtherRawMat.Dock = DockStyle.Fill;
            dataGridView_OtherRawMat.Location = new Point(3, 19);
            dataGridView_OtherRawMat.Name = "dataGridView_OtherRawMat";
            dataGridView_OtherRawMat.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView_OtherRawMat.RowTemplate.Height = 25;
            dataGridView_OtherRawMat.Size = new Size(405, 197);
            dataGridView_OtherRawMat.TabIndex = 0;
            dataGridView_OtherRawMat.DataError += dataGridView_OtherRawMat_DataError;
            // 
            // Quantity
            // 
            Quantity.DataPropertyName = "Name";
            Quantity.Name = "Quantity";
            Quantity.Resizable = DataGridViewTriState.True;
            // 
            // RawMat
            // 
            RawMat.FillWeight = 250F;
            RawMat.Name = "RawMat";
            RawMat.Resizable = DataGridViewTriState.True;
            RawMat.SortMode = DataGridViewColumnSortMode.Automatic;
            RawMat.Width = 250;
            // 
            // comboBox_Extra
            // 
            comboBox_Extra.FormattingEnabled = true;
            comboBox_Extra.Location = new Point(193, 416);
            comboBox_Extra.Name = "comboBox_Extra";
            comboBox_Extra.Size = new Size(215, 23);
            comboBox_Extra.TabIndex = 12;
            // 
            // checkBox_Extra
            // 
            checkBox_Extra.AutoSize = true;
            checkBox_Extra.Checked = true;
            checkBox_Extra.CheckState = CheckState.Checked;
            checkBox_Extra.Location = new Point(12, 416);
            checkBox_Extra.Name = "checkBox_Extra";
            checkBox_Extra.Size = new Size(151, 19);
            checkBox_Extra.TabIndex = 11;
            checkBox_Extra.Text = "Contient un coût extra :";
            checkBox_Extra.UseVisualStyleBackColor = true;
            checkBox_Extra.CheckedChanged += checkBox_Extra_CheckedChanged;
            // 
            // comboBox_ProduitSimple
            // 
            comboBox_ProduitSimple.FormattingEnabled = true;
            comboBox_ProduitSimple.Location = new Point(188, 44);
            comboBox_ProduitSimple.Name = "comboBox_ProduitSimple";
            comboBox_ProduitSimple.Size = new Size(215, 23);
            comboBox_ProduitSimple.TabIndex = 15;
            // 
            // checkBox_ProduitSimple
            // 
            checkBox_ProduitSimple.AutoSize = true;
            checkBox_ProduitSimple.Checked = true;
            checkBox_ProduitSimple.CheckState = CheckState.Checked;
            checkBox_ProduitSimple.Location = new Point(12, 46);
            checkBox_ProduitSimple.Name = "checkBox_ProduitSimple";
            checkBox_ProduitSimple.Size = new Size(170, 19);
            checkBox_ProduitSimple.TabIndex = 16;
            checkBox_ProduitSimple.Text = "Contient le produit simple :";
            checkBox_ProduitSimple.UseVisualStyleBackColor = true;
            checkBox_ProduitSimple.CheckedChanged += checkBox_ProduitSimple_CheckedChanged;
            // 
            // Recipe
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 485);
            Controls.Add(checkBox_ProduitSimple);
            Controls.Add(comboBox_ProduitSimple);
            Controls.Add(comboBox_Extra);
            Controls.Add(checkBox_Extra);
            Controls.Add(checkBox_OtherRawMat);
            Controls.Add(groupBox_OtherRawMat);
            Controls.Add(checkBox_Surface);
            Controls.Add(button_Save);
            Controls.Add(button_Cancel);
            Controls.Add(groupBox_Surface);
            Controls.Add(label_idArticle);
            Controls.Add(label_nameArticle);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Recipe";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recipe";
            groupBox_Surface.ResumeLayout(false);
            groupBox_Surface.PerformLayout();
            groupBox_OtherRawMat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_OtherRawMat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label_nameArticle;
        private Label label_idArticle;
        private GroupBox groupBox_Surface;
        private ComboBox comboBox_Surface;
        private Label label_meters;
        private Label label_dimension;
        private Button button_Cancel;
        private Button button_Save;
        private CheckBox checkBox_Surface;
        private CheckBox checkBox_OtherRawMat;
        private GroupBox groupBox_OtherRawMat;
        private Controls.DoubleBufferedDGV dataGridView_OtherRawMat;
        private DataGridViewTextBoxColumn Quantity;
        private DataGridViewComboBoxColumn RawMat;
        private ComboBox comboBox_Extra;
        private CheckBox checkBox_Extra;
        private Label label_ProduitSimple;
        private Label label_ContientSimple;
        private ComboBox comboBox_ProduitSimple;
        private CheckBox checkBox_ProduitSimple;
    }
}
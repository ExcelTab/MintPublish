namespace Mint.Controls
{
    partial class GroupListview
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Lbl_Header = new Label();
            Lbl_PlusMinus = new Label();
            dataGridView_List = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView_List).BeginInit();
            SuspendLayout();
            // 
            // Lbl_Header
            // 
            Lbl_Header.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Lbl_Header.BackColor = Color.FromArgb(215, 255, 153);
            Lbl_Header.BorderStyle = BorderStyle.FixedSingle;
            Lbl_Header.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Lbl_Header.Location = new Point(20, 0);
            Lbl_Header.Name = "Lbl_Header";
            Lbl_Header.Size = new Size(189, 23);
            Lbl_Header.TabIndex = 0;
            Lbl_Header.Text = "label1";
            Lbl_Header.Click += Lbl_Header_Click;
            // 
            // Lbl_PlusMinus
            // 
            Lbl_PlusMinus.BorderStyle = BorderStyle.FixedSingle;
            Lbl_PlusMinus.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Lbl_PlusMinus.Location = new Point(0, 0);
            Lbl_PlusMinus.Name = "Lbl_PlusMinus";
            Lbl_PlusMinus.Size = new Size(23, 23);
            Lbl_PlusMinus.TabIndex = 2;
            Lbl_PlusMinus.Text = "+";
            Lbl_PlusMinus.TextAlign = ContentAlignment.MiddleCenter;
            Lbl_PlusMinus.Click += Lbl_PlusMinus_Click;
            // 
            // dataGridView_List
            // 
            dataGridView_List.AllowUserToAddRows = false;
            dataGridView_List.AllowUserToDeleteRows = false;
            dataGridView_List.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.Green;
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridView_List.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView_List.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView_List.BackgroundColor = Color.White;
            dataGridView_List.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView_List.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.Green;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView_List.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView_List.Dock = DockStyle.Fill;
            dataGridView_List.GridColor = Color.Silver;
            dataGridView_List.Location = new Point(0, 0);
            dataGridView_List.Name = "dataGridView_List";
            dataGridView_List.ReadOnly = true;
            dataGridView_List.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView_List.RowHeadersVisible = false;
            dataGridView_List.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.SelectionBackColor = Color.Green;
            dataGridView_List.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView_List.RowTemplate.Height = 25;
            dataGridView_List.ScrollBars = ScrollBars.Horizontal;
            dataGridView_List.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_List.ShowEditingIcon = false;
            dataGridView_List.Size = new Size(212, 88);
            dataGridView_List.TabIndex = 3;
            // 
            // GroupListview
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(Lbl_PlusMinus);
            Controls.Add(Lbl_Header);
            Controls.Add(dataGridView_List);
            Name = "GroupListview";
            Size = new Size(212, 88);
            ((System.ComponentModel.ISupportInitialize)dataGridView_List).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label Lbl_Header;
        private Label Lbl_PlusMinus;
        private DataGridView dataGridView_List;
    }
}

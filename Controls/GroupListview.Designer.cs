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
            Lbl_Header = new Label();
            Lvi_List = new DoubleBufferedListView();
            Lbl_PlusMinus = new Label();
            SuspendLayout();
            // 
            // Lbl_Header
            // 
            Lbl_Header.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Lbl_Header.BackColor = Color.FromArgb(215, 255, 153);
            Lbl_Header.BorderStyle = BorderStyle.FixedSingle;
            Lbl_Header.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Lbl_Header.Location = new Point(22, 0);
            Lbl_Header.Name = "Lbl_Header";
            Lbl_Header.Size = new Size(218, 23);
            Lbl_Header.TabIndex = 0;
            Lbl_Header.Text = "label1";
            // 
            // Lvi_List
            // 
            Lvi_List.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Lvi_List.CheckBoxes = true;
            Lvi_List.FullRowSelect = true;
            Lvi_List.GridLines = true;
            Lvi_List.Location = new Point(0, 23);
            Lvi_List.Name = "Lvi_List";
            Lvi_List.Scrollable = false;
            Lvi_List.Size = new Size(240, 53);
            Lvi_List.TabIndex = 1;
            Lvi_List.UseCompatibleStateImageBehavior = false;
            Lvi_List.View = View.Details;
            Lvi_List.ItemChecked += Lvi_List_ItemChecked;
            Lvi_List.ItemSelectionChanged += Lvi_List_ItemSelectionChanged;
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
            // GroupListview
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Lbl_PlusMinus);
            Controls.Add(Lvi_List);
            Controls.Add(Lbl_Header);
            Name = "GroupListview";
            Size = new Size(240, 74);
            ResumeLayout(false);
        }

        #endregion

        private Label Lbl_Header;
        private DoubleBufferedListView Lvi_List;
        private Label Lbl_PlusMinus;
    }
}

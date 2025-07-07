namespace Mint.Forms
{
    partial class ListTable
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
            Lbl_ListTitle = new Label();
            Lvi_List = new ListView();
            Trv_List = new TreeView();
            button_Cancel = new Button();
            button_OK = new Button();
            SuspendLayout();
            // 
            // Lbl_ListTitle
            // 
            Lbl_ListTitle.Location = new Point(0, 9);
            Lbl_ListTitle.Name = "Lbl_ListTitle";
            Lbl_ListTitle.Size = new Size(38, 21);
            Lbl_ListTitle.TabIndex = 0;
            Lbl_ListTitle.Text = "label1";
            Lbl_ListTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Lvi_List
            // 
            Lvi_List.FullRowSelect = true;
            Lvi_List.Location = new Point(0, 33);
            Lvi_List.MultiSelect = false;
            Lvi_List.Name = "Lvi_List";
            Lvi_List.Size = new Size(63, 346);
            Lvi_List.TabIndex = 1;
            Lvi_List.UseCompatibleStateImageBehavior = false;
            Lvi_List.SelectedIndexChanged += Liv_List_SelectedIndexChanged;
            // 
            // Trv_List
            // 
            Trv_List.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Trv_List.FullRowSelect = true;
            Trv_List.Location = new Point(0, 33);
            Trv_List.Name = "Trv_List";
            Trv_List.Size = new Size(63, 346);
            Trv_List.TabIndex = 2;
            Trv_List.AfterSelect += TreeView_AfterSelect;
            // 
            // button_Cancel
            // 
            button_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_Cancel.Location = new Point(5, 385);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(58, 23);
            button_Cancel.TabIndex = 3;
            button_Cancel.Text = "Annuler";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // button_OK
            // 
            button_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_OK.Location = new Point(1118, 385);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(56, 23);
            button_OK.TabIndex = 4;
            button_OK.Text = "Valider";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Visible = false;
            button_OK.Click += button_OK_Click;
            // 
            // ListTable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1186, 410);
            Controls.Add(button_OK);
            Controls.Add(button_Cancel);
            Controls.Add(Trv_List);
            Controls.Add(Lvi_List);
            Controls.Add(Lbl_ListTitle);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ListTable";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Label Lbl_ListTitle;
        private ListView Lvi_List;
        private TreeView Trv_List;
        private Button button_Cancel;
        private Button button_OK;
    }
}
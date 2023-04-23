namespace Mint.Controls
{
    partial class AppButton
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
            Pic_App = new PictureBox();
            Lab_App = new Label();
            ((System.ComponentModel.ISupportInitialize)Pic_App).BeginInit();
            SuspendLayout();
            // 
            // Pic_App
            // 
            Pic_App.BackgroundImageLayout = ImageLayout.None;
            Pic_App.Location = new Point(4, 4);
            Pic_App.Name = "Pic_App";
            Pic_App.Size = new Size(67, 69);
            Pic_App.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_App.TabIndex = 0;
            Pic_App.TabStop = false;
            Pic_App.Click += App_Click;
            // 
            // Lab_App
            // 
            Lab_App.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            Lab_App.Location = new Point(0, 76);
            Lab_App.Margin = new Padding(0);
            Lab_App.Name = "Lab_App";
            Lab_App.Size = new Size(77, 31);
            Lab_App.TabIndex = 1;
            Lab_App.Text = "label1";
            Lab_App.TextAlign = ContentAlignment.TopCenter;
            Lab_App.Click += App_Click;
            // 
            // AppButton
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Lab_App);
            Controls.Add(Pic_App);
            Name = "AppButton";
            Size = new Size(77, 102);
            ((System.ComponentModel.ISupportInitialize)Pic_App).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox Pic_App;
        private Label Lab_App;
    }
}

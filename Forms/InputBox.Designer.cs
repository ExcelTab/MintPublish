namespace Mint.Forms
{
    partial class InputBox
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
            button_Cancel = new Button();
            button_OK = new Button();
            label_Input = new Label();
            textBox_Input = new TextBox();
            SuspendLayout();
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new Point(12, 103);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 23);
            button_Cancel.TabIndex = 0;
            button_Cancel.Text = "Annuler";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // button_OK
            // 
            button_OK.Location = new Point(322, 103);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 23);
            button_OK.TabIndex = 1;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // label_Input
            // 
            label_Input.AutoSize = true;
            label_Input.Location = new Point(12, 10);
            label_Input.Name = "label_Input";
            label_Input.Size = new Size(38, 15);
            label_Input.TabIndex = 2;
            label_Input.Text = "label1";
            // 
            // textBox_Input
            // 
            textBox_Input.CausesValidation = false;
            textBox_Input.Location = new Point(12, 74);
            textBox_Input.Name = "textBox_Input";
            textBox_Input.Size = new Size(385, 23);
            textBox_Input.TabIndex = 0;
            textBox_Input.KeyPress += textBox_Input_KeyPress;
            // 
            // InputBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(403, 138);
            Controls.Add(textBox_Input);
            Controls.Add(label_Input);
            Controls.Add(button_OK);
            Controls.Add(button_Cancel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "InputBox";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Cancel;
        private Button button_OK;
        private Label label_Input;
        private TextBox textBox_Input;
    }
}
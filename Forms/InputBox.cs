using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mint.Forms
{
    public partial class InputBox : Form
    {
        public string UserInput { get; private set; }
        public string label_Title
        {
            get { return label_Input.Text; }
            set { label_Input.Text = value; }
        }

        public InputBox()
        {
            InitializeComponent();
            textBox_Input.Focus();
        }

        public void GiveFocus()
        {
            textBox_Input.Focus();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            UserInput = textBox_Input.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            //If the user pressed enter, simulate a click on the OK button
            if (e.KeyChar == (char)13)
            {
                UserInput = textBox_Input.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}

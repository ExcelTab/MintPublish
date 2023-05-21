using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mint
{
    public partial class BlackOverlay : Form
    {
        public BlackOverlay()
        {
            InitializeComponent();
            //Find the active Home.cs form and set it as the parent of this form, then set the location and size of this form to the location and size of the parent form
            Form parentForm = Application.OpenForms[0];
            this.Location = parentForm.Location;
            this.Size = parentForm.Size;


        }

        private void BlackOverlay_Load(object sender, EventArgs e)
        {

        }
    }
}

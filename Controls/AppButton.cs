using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mint.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Mint.Controls
{
    public partial class AppButton : UserControl
    {

        public delegate void WaitDelegate(bool wait);
        public static event WaitDelegate WaitUpdate;


        //Get Home form groupbox object for docking
        private MainForm For_Main;
        private string App_Name;

        public MainForm Parrent_Form
        {
            get { return For_Main; }
            set { For_Main = value; }
        }

        public AppButton()
        {
            InitializeComponent();
        }
        // Changing the App label
        public string Label
        {
            get { return Lab_App.Text; }
            set { Lab_App.Text = value; }
        }

        // Changing the App Image
        public Image Image
        {
            get { return Pic_App.Image; }
            set { Pic_App.Image = value; }
        }

        // Changing the App Name
        public string AppName
        {
            get { return App_Name; }
            set { App_Name = value; }
        }


        private async void App_Click(object sender, EventArgs e)
        {
            // Open the specific App
            System.Windows.Forms.GroupBox AppBox = (System.Windows.Forms.GroupBox)For_Main.Controls["Grp_Forms"];
            string formName = "Mint.Forms.App_" + App_Name;
            Type formType = Type.GetType(formName);
            if (formType != null)
            {
                // TODO: verify if an instance of the form is already open
                // and if so, bring it to the front
                await Task.Run(async () => { await LoadForm(AppBox,formType); });

            }
            else
            {
                // Handle case when form type is not found
                AppBox.Visible = false;
                MessageBox.Show("Cette application n'est pas encore disponible", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);      
            }
        }

        private async Task LoadForm(System.Windows.Forms.GroupBox AppBox,Type formType)
        {
            WaitUpdate?.Invoke(true);
            
            if (AppBox != null)
            {
                AppBox.Invoke((MethodInvoker)delegate {
                    Form form = (Form)Activator.CreateInstance(formType);
                    if (AppBox.Controls.Count > 0 && AppBox.Controls[0] is Form previousForm)
                    {
                        previousForm.Close();
                        previousForm.Dispose();
                    }

                    AppBox.Controls.Clear(); // Clear all current controls in the groupbox
                    form.TopLevel = false;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    AppBox.Controls.Add(form);
                    form.Show();
                    AppBox.Visible = true;
                });
            }
            WaitUpdate?.Invoke(false);
        }
    }
}

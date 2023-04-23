
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Mint.Controls;
using Mint.Properties;
using Mint.Code;
using System.Resources;

namespace Mint
{
    public partial class MainForm : Form
    {

        public string sLoggedAs;

        public MainForm()
        {
            InitializeComponent();

            // Load the last logged in User
            string User = Settings.Default.login;
            if (User == string.Empty)
            {
                Lbl_LoggedAs.Text = "Not logged in";
            }
            else
            {
                Change_User();
            }
        }



        private void Open_Login_Form(object sender, EventArgs e)
        {
            //Open the login form and dim the main form
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Size = this.Size;
            blackOverlay.Location = this.Location;
            blackOverlay.Show();

            Login login = new Login();
            login.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            this.Show();
            //TODO : capter si la personne a mis CANCEL sur le login et ne pas lancer ChangeUser
            if (Settings.Default.login != null)
            {
                try
                {
                    Change_User();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while changing user: " + ex.Message);
                }
            }


        }

        private void Change_User()
        {
            //Check if the user is new or not
            if (Settings.Default.login != Lbl_LoggedAs.Text)
            {
                //Change the label on the top right button
                Lbl_LoggedAs.Text = Settings.Default.login;
                ToolStripMenuItem_Disconnect.Enabled = true;

                //Load the apps

                //Split the mods setting string into an array delimited by a semicolumn
                string sMods = Settings.Default.mods;

                // Split the string into a list of strings
                List<string> sModList = sMods.Split(';').ToList();

                // Clear all controls of the Grp_Apps group box
                Grp_Apps.Controls.Clear();

                // Define the x and y coordinates of the first button
                int x = 10;
                int y = 20;

                // Loop through the list and add a label control for each string
                foreach (string sMod in sModList)
                {
                    // Create a new instance of the AppButton control
                    var appButton = new AppButton();

                    // Set the MainForm property to the Home form
                    appButton.Parrent_Form = this;

                    //Locate the correct row in the ModLibrary DataTable based on the sMod value
                    DataRow[] ModRow = Mods.ModLibrary.Select("Name = '" + sMod + "'");

                    // Get Label and Image from the Resources
                    ResourceManager rm = Resources.ResourceManager;
                    appButton.Label = ModRow[0]["Label"].ToString();
                    appButton.AppName = ModRow[0]["FormName"].ToString();
                    Image ModImage = (Image)rm.GetObject(ModRow[0]["Image"].ToString());
                    appButton.Image = ModImage;

                    // Set the location of the control within the group box
                    appButton.Location = new Point(x, y);

                    // Add the control to the Grp_Apps group box
                    Grp_Apps.Controls.Add(appButton);

                    if (x == 10)
                    {
                        x = 10 + appButton.Width + 10;
                    }
                    else
                    {
                        x = 10;
                        y += appButton.Height + 10;
                    }
                    // Adjust the y coordinate for the next button
                }
                //Load default menu
                this.Grp_Forms.Visible = false;

                //Hide the connect buttons on main screen
                this.Lbl_NotConnected.Visible = false;
                this.Btn_HomeConnect.Visible = false;
            }

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Lbl_LoggedAs_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Disconnect_Click(object sender, EventArgs e)
        {
            //Disconnect the user
            Settings.Default.login = string.Empty;
            Settings.Default.password = string.Empty;
            Settings.Default.mods = string.Empty;
            Settings.Default.Save();
            Lbl_LoggedAs.Text = "Not logged in";
            ToolStripMenuItem_Disconnect.Enabled = false;
            //Load default menu
            this.Grp_Forms.Visible = false;
            Grp_Apps.Controls.Clear();
            //Show the connect buttons on main screen
            this.Lbl_NotConnected.Visible = true;
            this.Btn_HomeConnect.Visible = true;
        }
    }
}
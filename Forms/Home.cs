
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
using System.Reflection;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Mint.Forms;

namespace Mint
{
    public partial class MainForm : Form
    {
        public string sLoggedAs;
        // Declare a Timer object at the class level




        public MainForm()
        {
            InitializeComponent();
            Load_Version();
            Database.StatusUpdate += StatusUpdateHandler;
            Print.StatusUpdate += StatusUpdateHandler;
            App_Orders.StatusUpdate += StatusUpdateHandler;
            App_Orders.WaitUpdate += WaitUpdateHandler;
            App_Products.StatusUpdate += StatusUpdateHandler;
            App_Reports.StatusUpdate += StatusUpdateHandler;
            AppButton.WaitUpdate += WaitUpdateHandler;

            // Load the last logged in User
            string User = Settings.Default.login;
            if (User == string.Empty) { Lbl_LoggedAs.Text = "Not logged in"; }
            else { Change_User(true); }

            // Update from Buffer
            UpdateDatabases();

        }

        private void StatusUpdateHandler(string status)
        {
            if (label_status.InvokeRequired)
            {
                // If called from a different thread, use Invoke to update UI
                label_status.Invoke(new MethodInvoker(delegate
                {
                    if (string.IsNullOrEmpty(status))
                    {
                        if (label_status.Visible)
                        {
                            label_status.Visible = false;
                        }
                    }
                    else
                    {
                        if (!label_status.Visible)
                        {
                            label_status.Visible = true;
                        }
                        label_status.Text = status;
                    }
                }));
            }
            else
            {
                // If called from the UI thread, update directly
                if (string.IsNullOrEmpty(status))
                {
                    if (label_status.Visible)
                    {
                        label_status.Visible = false;
                    }
                }
                else
                {
                    if (!label_status.Visible)
                    {
                        label_status.Visible = true;
                    }
                    label_status.Text = status;
                }
            }
        }
        private void WaitUpdateHandler(bool wait)
        {
            if (pictureBox_AppLoading.InvokeRequired)
            {
                // If called from a different thread, use Invoke to update UI
                pictureBox_AppLoading.Invoke(new MethodInvoker(delegate
                {
                    if (wait == false)
                    {
                        if (pictureBox_AppLoading.Visible)
                        {
                            pictureBox_AppLoading.Visible = false;
                        }
                    }
                    else
                    {
                        if (!pictureBox_AppLoading.Visible)
                        {
                            pictureBox_AppLoading.BringToFront();
                            pictureBox_AppLoading.Visible = true;
                        }
                    }
                }));
            }
            else
            {
                // If called from the UI thread, update directly
                if (wait == false)
                {
                    if (pictureBox_AppLoading.Visible)
                    {
                        pictureBox_AppLoading.Visible = false;
                    }
                }
                else
                {
                    if (!pictureBox_AppLoading.Visible)
                    {
                        pictureBox_AppLoading.BringToFront();
                        pictureBox_AppLoading.Visible = true;
                    }
                }
            }
        }


        private void Open_Login_Form(object sender, EventArgs e)
        {
            //Open the login form and dim the main form
            BlackOverlay blackOverlay = new BlackOverlay();
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

        private void Change_User(bool forceReload = false)
        {
            //Check if the user is new or not
            if (Settings.Default.login != Lbl_LoggedAs.Text || forceReload == true)
            {
                //Change the label on the top right button
                Lbl_LoggedAs.Text = Settings.Default.login;
                ToolStripMenuItem_Disconnect.Enabled = true;

                //Load the apps

                //Split the mods setting string into an array delimited by a semicolumn
                if (forceReload == true) { Reload_Mods(); }
                string sMods = Settings.Default.mods;

                // Split the string into a list of strings
                List<string> sModList = null;
                if (sMods.Contains(";")) { sModList = sMods.Split(';').ToList(); }
                else if (sMods.Contains(",")) { sModList = sMods.Split(',').ToList(); }
                else { sModList = new List<string> { sMods }; }
                
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
                    //TODO: check if the mod still exists and if not, set the default logon
                    try
                    {
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please reconnect to the app: " + ex.Message);
                    }


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

        private void Load_Version()
        {

            // Get the ClickOnce deployment version, if live
            string clickOnceVersion = Application.ProductVersion;

            //TODO  : fonctionnerait en .Net 7 ?
            clickOnceVersion = Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion");
            if (clickOnceVersion != null) { Lbl_Version.Text = "Version : " + clickOnceVersion.ToString().Replace("1.1.",""); }
            //Lbl_Version.Text = "Version alpha: 4.02.10";
        }

        private void Lbl_Version_Click(object sender, EventArgs e)
        {

        }

        private async void UpdateDatabases()
        {
            try
            {
                //Load from buffer to main
                await Task.Run(async () => { await Database.LoadDataFromBufferToMain(true); });

                //Store the id_article to increase performance
                await Task.Run(async () => { await App_Orders.StoreIDArticleLocally(); });

                //Consume the rawmat and store local costs of production
                await Task.Run(async () => { await App_Orders.ConsumeRawMatsAndStoreCost(); });

                //Change to "livré mkp" the old orders
                await Task.Run(async () => { await App_Orders.UpdateOldOrderStates(); });

                //If the TBL_PRODUCT table last transfer date is more than 15 days, then launch the CompareProductList sub 
                String lastTransferDate  = Database.GetFieldFromField("CATALOG_TABLES", "table_name", "TBL_PRODUCT", "last_transfer_date");
                if (!string.IsNullOrEmpty(lastTransferDate))
                {
                    DateTime lastTransferDateDT = Convert.ToDateTime(lastTransferDate);
                    if (DateTime.Now.Subtract(lastTransferDateDT).TotalDays > 15)
                    {
                        await Task.Run(async () => { await App_Products.CompareProductLists(); });
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void changeEmailSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open the login form and dim the main form
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            EmailConfig emailConfig = new EmailConfig();
            emailConfig.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            this.Show();


        }

        private void Reload_Mods()
        {
            //Verify if the User exists in the TBL_USER sql table
            string sCommand = "SELECT * FROM TBL_USERS WHERE login = @login AND password = @password";

            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                SqlCommand command = new SqlCommand(sCommand, connection);
                command.Parameters.AddWithValue("@login", Settings.Default.login);
                command.Parameters.AddWithValue("@password", Settings.Default.password);

                // Try to open the connection, but catch any exceptions
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    // TODO : si un problème de sauvegarde de la clé produit, il ne sait pas se connecter. Renvoyer le prompt clé produit
                    MessageBox.Show("La connection à la base de donnée est impossible :  " + ex.Message, "Erreur de connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    // Move to the first (and only) row in the result set
                    reader.Read();
                    Properties.Settings.Default.mods = reader["mods"].ToString();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    // user does not exist in TBL_USER, show error message
                    MessageBox.Show("Invalid login or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                reader.Close();
            }
        }
    }
}
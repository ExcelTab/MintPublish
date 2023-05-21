using System.Data.SqlClient;
using Mint.Code;

namespace Mint
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();

            // Set the placeholder text for the TB_Login control
            TB_Login.Text = "Enter your login";
            // Set the text color to gray to indicate that it's a placeholder
            TB_Login.ForeColor = Color.Gray;

            // Set the placeholder text for the TB_Password control
            TB_Password.Text = "Enter your password";
            // Set the text color to gray to indicate that it's a placeholder
            TB_Password.ForeColor = Color.Gray;

        }

        private void TB_Password_Enter(object sender, EventArgs e)
        {
            // Clear the TB_Password control if it still contains the placeholder text
            if (TB_Password.Text == "Enter your password")
            {
                TB_Password.Text = "";
                TB_Password.ForeColor = Color.Black;
            }
        }

        private void TB_Password_Leave(object sender, EventArgs e)
        {
            // Add back the placeholder text if the TB_Password control is empty
            if (TB_Password.Text == "")
            {
                TB_Password.Text = "Enter your password";
                TB_Password.ForeColor = Color.Gray;
            }
        }


        private void TB_Login_Enter(object sender, EventArgs e)
        {
            // Clear the TB_Login control if it still contains the placeholder text
            if (TB_Login.Text == "Enter your login")
            {
                TB_Login.Text = "";
                TB_Login.ForeColor = Color.Black;
            }
        }

        private void TB_Login_Leave(object sender, EventArgs e)
        {
            // Add back the placeholder text if the TB_Login control is empty
            if (TB_Login.Text == "")
            {
                TB_Login.Text = "Enter your username";
                TB_Login.ForeColor = Color.Gray;
            }
        }


        private void BtnLoginOK_Click(object sender, EventArgs e)
        {
            //Verify if the User exists in the TBL_USER sql table
            string sCommand = "SELECT * FROM TBL_USERS WHERE Login = @login AND Password = @password";

            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                SqlCommand command = new SqlCommand(sCommand, connection);
                command.Parameters.AddWithValue("@login", TB_Login.Text);
                command.Parameters.AddWithValue("@password", TB_Password.Text);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    // Move to the first (and only) row in the result set
                    reader.Read();

                    // user exists in TBL_USER, proceed with login
                    //Save Login and Password in the settings
                    Properties.Settings.Default.login = this.TB_Login.Text;
                    Properties.Settings.Default.password = this.TB_Password.Text;
                    //Load the Mods column of the Reader into the settings
                    Properties.Settings.Default.mods = reader["Mods"].ToString();
                    Properties.Settings.Default.Save();

                    //TODO: Figure out how to hide the login form without it hiding the main form
                    //Hides the login form
                    this.Close();

                }
                else
                {
                    // user does not exist in TBL_USER, show error message
                    MessageBox.Show("Invalid login or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                reader.Close();
            }
        }
        private void BtnLoginCancel_Click(object sender, EventArgs e)
        {
            //Close the login form
            this.Close();
        }
    }
}

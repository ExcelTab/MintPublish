using Mint.Code;
using Mint.Properties;
using System;

namespace Mint
{
    public partial class EmailConfig : Form
    {
        public EmailConfig()
        {
            InitializeComponent();

            if (Properties.Settings.Default.emailUser != "")
            {
                TB_EmailUser.Text = Properties.Settings.Default.emailUser;
                TB_EmailPassword.Text = Properties.Settings.Default.emailPassword;
                TB_EmailSMTP.Text = Properties.Settings.Default.emailSMTP;
                TB_EmailPort.Text = Properties.Settings.Default.emailPort;
            }

        }

        private void BtnEmailOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.emailUser = TB_EmailUser.Text;
            Properties.Settings.Default.emailPassword = TB_EmailPassword.Text;
            Properties.Settings.Default.emailSMTP = TB_EmailSMTP.Text;
            Properties.Settings.Default.emailPort = TB_EmailPort.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
        private void BtnEmailCancel_Click(object sender, EventArgs e)
        {
            //Close the login form
            this.Close();
        }
    }
}

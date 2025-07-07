using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Mail;


namespace Mint.Code
{
    internal class Mails
    {
        public static string SendMail(string sTo, string sSubject, string sBody, string sCC = "", DataTable dt = null, string attachment = "", bool displayFirst = false)
        {
            //Check if mail settings are configured
            if (Properties.Settings.Default.emailUser == "")
            {
                //Open the login form and dim the main form
                BlackOverlay blackOverlay = new BlackOverlay();
                blackOverlay.Show();

                EmailConfig emailConfig = new EmailConfig();
                emailConfig.ShowDialog();

                //Once the login form is closed
                blackOverlay.Close();

                if (Properties.Settings.Default.emailUser == "")
                {
                    //User did not fill the email settings
                    return "Pas d'information sur le compte à utiliser pour envoyer l'email";
                }
            }

            //Configure les paramètres d'envoi de courrier électronique
            // Récupère les données de la DataTable sous forme de texte ou de tableau HTML

            string tableData = "";
            if (dt == null)
            {
                tableData = "";
            }
            else
            {
                tableData = Mint.Code.BasicFunctions.ConvertDataTableToHtml(dt);
            }
            
            try
            {
                SmtpClient smtpClient = new SmtpClient(Properties.Settings.Default.emailSMTP);
                smtpClient.Port = Convert.ToInt32(Properties.Settings.Default.emailPort);
                smtpClient.Credentials = new NetworkCredential(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailPassword);
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Properties.Settings.Default.emailUser, "Mint - Hexoa");
                //Modification 17 JUN 2025 : better managing of multiple destinataires
                // Split, trim, deduplicate, and add recipients
                var recipients = sTo
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(email => email.Trim())
                    .Where(email => !string.IsNullOrWhiteSpace(email))
                    .Distinct(StringComparer.OrdinalIgnoreCase);

                foreach (var recipient in recipients)
                {
                    mail.To.Add(recipient);
                }
                mail.Subject = sSubject;
                mail.Body = sBody + "\n\n" + tableData; // Ajoute le contenu de la DataTable ici
                mail.IsBodyHtml = true; // if table is in est en HTML

                // Affiche le courrier électronique au lieu de l'envoyer directement
                if (displayFirst)
                {
                    string sBodyWithoutHTML = mail.Body.Replace("<html><body>","").Replace("</body></html>", "").
                        Replace("<p>","\n").Replace("</p>","").
                        Replace("<table border='1'><tr>","Table \n").Replace("</table>", "").
                        Replace("<th>", "  |  ").Replace("</th>", "").
                        Replace("<tr>", "\n").Replace("</tr>", "").
                        Replace("<td>", "  |  ").Replace("</td>", "").
                        Replace("<li>","  -  ").Replace("</li>", "\n").
                        Replace("<ul>", "\n").Replace("</ul>", "\n");

                    string recipientList = string.Join("; ", recipients);
                    DialogResult result = MessageBox.Show($"Destinataire: {recipientList}\nSujet: {sSubject}\n\n{sBodyWithoutHTML}", "Voulez-vous envoyer cet email ?", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes)
                    {
                        return "";
                    }
                }
                smtpClient.Send(mail);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using Mint.Code;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Point = System.Drawing.Point;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace Mint.Forms
{
    public partial class App_Orders : Form
    {
        #region Global Events
        public delegate void StatusUpdateDelegate(string message);
        public static event StatusUpdateDelegate StatusUpdate;
        public delegate void WaitDelegate(bool wait);
        public static event WaitDelegate WaitUpdate;
        #endregion

        #region Local Variables
        DataTable dtOngoingDetails;
        DataTable dtPackages;
        int id_order;
        string selected_atelier;
        bool all_ateliers = false;
        bool homeNeedsUpdate = true;
        bool listNeedsUpdate = true;
        #endregion


        public App_Orders()
        {
            InitializeComponent();
            //Refresh_Orders() ne pas lancer pour que le module se charge plus vite

            //Voir les permissions du User
            LoadAteliersForUser();
            LoadSettings();

            RefreshHomeView();

        }

        #region Procedures to launch on start
        private void LoadAteliersForUser()
        {
            string connectedUser = Properties.Settings.Default.login;
            string queryUser = "SELECT vue_atelier FROM TBL_USERS WHERE login = '" + connectedUser + "'";
            string permission = "Aucun";
            DataTable dtUser = Database.LoadData(queryUser);
            if (dtUser.Rows.Count > 0)
            {
                permission = dtUser.Rows[0]["Vue_Atelier"].ToString();
            }

            string queryAteliers = "SELECT * FROM TBL_ATELIER Where is_active = 1";
            if (permission == "Tous")
            {
                all_ateliers = true;
                radioButton_All.Visible = true;
                radioButton_Cancel.Visible = true;
                button_CancelOF.Visible = true;
                button_ModifyOF.Visible = true;

            }
            else
            {
                all_ateliers = false;
                radioButton_All.Visible = false;
                button_CancelOF.Visible = false;
                button_ModifyOF.Visible = false;
                radioButton_Cancel.Visible = false;
                queryAteliers += " and id_atelier = " + permission;
            }
            //Load the Combobox Atelier with the list of Ateliers
            queryAteliers += " Order By sort_order asc";
            DataTable dtAteliers = Database.LoadData(queryAteliers);

            // Désactiver l'événement SelectedIndexChanged
            comboBox_Atelier.SelectedIndexChanged -= comboBox_Atelier_SelectedIndexChanged;

            comboBox_Atelier.DataSource = dtAteliers;
            comboBox_Atelier.DisplayMember = "name";
            comboBox_Atelier.ValueMember = "id_atelier";
            //By default, select the first value of the combobox
            comboBox_Atelier.SelectedIndex = 0;
            selected_atelier = dtAteliers.Rows[0]["name"].ToString();

            // Désactiver l'événement SelectedIndexChanged
            comboBox_Atelier.SelectedIndexChanged += comboBox_Atelier_SelectedIndexChanged;

        }
        private void LoadSettings()
        {
            //Load the settings from the App resources
            textBox_PathGLS.Text = Properties.Settings.Default.glsPath;
            textBox_OFTemplate.Text = Properties.Settings.Default.OFTemplatePath;
            textBox_Printer.Text = Properties.Settings.Default.printerPath;
        }
        #endregion

        #region Overall procedures used accross the App
        private void ChangeItemStatus(string id_order_detail, string id_sales_state, string id_order)
        {
            homeNeedsUpdate = true;
            listNeedsUpdate = true;
            //Change the id_sales_state of the line in TBL_SALES
            string query = "UPDATE TBL_SALES SET id_sales_state = " + id_sales_state + ", date_update = GETDATE() WHERE id_order_detail = " + id_order_detail;

            //Save this on the buffer and on the main
            Database.ExecuteQuery(query, null, true);
            Database.ExecuteQuery(query, null, false);

            //Add this change to TBL_SALES_CHANGE
            query = "INSERT INTO TBL_SALES_CHANGE (id_order_detail, id_sales_state, date_change, user_change, date_add) VALUES (" + id_order_detail + ", " + id_sales_state + ", GETDATE(),'" + Properties.Settings.Default.login + "', GETDATE())";
            Database.ExecuteQuery(query, null, true);
            //Database.ExecuteQuery(query, null, false); car les numéros Auto ID ne sont pas les mêmes

            //Change the order Status if everything is done
            UpdateOrderStatus(id_order);

        }
        private void UpdateOrderStatus(string id_order)
        {
            //Loop through all the id_order_details in TBL_SALES for that id_order
            string query = "SELECT * FROM ViewOrderSalesState WHERE id_order = " + id_order;
            DataTable dt = Database.LoadData(query, true);

            string current_order_state = string.Empty; // Default value if not found or null
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["id_order_state"] != DBNull.Value)
            {
                current_order_state = dt.Rows[0]["id_order_state"].ToString();
            }

            int i_exp = 0;
            int i_end = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["id_sales_state"].ToString() == "1") { i_exp++; }
                if (row["is_end_sales"].ToString() == "1") { i_end++; }
            }

            //Mettre à jour le statut de la commande
            int i_state = 0;
            if (i_end == dt.Rows.Count)
            {
                if (i_exp == dt.Rows.Count) { i_state = 4; } //Livraison en cours (sera marqué Livrée par Prestashop
                else if (i_exp == 0) { i_state = 6; } //Annulé
                else if (i_exp > 0) { i_state = 34; } //Livrée avec partie annulée
            }
            else
            {
                if (i_exp > 0) { i_state = 20; } //Livraison partielle
                else { i_state = 3; } //En cours de fabrication
            }


            //Check if the order is already in this state
            if (current_order_state != i_state.ToString())
            {
                //Change the id_order_state of the line in TBL_ORDER
                query = "UPDATE TBL_ORDER SET id_order_state = " + i_state.ToString() + ", date_update = GETDATE() WHERE id_order = " + id_order;

                //Save this on the buffer and on the main
                Database.ExecuteQuery(query, null, true);
                Database.ExecuteQuery(query, null, false);

                //Add this change to TBL_ORDER_CHANGE
                query = "INSERT INTO TBL_ORDER_CHANGE (id_order, id_order_state, date_change, id_website, date_add) VALUES (" + id_order + ", " + i_state.ToString() + ", GETDATE(),'" + Properties.Settings.Default.login + "', GETDATE())";
                Database.ExecuteQuery(query, null, true);
                //Database.ExecuteQuery(query, null, false); car les numéros auto ID ne sont pas les memes
            }
        }
        private async void RefreshActiveView()
        {
            //Refresh the table if the second tab is selected
            if (tabControl_Orders.SelectedTab == tabPage_List)
            {
                Refresh_Orders();
            }
            if (tabControl_Orders.SelectedTab == tabPage_Home)
            {
                RefreshHomeView();
            }
            if (tabControl_Orders.SelectedTab == tabPage_Scan)
            {
                textBox_Scan.Focus();
            }
        }
        private void tabControl_Orders_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshActiveView();
        }
        private void comboBox_Atelier_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_atelier = comboBox_Atelier.Text;
            listNeedsUpdate = true;
            homeNeedsUpdate = true;
            RefreshActiveView();
        }
        private void radioButton_All_CheckedChanged(object sender, EventArgs e)
        {
            listNeedsUpdate = true;
            homeNeedsUpdate = true;
            RefreshActiveView();
        }
        private void radioButton_Atelier_CheckedChanged(object sender, EventArgs e)
        {
            listNeedsUpdate = true;
            homeNeedsUpdate = true;
            RefreshActiveView();
        }
        #endregion

        #region Procedures related to a tab or a button
        private async Task RefreshHomeView()
        {
            //Only do if the tab is selected
            if (tabControl_Orders.SelectedTab != tabPage_Home) { return; }
            if (!homeNeedsUpdate) { return; }
            this.Cursor = Cursors.WaitCursor;

            //Pie chart
            chart_StatusOngoing.Series[0].Points.Clear();
            string query = "SELECT Statut, Sum(Total) as Totals FROM SummaryOngoingStatus";
            if (radioButton_Atelier.Checked == true) { query += " where atelier = '" + selected_atelier + "'"; }
            query += " GROUP BY Statut Order by Statut Asc";
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataPoint point = new DataPoint();
                    point.AxisLabel = row["Statut"].ToString();
                    point.YValues = new double[] { Convert.ToDouble(row["Totals"]) };
                    chart_StatusOngoing.Series[0].Points.Add(point);
                }
            }

            //20 Rolling Weeks
            chart_20LastWeeks.Series[0].Points.Clear();
            query = "SELECT Semaine, Sum(Total) as Totals FROM Summary20LastWeeks";
            if (radioButton_Atelier.Checked == true) { query += " where Atelier = '" + selected_atelier + "'"; }
            query += " GROUP BY Semaine Order By Semaine Asc";
            dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataPoint point = new DataPoint();
                    point.AxisLabel = row["Semaine"].ToString();
                    point.YValues = new double[] { Convert.ToDouble(row["Totals"]) };
                    chart_20LastWeeks.Series[0].Points.Add(point);
                }
            }

            //Alert on no matching atelier for ref (only for admins)
            int countAlarm = 0;
            string alertMessage = "";
            if (all_ateliers == true)
            {
                //Mapping d'articles
                query = "SELECT * FROM Alarm_ArticleMappingIssue";
                dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    countAlarm++;
                    alertMessage += "-----------------------------------" + Environment.NewLine;
                    alertMessage = "Alerte : " + dt.Rows.Count + " références non liées à un article : ";
                    foreach (DataRow row in dt.Rows)
                    {
                        alertMessage += Environment.NewLine + "      ~ " + row["h_reference"].ToString();
                    }
                    alertMessage += Environment.NewLine + Environment.NewLine;
                }

                //Stock Raw Mat
                query = "SELECT * FROM Alarm_Stock";
                dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    countAlarm++;
                    alertMessage += "-----------------------------------" + Environment.NewLine;
                    alertMessage += "Alerte : " + dt.Rows.Count + " matières premières sous leur stock d'alerte : ";
                    foreach (DataRow row in dt.Rows)
                    {
                        alertMessage += Environment.NewLine + "      ~ ID:" + row["id_rawmat"].ToString() + " - " + row["name"].ToString() + "  [Stock : " + row["stock"] + " | Alerte : " + row["alert"] + "]";
                    }
                    alertMessage += Environment.NewLine + Environment.NewLine;
                }

                //Stock Finished Goods
                query = "SELECT * FROM Alarm_StockNegoce";
                dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    countAlarm++;
                    alertMessage += "-----------------------------------" + Environment.NewLine;
                    alertMessage += "Alerte : " + dt.Rows.Count + " références sous leur stock d'alerte : ";
                    foreach (DataRow row in dt.Rows)
                    {
                        alertMessage += Environment.NewLine + "      ~ " + row["name"].ToString() + "  [Stock : " + row["stock"] + " | Alerte : " + row["alert"] + "]";
                    }
                    alertMessage += Environment.NewLine + Environment.NewLine;
                }
                if (countAlarm > 0)
                {
                    alertMessage += "-----------------------------------";
                    textBox_Alarms.Text = alertMessage;
                    textBox_Alarms.BackColor = Color.Orange;
                }
                else
                {
                    textBox_Alarms.Text = "Aucune alerte à signaler";
                    textBox_Alarms.BackColor = Color.Gainsboro;
                }
            }
            splitContainer1.Panel2.BackColor = textBox_Alarms.BackColor;
            homeNeedsUpdate = false;
            this.Cursor = Cursors.Default;
        }
        private void SendMailToAteliers()
        {
            //Prendre tout ce qui est en OF imprimé, et envoyer un mail à l'atelier concerné
            string query = "SELECT name, atelier_email from TBL_ATELIER where atelier_email is not null";
            if (radioButton_Atelier.Checked == true) { query += " and name = '" + selected_atelier + "'"; }
            DataTable dtAteliers = Database.LoadData(query);

            foreach (DataRow row in dtAteliers.Rows)
            {
                query = "SELECT * FROM ViewOngoingOrdersMail WHERE atelier = '" + row["name"] + "'";
                DataTable dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    string sSubject = "Commandes à préparer pour Hexoa";
                    string sTo = row["atelier_email"].ToString();
                    string sBody = "<html><body><p>Bonjour,</p><p>Vous trouverez ci-dessous la liste des commandes à préparer.</p><p>Merci</p><p></p><p>Hexoa";

                    string envoiMail = Mint.Code.Mails.SendMail(sTo, sSubject, sBody, "", dt, "");
                    if (envoiMail != "")
                    {
                        MessageBox.Show("Erreur lors de l'envoi du mail à l'atelier " + row["name"] + " : " + envoiMail);
                    }
                    else
                    {
                        //Changer les statuts pour ne pas renvoyer la même chose
                        foreach (DataRow idUniqueRow in dt.Rows)
                        {
                            string id_order = idUniqueRow["id_order"].ToString();
                            string id_unique = idUniqueRow["id_unique"].ToString();
                            ChangeItemStatus(id_unique, "4", id_order); //4 = En cours de production
                        }

                    }
                }
            }
        }
        private void Refresh_Orders()
        {
            //Only do if the tab is selected
            //if (tabControl_Orders.SelectedTab != tabPage_List) { return; }
            if (!listNeedsUpdate) { return; }

            this.Cursor = Cursors.WaitCursor;
            label_DatePrestaTBLORDER.Text = "connecting...";

            // Load all the ongoing headers
            string queryOngoing = "SELECT * FROM ViewOngoingOrders where (end_order = 0";
            if (checkBox_ShowComplete.Checked) { queryOngoing += " or end_order = 1"; }
            if (radioButton_Atelier.Checked) { queryOngoing += " ) and (Atelier = '" + selected_atelier + "'"; }
            queryOngoing += ") order by ID desc";
            DataTable dtOngoing = Database.LoadData(queryOngoing);
            //remove the end_order column
            dtOngoing.Columns.Remove("end_order");
            if (radioButton_Atelier.Checked)
            {
                dtOngoing.Columns.Remove("Atelier");
            }
            else
            {
                foreach (DataRow row in dtOngoing.Rows)
                {
                    if (row["atelier"] == DBNull.Value)
                    {
                        row["atelier"] = "Atelier inconnu";
                    }
                }
            }


            //Merge two rows if they have the same id_order, but add all Ateliers to the same cell
            if (radioButton_All.Checked && dtOngoing.Rows.Count > 0)
            {
                int rowCount = dtOngoing.Rows.Count;
                int currentRow = 0;

                while (currentRow < rowCount - 1)
                {
                    DataRow currentRowData = dtOngoing.Rows[currentRow];
                    DataRow nextRowData = dtOngoing.Rows[currentRow + 1];

                    int currentIDOrder = currentRowData.Field<int>("ID");
                    int nextIDOrder = nextRowData.Field<int>("ID");

                    if (currentIDOrder == nextIDOrder)
                    {
                        // Fusionnez l'atelier en ajoutant une virgule
                        currentRowData["Atelier"] += ", " + nextRowData.Field<string>("Atelier");

                        // Sommez la quantité en double
                        currentRowData["Articles"] = currentRowData.Field<double>("Articles") + nextRowData.Field<double>("Articles");

                        // Supprimez la ligne suivante
                        dtOngoing.Rows.RemoveAt(currentRow + 1);
                        rowCount--; // Réduisez le nombre total de lignes
                    }
                    else
                    {
                        // Passez à la ligne suivante
                        currentRow++;
                    }
                }
            }


            //Load all the details into a datasource available for details
            //TODO : peut etre une meilleure manière de charger la datatable en une fois ?
            string queryOngoingDetails = "SELECT * FROM ViewOngoingOrdersDetails where (end_order = 0";
            if (checkBox_ShowComplete.Checked == true) { queryOngoingDetails += " or end_order = 1"; }
            if (radioButton_Atelier.Checked == true) { queryOngoingDetails += " ) and (atelier = '" + selected_atelier + "'"; }
            queryOngoingDetails += ")";
            dtOngoingDetails = Database.LoadData(queryOngoingDetails);
            foreach (DataRow row in dtOngoingDetails.Rows)
            {
                if (row["atelier"] == DBNull.Value)
                {
                    row["atelier"] = "Atelier inconnu";
                }
            }
            dataGridView_Ongoing.DataSource = dtOngoing;

            //Load all the package ids into a datatable available for details
            string queryOngoingPackages = "SELECT * FROM TBL_EXPEDITION";
            dtPackages = Database.LoadData(queryOngoingPackages);

            //Place the Atelier column in third position
            if (dataGridView_Ongoing.Columns.Contains("Atelier"))
            {
                dataGridView_Ongoing.Columns["Atelier"].DisplayIndex = 2;
            }

            //Update the date of prestashop
            string lastSynchro = Database.GetFieldFromField("CATALOG_TABLES", "Table_Name", "TBL_ORDER", "Last_Transfer_Date");
            //the lastYnchro string contains a date in server time. Convert to local time
            DateTime lastSynchroDate = DateTime.Parse(lastSynchro);
            lastSynchro = lastSynchroDate.ToLocalTime().ToString();
            label_DatePrestaTBLORDER.Text = lastSynchro;

            listNeedsUpdate = false;
            this.Cursor = Cursors.Default;
        }
        private void Scan_Clicked()
        {

            string textboxscan = textBox_Scan.Text;

            //Only do if textbox_Scan.text is not empty and is numeric
            if (textboxscan != "" && int.TryParse(textboxscan, out int n))
            {
                //numéro de commande correct
                id_order = Convert.ToInt32(textboxscan);

            }
            else
            {
                MessageBox.Show("Veuillez entrer un numéro de commande valide", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox_Scan.Focus();
                return;
            }

            //Display OF or change commandStatus
            if (radioButton_PrintOF.Checked == true)
            {
                //Display OF
                string atelier = "";
                if (radioButton_Atelier.Checked == true) { atelier = selected_atelier; }
                Mint.Code.PDF.CreatePDF("OF", Properties.Settings.Default.OFTemplatePath, textboxscan, atelier);
            }
            else if (radioButton_Cancel.Checked == true)
            {
                //Cancel the OF
                CancelOF(textboxscan);
            }
            else if (radioButton_Colis.Checked == true)
            {
                PopUpColis();

            }
            else
            {
                //Change the status of the command
                List<string> id_unique = new List<string>();
                string query = "SELECT id_unique, h_reference, [desc support], [desc dimension], statut_partiel FROM ViewOngoingOrdersDetails WHERE id_order = " + textboxscan;
                if (radioButton_Atelier.Checked == true) { query += " and atelier = '" + selected_atelier + "'"; }

                DataTable dt = Database.LoadData(query);
                if (dt.Rows.Count == 0)
                {
                    string Message = "La commande n°" + textboxscan + " n'est pas une commande en cours";
                    if (radioButton_Atelier.Checked == true) { Message = Message + " pour l'atelier " + selected_atelier; }
                    MessageBox.Show(Message);
                    return;
                }
                else if (dt.Rows.Count == 1)
                {
                    id_unique.Add(dt.Rows[0]["id_unique"].ToString());
                }
                else if (checkBox_ScanAllOrder.Checked == true)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        id_unique.Add(row["id_unique"].ToString());
                    }
                }
                else
                {
                    //Only show list if more than one item
                    BlackOverlay blackOverlay = new BlackOverlay();
                    blackOverlay.Show();

                    ListTable list_Select = new ListTable();
                    list_Select.ViewMode = "ListView";
                    list_Select.MultiSelect = true;
                    list_Select.Load_List(query);
                    list_Select.Set_Title("Choix d'une partie ou de toute la commande");
                    list_Select.ShowDialog();
                    blackOverlay.Close();
                    if (list_Select.ID_Selected == null) { return; }
                    id_unique = list_Select.ID_Selected.Split(", ").ToList();
                }

                int i_sales_state = 0;
                if (radioButton_OFP.Checked == true) { i_sales_state = 3; }
                else if (radioButton_ongoing.Checked == true) { i_sales_state = 4; }
                else if (radioButton_prod.Checked == true) { i_sales_state = 5; }
                else if (radioButton_EXP.Checked == true) { i_sales_state = 1; }
                else
                {
                    return;
                }

                //Changer les statuts dans la database
                //Cast the textbox_Scan string to int and store it in int_order

                foreach (string id in id_unique)
                {
                    ChangeItemStatus(id, i_sales_state.ToString(), textboxscan);
                }

                //Etiquette GLS
                if (radioButton_EXP.Checked == true && checkBox_GLS.Checked == true) { EtiquetteGLS(string.Join(",", id_unique)); }

                //Etiquette GLS
                if (radioButton_EXP.Checked == true && checkBox_Package.Checked == true) { PopUpColis(); }


                //Add to the log label 
                string datenow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                string log = datenow + "   -   Commande n° " + textBox_Scan.Text + " marquée comme ";
                if (radioButton_OFP.Checked == true) { log = log + "[OF Imprimé]"; }
                else if (radioButton_ongoing.Checked == true) { log = log + "[En cours de production]"; }
                else if (radioButton_prod.Checked == true) { log = log + "[Produit]"; }
                else if (radioButton_EXP.Checked == true) { log = log + "[Expédié]"; }
                log = log + " par " + Properties.Settings.Default.login;
                label_Log.Text = log + "\n" + label_Log.Text;



            }
            //put cursor in the Textbox_scan
            textBox_Scan.Text = "";
            textBox_Scan.Focus();
        }
        private void CancelOF(string id_order)
        {
            //Ask the user to confirm through a message box, and only cancel the order if he clicks on "Yes"
            DialogResult dialogResult = MessageBox.Show("Êtes-vous sûr de vouloir annuler toute la commande n°" + id_order + " ?", "Annuler la commande", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //Cancel all the lines in TBL_SALES (id_sales_state = 2)
                string query = "UPDATE TBL_SALES SET id_sales_state = 2, date_update = GETDATE() WHERE id_order = " + id_order;
                Database.ExecuteQuery(query, null, true);
                Database.ExecuteQuery(query, null, false);

                UpdateOrderStatus(id_order);

                //Send an email to corresponding ateliers to warn what has changed
                query = @$"SELECT notif_email from TBL_ATELIER as A 
                    left join TBL_SALES as S on A.id_atelier = S.id_atelier 
                    where A.notif_email is not null and S.id_order = " + id_order;
                DataTable dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    string sSubject = $"Hexoa - Commande n°{id_order} annulée";
                    List<string> emailList = new List<string>();
                    foreach (DataRow emailRow in dt.Rows)
                    {
                        emailList.Add(emailRow["notif_email"].ToString());
                    }
                    string sTo = string.Join(";", emailList);
                    string sBody = $"<html>" +
                        $"<body><p>Bonjour," +
                        $"</p><p>Le client vient d’annuler la commande n° {id_order.ToString()}." +
                        $"</p><p>Merci de ne pas la produire et de nous confirmer la prise en compte de cette annulation par retour d’email" +
                        $"</p><p></p><p>Hexoa";



                    string envoiMail = Mint.Code.Mails.SendMail(sTo, sSubject, sBody, "", null, "", true);
                    if (envoiMail != "")
                    {
                        MessageBox.Show("Erreur lors de l'envoi du mail à l'atelier " + string.Join(";", emailList) + " : " + envoiMail);
                    }
                }

                //Refresh the view if we are on the "Ongoing" tab
                if (tabControl_Orders.SelectedTab == tabPage_List)
                {
                    listNeedsUpdate = true;
                    RefreshActiveView();
                }
                else if (tabControl_Orders.SelectedTab == tabPage_Scan)
                {
                    //Add to the log label 
                    string datenow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    string log = datenow + "   -   Commande n° " + textBox_Scan.Text + " marquée comme [Annulé]";
                    log = log + " par " + Properties.Settings.Default.login;
                    label_Log.Text = log + "\n" + label_Log.Text;
                }

            }
        }
        private void button_ModifyOF_Click(object sender, EventArgs e)
        {
            //Display a pop-up with all the columns of the TBL_ORDER and allows modification and save
            string query = "SELECT * FROM TBL_ORDER WHERE id_order = " + id_order.ToString();
            DataTable dt = Database.LoadData(query);
            //Check if the datatable is empty
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Aucun OF trouvé");
                return;
            }
            //Display the hidden pannel
            
            label_ModifyOF.Text = "Modification de la commande n°" + id_order.ToString();
            //Clear all the controls from the tableLayoutPannel_ModifyOF
            tableLayoutPanel_ModifyOF.Controls.Clear();
            //Loop through the columns of DT, and add a label and a textbox for each column
            tableLayoutPanel_ModifyOF.RowCount = dt.Columns.Count-1;
            tableLayoutPanel_ModifyOF.ColumnCount = 2;
            tableLayoutPanel_ModifyOF.ColumnStyles.Clear();
            tableLayoutPanel_ModifyOF.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel_ModifyOF.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataColumn column = dt.Columns[i];
                Label label = new Label
                {
                    Text = column.ColumnName,
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    TextAlign = ContentAlignment.MiddleRight
                };
                TextBox textBox = new TextBox
                {
                    Text = dt.Rows[0][column].ToString(),
                    Dock = DockStyle.Fill,
                    BackColor = Color.LightYellow
                };
                tableLayoutPanel_ModifyOF.Controls.Add(label, 0, i-1);
                tableLayoutPanel_ModifyOF.Controls.Add(textBox, 1, i-1);
            }
            //Delete the first row

            panel_ModifyOF.Visible = true;

        }
        private void button_CancelModifyOF_Click(object sender, EventArgs e)
        {
            panel_ModifyOF.Visible = false;
        }
        private void button_SaveModifyOF_Click(object sender, EventArgs e)
        {
            // Loop through the controls of the tableLayoutPannel_ModifyOF, and save the modified values in the database
            string query = "SELECT * FROM TBL_ORDER WHERE id_order = " + id_order.ToString();
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Aucun OF trouvé");
                return;
            }

            StringBuilder updateQuery = new StringBuilder("UPDATE TBL_ORDER SET ");
            bool hasChanges = false;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            //Create an array that stores the lable, the old value, and the new value
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DataTable dtChanges = new DataTable();
            dtChanges.Columns.Add("Elément", typeof(string));
            dtChanges.Columns.Add("Ancienne valeur", typeof(string));
            dtChanges.Columns.Add("Nouvelle valeur", typeof(string));
            foreach (Control control in tableLayoutPanel_ModifyOF.Controls)
            {
                if (control is Label label)
                {
                    // Get the name of the column
                    string columnName = label.Text;
                    // Get the value of the textbox
                    TextBox textBox = (TextBox)tableLayoutPanel_ModifyOF.GetControlFromPosition(1, tableLayoutPanel_ModifyOF.GetRow(label));
                    string newValue = textBox.Text;
                    object oldValue = dt.Rows[0][columnName];

                    // Compare the new value with the old value
                    if (!newValue.Equals(oldValue.ToString()))
                    {
                        // Add to the query
                        updateQuery.Append(columnName + " = @" + columnName + ", ");
                        parameters.Add("@" + columnName, Convert.ChangeType(newValue, oldValue.GetType()));
                        hasChanges = true;
                        // Add to the changes datatable
                        dtChanges.Rows.Add(columnName, oldValue, newValue);
                    }
                }
            }

            if (hasChanges)
            {
                // add the dateupdate clause
                updateQuery.Append("date_update = GETDATE()");
                // Add the where clause
                updateQuery.Append(" WHERE id_order = " + id_order.ToString());
                // Execute the query
                Database.ExecuteQuery(updateQuery.ToString().TrimEnd(',', ' '), parameters, true);
                Database.ExecuteQuery(updateQuery.ToString().TrimEnd(',', ' '), parameters, false);

                //Send an email to corresponding ateliers to warn what has changed
                query = @$"SELECT notif_email from TBL_ATELIER as A 
                    left join TBL_SALES as S on A.id_atelier = S.id_atelier 
                    where A.notif_email is not null and S.id_order = "+ id_order.ToString();
                dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    string sSubject = $"Hexoa - Commande n°{id_order.ToString()} modifiée";
                    List<string> emailList = new List<string>();
                    foreach (DataRow emailRow in dt.Rows)
                    {
                        emailList.Add(emailRow["notif_email"].ToString());
                    }
                    string sTo = string.Join(";", emailList);
                    string sBody = $"<html><body><p>Bonjour," +
                       $"</p><p>Le client de la commande n° {id_order.ToString()} a modifié ses données, " +
                       $"et nous avons effectué la mise à jour dans notre système." +
                       $"</p><ul>" +
                       $"<li>Si l’étiquette de transport n’a pas encore été créée, tout est en ordre : les connées correctes seront prises en compte automatiquement lorsque vous scannerez le bon de commande.</li>" +
                       $"<li>Si l’étiquette a déjà été imprimée, mais que le colis n’a pas encore été récupéré par le transporteur, merci de re-scanner le bon de commande afin de générer une nouvelle étiquette avec les bonnes données.</li>" +
                       $"</ul>Merci de confirmer la prise en compte de cette mise à jour par retour d’email." +
                       $"</p><p></p><p>Hexoa</p>";

                    string envoiMail = Mint.Code.Mails.SendMail(sTo, sSubject, sBody, "", dtChanges, "",true);
                    if (envoiMail != "")
                    {
                        MessageBox.Show("Erreur lors de l'envoi du mail à l'atelier " + string.Join(";", emailList) + " : " + envoiMail);
                    }
                }
                listNeedsUpdate = true;
                RefreshActiveView();
            }

            panel_ModifyOF.Visible = false;

        }
        private void EtiquetteGLS(string idUniqueList)
        {
            string query = $"SELECT * FROM dbo.ViewGLS WHERE id_unique IN ({idUniqueList})";

            DataTable dt = Database.LoadData(query);
            dt.Columns.Remove("id_unique");
            //Keep all info but on one line, and sum Nbre COlis and Poids KG
            // Calcule la somme des colonnes "Nbre Colis" et "Poids Kg"
            //int sommeNbreColis = 0;
            double sommePoidsKg = 0.0;

            foreach (DataRow row in dt.Rows)
            {
                //sommeNbreColis += Convert.ToInt32(row["Nbre Colis"]);
                sommePoidsKg += Convert.ToDouble(row["Poids Kg"]);
            }

            // Écrase les valeurs sur la première rangée
            //dt.Rows[0]["Nbre Colis"] = sommeNbreColis;
            dt.Rows[0]["Poids Kg"] = sommePoidsKg;

            // Supprime toutes les autres lignes
            for (int i = dt.Rows.Count - 1; i > 0; i--)
            {
                dt.Rows.RemoveAt(i);
            }


            string GLSPath = Properties.Settings.Default.glsPath;
            bool openFile = false;
            if (GLSPath == "")
            {
                MessageBox.Show("Le chemin d'accès au logiciel GLS n'est pas renseigné dans les paramètres");
                openFile = true;
            }
            else
            {
                //Check if GLSPath is a valid path on the users computer 
                if (!Directory.Exists(GLSPath))
                {
                    MessageBox.Show("Le chemin d'accès au logiciel GLS n'est pas correct");
                    openFile = true;
                }
            }

            string csvPath;
            if (openFile == false) //Si le chemin d'accès est correct
            {
                string stringdate = DateTime.Now.ToString("yyyymmddhhmmss");
                csvPath = GLSPath + "\\EtiquetteGLS" + stringdate + ".csv";
            }
            else
            {
                csvPath = "output.csv";
            }

            CSV.CreateCsv(dt, csvPath, openFile);

        }
        private void PopUpColis()
        {
            //Display a popup message to ask for the input of a number of package
            InputBox inputBox = new InputBox();
            inputBox.label_Title = "Encodez ou scannez le numéro de colis indiqué sur l'étiquette GLS :";
            string numColis = "";
            inputBox.GiveFocus();
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                numColis = inputBox.UserInput;
                // Use userInput as needed
            }
            //Add the package to the DB
            if (numColis != "")
            {
                string query = "INSERT INTO TBL_EXPEDITION (id_order,id_package, date_add) VALUES (" + textBox_Scan.Text + ",'" + numColis + "', GETDATE())";
                Database.ExecuteQuery(query, null, true);
                //Database.ExecuteQuery(query, null, false); pas besoin, sera synchro
            }
        }
        private async Task PrintAllOFNew()
        {

            //vérifier d'abord si on a un dossier de destination
            if (Properties.Settings.Default.printerPath == "" || !Directory.Exists(Properties.Settings.Default.printerPath))
            {
                MessageBox.Show("Le chemin d'accès au dossier d'impression n'est pas renseigné dans les paramètres, ou n'est pas valide");
                return;
            }

            //Will act like if every order in the PrintPDF query was scanned, display PDF, and the PDF was printed
            //Get the list of all the orders that have no sales state yet
            StatusUpdate?.Invoke($"Détection des commandes sans statut et à produire...");

            string query = "SELECT Distinct id_order from ViewOngoingOrdersDetails where statut_partiel is null and end_order = 0";
            if (radioButton_Atelier.Checked == true) { query += " AND atelier = '" + selected_atelier + "'"; }
            DataTable dtOrdersToPrint = Database.LoadData(query);

            //Do only if there are rows
            if (dtOrdersToPrint.Rows.Count > 0)
            {
                //Display the OF
                string atelier = "";
                if (radioButton_Atelier.Checked == true) { atelier = selected_atelier; }
                int count = 1;
                //Update the orders to printed
                foreach (DataRow row in dtOrdersToPrint.Rows)
                {
                    StatusUpdate?.Invoke($"Génération des OF : {count}/{dtOrdersToPrint.Rows.Count}");
                    string id_order = row["id_order"].ToString();
                    Mint.Code.PDF.CreatePDF("OF", Properties.Settings.Default.OFTemplatePath, id_order, atelier, true);

                    //change the sales state of all items on the OF
                    query = "SELECT id_unique, id_order from ViewOngoingOrdersDetails where statut_partiel is null and id_order = " + id_order;
                    if (radioButton_Atelier.Checked == true) { query += " AND atelier = '" + selected_atelier + "'"; }
                    DataTable dtIDToUpdate = Database.LoadData(query);

                    foreach (DataRow idUniqueRow in dtIDToUpdate.Rows)
                    {
                        string id_unique = idUniqueRow["id_unique"].ToString();
                        ChangeItemStatus(id_unique, "3", id_order); //3 = OF imprimé
                    }
                    count++;
                }
                StatusUpdate?.Invoke(null);
                //Envoyer tout vers l'imprimante
                Mint.Code.Print pdfPrinter = new Mint.Code.Print();
                pdfPrinter.PrintAndDeletePDFsAsync(Properties.Settings.Default.printerPath);

                //MessageBox.Show("Opération terminée." + dtOrdersToPrint.Rows.Count + " ordres de fabrication (OF) ont été envoyés pour impression.");

            }
        }
        private void Show_Details(int id_order)
        {
            DataRow[] dr = dtOngoingDetails.Select("id_order = " + id_order.ToString());

            //Exit if the filter returns nothing
            if (dr.Length == 0) { return; }

            //fill the label values of the details pannel
            label_Details_Order.Text = "Commande n°" + id_order.ToString() + ": " + dr[0]["order_name"].ToString().ToUpper();

            label_Details_NameFirstName.Text = dr[0]["customer_last_name"].ToString().ToUpper() + " " + dr[0]["customer_first_name"].ToString();
            label_Details_Society.Text = dr[0]["business_name"].ToString();
            label_Details_Address1.Text = dr[0]["address_1"].ToString();
            label_Details_Address2.Text = dr[0]["address_2"].ToString();
            label_Details_CPCity.Text = dr[0]["zipcode"].ToString() + " - " + dr[0]["city"].ToString();
            label_Details_Country.Text = dr[0]["country"].ToString().ToUpper();
            label_Details_Tel1.Text = dr[0]["phone"].ToString();
            label_Details_Tel2.Text = dr[0]["phone_mobile"].ToString();


            //Get the package nums
            DataRow[] drPackages = dtPackages.Select("id_order = " + id_order.ToString());
            var stringBuilder = new StringBuilder();
            var links = new List<LinkLabel.Link>();
            linkLabel_Details_Package.Links.Clear();
            foreach (DataRow row in drPackages)
            {
                string packNum = row["id_package"].ToString();
                if (packNum.Length > 34) { packNum = packNum.Substring(33, 8); }
                if (stringBuilder.Length > 0) { stringBuilder.Append(", "); }

                // Create a clickable link for each package number with the GLS URL
                var link = new LinkLabel.Link(stringBuilder.Length, packNum.Length, "https://gls-group.eu/FR/fr/suivi-colis?match=" + packNum);
                links.Add(link);

                // Append the package number to the StringBuilder
                stringBuilder.Append(packNum);
            }

            // Set the text with line breaks in the LinkLabel
            linkLabel_Details_Package.Text = stringBuilder.ToString();

            // Add the links to the LinkLabel
            foreach (var link in links)
            {
                linkLabel_Details_Package.Links.Add(link);
            }

            //fill the datagridview with the details of the order
            DataTable dtDetail = dr.CopyToDataTable();
            //Keep only some columns
            string[] columnsToKeep = { "id_unique", "desc support", "h_reference", "Image", "URL_custom", "url_image", "quantity", "statut_partiel" };
            if (radioButton_All.Checked) { columnsToKeep = columnsToKeep.Concat(new[] { "atelier" }).ToArray(); }

            // Utilise la liste des colonnes à conserver pour filtrer la DataTable
            dtDetail = dtDetail.DefaultView.ToTable(false, columnsToKeep);

            //Rename the columns
            dtDetail.Columns["id_unique"].ColumnName = "ID";
            dtDetail.Columns["desc support"].ColumnName = "Support";
            dtDetail.Columns["h_reference"].ColumnName = "Référence";
            dtDetail.Columns["Image"].ColumnName = "Image";
            dtDetail.Columns["URL_custom"].ColumnName = "URL Custom";
            dtDetail.Columns["url_image"].ColumnName = "URL Image";
            dtDetail.Columns["quantity"].ColumnName = "Quantité";
            dtDetail.Columns["statut_partiel"].ColumnName = "Statut";
            ;
            if (radioButton_All.Checked == true) { dtDetail.Columns["atelier"].ColumnName = "Atelier"; }
            dataGridView_Details.DataSource = dtDetail;
            //Hide the url column
            dataGridView_Details.Columns["URL Custom"].Visible = false;
            dataGridView_Details.Columns["URL Image"].Visible = false;
            //Deselect all rows of the datagridview
            dataGridView_Details.ClearSelection();
            // Clear the image box
            this.pictureBox_Details.Visible = false;
            this.button_downloadImage.Visible = false;
        }
        private void button_OKScan_Click(object sender, EventArgs e)
        {
            Scan_Clicked();
        }
        private void button_ongoing_detail_Click(object sender, EventArgs e)
        {
            if (splitContainer_Ongoing.Panel2Collapsed == true)
            {
                splitContainer_Ongoing.Panel2Collapsed = false;
                button_ongoing_detail.Text = "Cacher les détails";
            }
            else
            {
                splitContainer_Ongoing.Panel2Collapsed = true;
                button_ongoing_detail.Text = "Afficher les détails";
            }

        }
        private void button_SendToAteliers_Click(object sender, EventArgs e)
        {
            SendMailToAteliers();
        }
        private void button_Details_OF_Click(object sender, EventArgs e)
        {
            string atelier = "";
            if (radioButton_Atelier.Checked == true) { atelier = selected_atelier; }
            PDF.CreatePDF("OF", "", id_order.ToString(), atelier);
        }
        private void button_CancelOF_Click(object sender, EventArgs e)
        {
            CancelOF(id_order.ToString());
        }
        private async void buttonPrintOF_Click(object sender, EventArgs e)
        {
            //Deactivate if on view all
            if (radioButton_All.Checked == true) { MessageBox.Show("La fonction [ Imprimer tous les OF ] est désactivée quand le mode 'Tous les ateliers' est sélectionné.", "Fonction désactivée", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else { await Task.Run(async () => { await PrintAllOFNew(); }); }
        }
        private void button_downloadImage_Click(object sender, EventArgs e)
        {
            //Download the picture into the classic download folder of the user
            string url = pictureBox_Details.imageURLs[0];
            string id_order_detail = pictureBox_Details.id_order_detail;
            //Get the filename after the last slash
            string fileName = id_order_detail + '-' + url.Substring(url.LastIndexOf('/') + 1);

            string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\" + fileName;
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(url, downloadPath);
            }
            //Open the folder
            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + downloadPath + "\"");

        }
        private void dataGridView_Ongoing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Send the details of the order to the Details pannel
            id_order = (int)dataGridView_Ongoing.Rows[e.RowIndex].Cells["ID"].Value;
            Show_Details(id_order);
        }
        private void dataGridView_Ongoing_SelectionChanged(object sender, EventArgs e)
        {
            // Check if the event was triggered by a user action (selection change)
            if (dataGridView_Ongoing.Focused && dataGridView_Ongoing.SelectedRows.Count > 0)
            {
                // Get the selected row index
                int rowIndex = dataGridView_Ongoing.SelectedRows[0].Index;

                // Check if the row index is valid
                if (rowIndex >= 0 && rowIndex < dataGridView_Ongoing.Rows.Count)
                {
                    // Get the value of the "order_name" cell from the selected row
                    id_order = (int)dataGridView_Ongoing.Rows[rowIndex].Cells["ID"].Value;

                    // Call the Show_Details method with the order_name parameter
                    Show_Details(id_order);
                }
            }
        }
        private void dataGridView_Details_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_Details.SelectedRows.Count != 1) { return; }
            //Send the details of the order to the Details pannel
            int rowIndex = dataGridView_Details.SelectedRows[0].Index;
            if (rowIndex == -1) { return; }
            string imageReference = dataGridView_Details.Rows[rowIndex].Cells["Image"].Value.ToString();
            string id_order_detail = dataGridView_Details.Rows[rowIndex].Cells["ID"].Value.ToString();
            string imageURL = "";

            if (dataGridView_Details.Rows[rowIndex].Cells["URL Custom"].Value.ToString() == "")
            {
                imageURL = dataGridView_Details.Rows[rowIndex].Cells["URL Image"].Value.ToString();
                button_downloadImage.Visible = false;
            }
            else
            {
                imageURL = dataGridView_Details.Rows[rowIndex].Cells["URL Custom"].Value.ToString();
                button_downloadImage.Visible = true;
            }

            if (imageReference != "" && imageURL != "")
            {
                pictureBox_Details.Visible = true;
                pictureBox_Details.id_order_detail = id_order_detail;
                pictureBox_Details.LoadPictures(imageReference, imageURL);
            }
            else
            {
                pictureBox_Details.Visible = false;
                button_downloadImage.Visible = false;
            }
        }
        private void checkBox_ShowComplete_CheckedChanged(object sender, EventArgs e)
        {
            listNeedsUpdate = true;
            RefreshActiveView();
        }
        private void textBox_Scan_KeyPress(object sender, KeyPressEventArgs e)
        {
            //If the user pressed enter, simulate a click on the OK button
            if (e.KeyChar == (char)13)
            {
                Scan_Clicked();
            }
        }
        private void linkLabel_Details_Package_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = e.Link.LinkData as string;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        // Check if the URL is a well-formed absolute URI (i.e., a web link)
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                        {
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        // Handle invalid URL or other cases as needed
                        MessageBox.Show("L'URL est invalide.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur
                    MessageBox.Show("Impossible d'ouvrir l'URL: " + ex.Message);
                }
            }
        }
        private void dataGridView_Details_MouseDown(object sender, MouseEventArgs e)
        {
            //Only do if the user can control all ateliers
            if (!all_ateliers) { return; }
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dataGridView_Details.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow >= 0)
                {
                    dataGridView_Details.ClearSelection();
                    dataGridView_Details.Rows[currentMouseOverRow].Selected = true;
                    // Use the existing context menu strip
                    ChangeSaleItemContextMenu changeSaleItemContextMenu = new ChangeSaleItemContextMenu();
                    changeSaleItemContextMenu.LoadSalesData(dataGridView_Details.Rows[currentMouseOverRow].Cells["ID"].Value.ToString());
                    // Show the custom form at the specified location
                    changeSaleItemContextMenu.StartPosition = FormStartPosition.Manual;
                    changeSaleItemContextMenu.Location = dataGridView_Details.PointToScreen(new Point(e.X, e.Y));
                    changeSaleItemContextMenu.Show();
                }
            }
        }
        #endregion

        #region Procedures related to the settings
        private void pictureBox_PathGLS_Click(object sender, EventArgs e)
        {
            textBox_PathGLS.Text = Code.BasicFunctions.ChoosePath("Choisissez le dossier de destination pour les étiquettes GLS", "Folder");
        }
        private void pictureBox_PathOF_Click(object sender, EventArgs e)
        {
            textBox_OFTemplate.Text = Code.BasicFunctions.ChoosePath("Choisissez le template à utiliser pour les OF", "File");
        }
        private void pictureBox_PathPrinter_Click(object sender, EventArgs e)
        {
            textBox_Printer.Text = Code.BasicFunctions.ChoosePath("Choisissez le dossier de lecture pour l'imprimante", "Folder");
        }
        private void pictureBox_GoGLS_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox_PathGLS.Text);
        }
        private void pictureBox_GoOF_Click(object sender, EventArgs e)
        {
            //Open template if the textbox is null
            if (textBox_OFTemplate.Text == "")
            {
                PDF.CreatePDF("OF", "", "0"); //This will open the template as the id is zero
            }
            else
            {
                System.Diagnostics.Process.Start("explorer.exe", textBox_OFTemplate.Text);
            }

        }
        private void pictureBox_GoPrinter_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox_Printer.Text);
        }
        private void textBox_PathGLS_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.glsPath = textBox_PathGLS.Text;
            Properties.Settings.Default.Save();
        }
        private void textBox_OFTemplate_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OFTemplatePath = textBox_OFTemplate.Text;
            Properties.Settings.Default.Save();
        }
        private void textBox_Printer_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.printerPath = textBox_Printer.Text;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Procedures launched by other forms
        public static async Task StoreIDArticleLocally()
        {
            //Store the ID article locally
            StatusUpdate?.Invoke($"Assignation des références aux articles...");
            string query = "SELECT id_order_detail from TBL_SALES where id_article is null"; //,reference,quantity,id_sales_state
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                //Store all id_unique in a string separated by a comma
                string id_unique = string.Join(",", dt.AsEnumerable().Select(row => row.Field<int>("id_order_detail")));


                //load matches in BestMatch_Article
                query = "SELECT id_unique, article, atelier, is_from_stock from BestMatch_Article where id_unique in (" + id_unique + ")";
                DataTable dtMatch = Database.LoadData(query);

                int count = 0;
                string id_article = "";
                string id_atelier = "";
                string is_from_stock = "";
                foreach (DataRow row in dt.Rows)
                {
                    StatusUpdate?.Invoke($"Assignation des références aux articles : " + count + "/" + dt.Rows.Count);
                    id_unique = row["id_order_detail"].ToString();

                    //find the id_article in dtMatch by filtering on id_unique
                    DataRow[] rows = dtMatch.Select("id_unique = " + id_unique);
                    if (rows.Length > 0)
                    {
                        id_article = rows[0]["article"].ToString();
                        id_atelier = rows[0]["atelier"].ToString();
                        is_from_stock = rows[0]["is_from_stock"].ToString();
                    }
                    else { id_article = ""; }
                    if (id_article != "")
                    {
                        query = "UPDATE TBL_SALES SET id_article = @id_article, id_atelier = @id_atelier, is_from_stock = @is_from_stock, date_update = GETDATE() where id_order_detail = @id_unique";
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("@id_article", (int)rows[0]["article"]);
                        param.Add("@id_atelier", (int)rows[0]["atelier"]);
                        param.Add("@is_from_stock", (int)rows[0]["is_from_stock"]);
                        param.Add("@id_unique", (int)row["id_order_detail"]);
                        Database.ExecuteQuery(query, param, true);
                        Database.ExecuteQuery(query, param);

                    }
                    count++;
                }
            }
            StatusUpdate?.Invoke($"");
        }
        public static async Task ConsumeRawMatsAndStoreCost()
        {
            //Consume all raw mats of new orders and store cost

            // Consume Raw Mats
            StatusUpdate?.Invoke($"Consommation des matières premières et sauvegarde des coûts de production...");
            string query = "SELECT id_order_detail from TBL_SALES where ERP_total_cost is null";
            //Request to have this only once objet is sent
            query += " and id_sales_state = 1";
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                //Store all id_unique in a string separated by a comma
                string id_unique = string.Join(",", dt.AsEnumerable().Select(row => row.Field<int>("id_order_detail")));

                //load matches in Raw Mat
                query = "SELECT * from Link_SalesToRawMat where id_order_detail in (" + id_unique + ")";
                DataTable dtRawMat = Database.LoadData(query);

                int count = 0;

                foreach (DataRow row in dtRawMat.Rows)
                {
                    count++;
                    string sTable = "";
                    string sIDField = "";
                    string sID = "";
                    string sType = "";

                    double consume_stock = (double)row["q_substract"];

                    StatusUpdate?.Invoke($"Consommation des matières premières et sauvegarde des coûts de production: " + count + "/" + dtRawMat.Rows.Count);
                    if (row["id_negoce"] != DBNull.Value && row["q_cost"] != DBNull.Value)
                    {
                        //if (Convert.ToInt32(row["stock"]) > 0)
                        // Consume Raw Mats and go below zero
                        sTable = "TBL_NEGOCE";
                        sType = "Negoce";
                        sIDField = "id_negoce";
                        sID = row["id_negoce"].ToString();
                    }
                    else if (row["id_rawmat"] != DBNull.Value && row["q_cost"] != DBNull.Value)
                    {
                        sTable = "TBL_RAWMAT";
                        sType = "RawMat";
                        sIDField = "id_rawmat";
                        sID = row["id_rawmat"].ToString();
                    }
                    if (sID != "")
                    {
                        //Store current stock
                        query = $"SELECT stock FROM {sTable} WHERE {sIDField} = {sID}";
                        DataTable dtStock = Database.LoadData(query);

                        if (dtStock.Rows[0]["stock"] != DBNull.Value)
                        //Article suivi en stock
                        {
                            double previous_stock = Convert.ToDouble(dtStock.Rows[0]["stock"]);
                            double new_stock = previous_stock - consume_stock;
                            //Consume the raw mat
                            query = $"UPDATE {sTable} SET stock = Round(@new_stock,2), date_update = GETDATE() WHERE {sIDField} = {sID}";
                            Dictionary<string, object> param = new Dictionary<string, object>();
                            param.Add("@new_stock", new_stock);
                            Database.ExecuteQuery(query, param);

                            //Add this consumption to the TBL_STOCK_CHANGE

                            query = "INSERT INTO TBL_STOCK_CHANGE (id_order_detail, type_modified, id_modified,old_value,new_value,user_change, date_add) VALUES (@id_order_detail, @type_modified, @id_modified,Round(@old_value,2),Round(@new_value,2),@user_change, GETDATE())";
                            Dictionary<string, object> param3 = new Dictionary<string, object>();
                            param3.Add("@id_order_detail", (int)row["id_order_detail"]);
                            param3.Add("@type_modified", sType);
                            param3.Add("@id_modified", sID);
                            param3.Add("@old_value", previous_stock);
                            param3.Add("@new_value", new_stock);
                            param3.Add("@user_change", Properties.Settings.Default.login);
                            Database.ExecuteQuery(query, param3);
                        }

                        //Store locally the production cost of the article by adding the cost of the raw mat
                        query = "UPDATE TBL_SALES SET ERP_total_cost = ROUND(ISNULL(ERP_total_cost, 0) + @totalcost, 2) WHERE id_order_detail = @id_order_detail";
                        Dictionary<string, object> param2 = new Dictionary<string, object>();
                        param2.Add("@totalcost", (double)row["q_cost"]);
                        param2.Add("@id_order_detail", (int)row["id_order_detail"]);
                        Database.ExecuteQuery(query, param2, true);
                        Database.ExecuteQuery(query, param2);

                    }
                }
            }
            StatusUpdate?.Invoke($"");
        }
        public static async Task UpdateOldOrderStates()
        {
            StatusUpdate?.Invoke($"Passage en Livré des vieilles commandes avec tous les objets expédiés...");
            string query = "SELECT id_order from Alarm_OrderStatus";
            string id_order = "";
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count++;
                    StatusUpdate?.Invoke($"Passage en Livré des vieilles commandes avec tous les objets expédiés...{count}/{dt.Rows.Count}");

                    id_order = row["id_order"].ToString();
                    //Change the id_order_state of the line in TBL_ORDER
                    query = "UPDATE TBL_ORDER SET id_order_state = 27, date_update = GETDATE() WHERE id_order = " + id_order;
                    Database.ExecuteQuery(query, null, true);
                    Database.ExecuteQuery(query, null, false);
                    //Add this change to TBL_ORDER_CHANGE
                    query = "INSERT INTO TBL_ORDER_CHANGE (id_order, id_order_state, date_change, id_website, date_add) VALUES (" + id_order + ", 27, GETDATE(),'Auto_ERP', GETDATE())";
                    Database.ExecuteQuery(query, null, true);
                    //Database.ExecuteQuery(query, null, false); car les numéros Auto ID ne sont pas les mêmes

                }
            }
            StatusUpdate?.Invoke($"");


        }
        #endregion


    }
}

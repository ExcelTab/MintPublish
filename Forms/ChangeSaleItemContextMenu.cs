using Mint.Code;
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
    public partial class ChangeSaleItemContextMenu : Form
    {
        public ChangeSaleItemContextMenu()
        {
            InitializeComponent();

            //Load the ateliers in the combobox
            string queryAteliers = "SELECT id_atelier,name FROM TBL_ATELIER Where is_active = 1";
            queryAteliers += " Order By sort_order asc";
            DataTable dtAteliers = Database.LoadData(queryAteliers);
            comboBox_Atelier.DataSource = dtAteliers;
            comboBox_Atelier.DisplayMember = "name";
            comboBox_Atelier.ValueMember = "id_atelier";

            //Load the articles in the combobox
            string queryArticles = "SELECT id_article,name FROM TBL_ARTICLE";
            queryArticles += " ORDER BY id_type, id_support, id_format, long, larg, name, id_dimension";
            DataTable dtArticles = Database.LoadData(queryArticles);
            comboBox_Article.DataSource = dtArticles;
            comboBox_Article.DisplayMember = "name";
            comboBox_Article.ValueMember = "id_article";

        }

        public void LoadSalesData(string id_order_detail)
        {
            //Load the sales data
            label_id_order_detail.Text = id_order_detail;
            string querySales = "SELECT id_atelier, id_article, Round(ERP_total_cost,2) as TotalCost FROM TBL_SALES";
            querySales += " WHERE id_order_detail = " + id_order_detail;
            DataTable dtSales = Database.LoadData(querySales);
            if (dtSales.Rows.Count > 0)
            {
                //Check for null values before applying to combobox
                if (dtSales.Rows[0]["id_atelier"] != DBNull.Value) { comboBox_Atelier.SelectedValue = dtSales.Rows[0]["id_atelier"].ToString(); }
                if (dtSales.Rows[0]["id_article"] != DBNull.Value) { comboBox_Article.SelectedValue = dtSales.Rows[0]["id_article"].ToString(); }
                string totalCost = dtSales.Rows[0]["TotalCost"].ToString();
                if (totalCost.EndsWith("00"))
                {
                    //Replace the last 00 by €
                    totalCost = totalCost.Substring(0, totalCost.Length - 2) + " €";
                }
                textBox_TotalCost.Text = totalCost;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string updateQuery = $@"UPDATE TBL_SALES SET id_atelier = {comboBox_Atelier.SelectedValue.ToString()},
                                id_article = {comboBox_Article.SelectedValue.ToString()},";
            if (textBox_TotalCost.Text != "") {updateQuery+= $"ERP_total_cost = {textBox_TotalCost.Text.Replace(" €", "").Replace(",", ".")},"; }
            updateQuery+= $"date_update = GETDATE() WHERE id_order_detail = {label_id_order_detail.Text}";
            try
            {
                Database.ExecuteQuery(updateQuery, null, true);
                Database.ExecuteQuery(updateQuery, null, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"La commande {updateQuery} est invalide. Merci de prendre un screenshot 
                de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
            }

            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mint.Code;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Mint.Forms
{
    public partial class App_Orders : Form
    {

        private SqlDataAdapter adapter;
        private DataTable dataTable;

        public App_Orders()
        {
            InitializeComponent();
            Lbl_User.Text = Properties.Settings.Default.login;
            Refresh_Orders();
        }
        private async void Refresh_Orders()
        {
            //Create a string list with all checkbox checked
            List<string> lStatus = new List<string>();
            foreach (Control c in Groupbox_Status.Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    if (cb.Checked)
                    {
                        lStatus.Add("'" + cb.Text + "'");
                    }
                }
            }


            //Store locally the value of the textboxes TB_Login and TB_Password
            SqlConnection connection = new SqlConnection(Database.MainConnectionString);
            await connection.OpenAsync();

            string query = "SELECT Distinct N_commande,Source,Ajouté_Le,OF_Status FROM ViewTestOrders " +
                "WHERE ";
            //Add the where clause according to the user logged in
            if (Properties.Settings.Default.login == "Mathieu") { query = query + "VisibleBrive is not null"; }
            else if (Properties.Settings.Default.login == "Brive") { query = query + "VisibleBrive = 1"; }
            else if (Properties.Settings.Default.login == "Seignosse") { query = query + "VisibleSeignosse = 1"; }
            else { query = query + "VisibleBrive is null"; }

            //add a where clause for each status checked
            if (lStatus.Count > 0) { query += " AND OF_STATUS IN (" + string.Join(",", lStatus) + ")"; }
            else { query += " AND OF_STATUS = 'Impossible'"; }

            SqlCommand command = new SqlCommand(query, connection);
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Assuming you have a ListView named listView1 in your form
            //TODO : replace the listview by a datagridview or better gridview
            Lvi_Orders.Items.Clear();
            Lvi_Orders.View = View.Details;

            // Add column headers
            foreach (DataColumn column in dataTable.Columns)
            {
                Lvi_Orders.Columns.Add(column.ColumnName);

            }

            // Add data rows
            foreach (DataRow row in dataTable.Rows)
            {
                Lvi_Orders.Items.Add(new ListViewItem(row.ItemArray.Select(x => x.ToString()).ToArray()));
            }

            Lvi_Orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            connection.Close();



        }

        private void Cbx_NEW_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Lvi_Orders_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button_Refresh_Click(object sender, EventArgs e)
        {
            Refresh_Orders();
        }
    }
}

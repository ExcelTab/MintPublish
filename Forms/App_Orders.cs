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

            // TODO : rechanger cette commande, faire différement
            string query = "SELECT * FROM TBL_ORDER";
            DataTable dt = Database.LoadData(query);

            Lvi_Orders.Items.Clear();
            Lvi_Orders.View = View.Details;

            // Add column headers
            foreach (DataColumn column in dt.Columns)
            {
                Lvi_Orders.Columns.Add(column.ColumnName);

            }

            // Add data rows
            foreach (DataRow row in dt.Rows)
            {
                Lvi_Orders.Items.Add(new ListViewItem(row.ItemArray.Select(x => x.ToString()).ToArray()));
            }

            Lvi_Orders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);



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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mint.Code.PDF.CreatePDF();
        }
    }
}

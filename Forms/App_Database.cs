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

namespace Mint.Forms
{
    public partial class App_Database : Form
    {

        private SqlDataAdapter adapter;
        private DataTable dataTable;

        public App_Database()
        {
            InitializeComponent();

            // Load the list of databases in the combobox
            List<string> databaseList = Database.LoadDatabases();
            Cbb_Database.DataSource = databaseList;
        }

        private void Load_Datatables(object sender, EventArgs e)
        {
            //Clear the value of the combobox
            Lbo_Tables.DataSource = null;
            // Load the list of tables in the combobox
            string sDatabase = Cbb_Database.SelectedItem.ToString();

            // if sDatabase is not null, then load the tables
            if (!string.IsNullOrEmpty(sDatabase))
            {
                bool bBuffer = false;
                if (sDatabase == "Hexoa Buffer") { bBuffer = true; }
                List<string> tableList = Database.LoadDatatables(bBuffer);
                Lbo_Tables.DataSource = tableList;
            }
        }

        private void Refresh_Datatable(object sender, EventArgs e)
        {
            //Hide the Gridview while loading
            Lbl_Charging.Visible = true;
            Dgv_Main.Visible = false;

            //Store locally the value of the textboxes TB_Login and TB_Password
            string login = Properties.Settings.Default.login;
            string password = Properties.Settings.Default.password;
            string sDatabase = Cbb_Database.SelectedItem.ToString();
            bool bBuffer = false;
            if (sDatabase == "Hexoa Buffer") { bBuffer = true; }
            string sTable = "";
            if (Lbo_Tables.SelectedItem != null)
            {
                sTable = Lbo_Tables.SelectedItem.ToString();
            }
            else
            {
                return; //Car peut etre en cours de changement de liste
            }

            string connectionString;
            if (login == "simonghislain") { connectionString = $"Server=tcp:exceltab.database.windows.net,1433;Initial Catalog={sDatabase};Persist Security Info=False;User ID={login};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; }
            else { connectionString = bBuffer ? Database.BufferConnectionString() : Database.MainConnectionString(); }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT * FROM {sTable};";
            SqlCommand command = new SqlCommand(query, connection);
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            Dgv_Main.DataSource = dataTable;
            connection.Close();
            // Show the Gridview after loading
            Lbl_Charging.Visible = false;
            Dgv_Main.Visible = true;

            // Add the Dgv_Main_DataError event handler
            Dgv_Main.DataError += new DataGridViewDataErrorEventHandler(Dgv_Main_DataError);
        }

        private void Dgv_Main_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Handle the exception and suppress the error message
            e.ThrowException = false;
        }

        private void Save_Datatable(object sender, EventArgs e)
        {
            //Upload all data from the datagridview that was modified by the user to the SQL database
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dataTable);
        }

        private void Refresh_Buffer(object sender, EventArgs e)
        {
            //Upload all data from the datagridview that was modified by the user to the SQL database
            Database.LoadDataFromBufferToMain(false);
        }


        private void Dgv_Main_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = dataTable.Rows[e.RowIndex][e.ColumnIndex];
        }
    }
}

using System.Data;
using System.Data.SqlClient;

namespace Mint.Code
{
    internal static class Database
    {
        /// <summary>
        ///  Containing all scripts related to data 
        /// </summary>
        /// 

        //Store locally the value of the textboxes TB_Login and TB_Password
        // TODO: Clean these connectionstrings
        public static readonly string ServerName = "exceltab.database.windows.net,1433";
        public static readonly string BufferConnectionString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Buffer;Persist Security Info=False;User ID=BotERP;Password=3094OIHEihoefseiEOIR9872oi;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static readonly string MainConnectionString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Main;Persist Security Info=False;User ID=BotERP;Password=3094OIHEihoefseiEOIR9872oi;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        static void LoadDataFromBuffferToMain()
        {
            /// <summary>
            ///  Checks for updates in the Buffer database and updates the main database accordingly
            /// </summary>
            /// 

            using (SqlConnection BufferConnection = new SqlConnection(BufferConnectionString))
            {
                BufferConnection.Open();

                // Check the last modified date of the buffered table
                string Command = "SELECT last_user_update FROM sys.tables WHERE object_id = OBJECT_ID('dbo.SALES')";
                SqlCommand SqlCommand = new SqlCommand(Command, BufferConnection);
                DateTime BufferDate = (DateTime)SqlCommand.ExecuteScalar();

                BufferConnection.Close();

                using (SqlConnection mainConnection = new SqlConnection(MainConnectionString))
                {
                    mainConnection.Open();

                    // Check the last modified date of the lodger table
                    string checkLodgerDateQuery = "SELECT LastModifiedDate FROM LodgerTable";
                    SqlCommand checkLodgerDateCommand = new SqlCommand(checkLodgerDateQuery, mainConnection);
                    DateTime lodgerModifiedDate = (DateTime)checkLodgerDateCommand.ExecuteScalar();

                    if (BufferDate > lodgerModifiedDate)
                    {
                        // Drop the main database table
                        string dropTableQuery = "DROP TABLE MainDataTable";
                        SqlCommand dropTableCommand = new SqlCommand(dropTableQuery, mainConnection);
                        dropTableCommand.ExecuteNonQuery();

                        // Create the main table
                        string createTableQuery = "CREATE TABLE MainDataTable (...)";
                        SqlCommand createTableCommand = new SqlCommand(createTableQuery, mainConnection);
                        createTableCommand.ExecuteNonQuery();

                        // Copy the data from the buffered table to the main table
                        string copyTableQuery = "INSERT INTO MainDataTable SELECT * FROM [BufferedDatabaseName].[dbo].[BufferedDataTable]";
                        SqlCommand copyTableCommand = new SqlCommand(copyTableQuery, mainConnection);
                        copyTableCommand.ExecuteNonQuery();

                        // Update the lodger table with the new last modified date
                        string updateLodgerQuery = "UPDATE LodgerTable SET LastModifiedDate = @ModifiedDate";
                        SqlCommand updateLodgerCommand = new SqlCommand(updateLodgerQuery, mainConnection);
                        updateLodgerCommand.Parameters.AddWithValue("@ModifiedDate", BufferDate);
                        updateLodgerCommand.ExecuteNonQuery();
                    }

                    mainConnection.Close();
                }
            }
        }

        // TODO: Chose if we use Using or declare connections in these 2 similar LoadData and LoadTable methods
        public static List<string> LoadDatabases()
        {
            List<string> DatabaseList = new List<string>();

            // Connect to the server
            string connectionString = "Server=" + ServerName + ";User ID=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                // Retrieve the list of databases
                DataTable databases = connection.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    string databaseName = database.Field<string>("database_name");
                    DatabaseList.Add(databaseName);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("You don't have master access to the server : " + ex.Message);
                DatabaseList.Add("Hexoa Main");
            }
            finally
            {
                connection.Close();
            }

            return DatabaseList;
        }

        public static List<string> LoadDatatables(string sDatabase)
        {
            // Load all tables from a database
            List<string> DatatableList = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(MainConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.tables", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DatatableList.Add(reader[0].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading tables: {ex.Message}");
            }

            return DatatableList;
        }

        public static DataTable LoadData(string sCommand)
        {
            // Load all data from a specific SQL Command
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(MainConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sCommand, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
            return dt;
        }
    }

}
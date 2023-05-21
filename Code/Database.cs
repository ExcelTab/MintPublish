using Mint.Properties;
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
  
        public static string MainConnectionString()
        {
            string sProductKey = Settings.Default.productkey;
            if(sProductKey==string.Empty)
            {
                //TODO: Changer ce système de clé produit pourri
                //Open an input box and ask the user to enter the product key
                sProductKey = Microsoft.VisualBasic.Interaction.InputBox("Cet outil est verouillé. Introduisez la clé que vous avez reçu d'ExcelTab.", "Clé Produit", "", -1, -1);
                Settings.Default.productkey = sProductKey;
                Settings.Default.Save();
            }
            string sConString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Main;Persist Security Info=False;User ID=BotERP;Password=" + sProductKey +";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return sConString;
        }

        public static string BufferConnectionString()
        {
            string sProductKey = Settings.Default.productkey;
            if (sProductKey == string.Empty)
            {
                //TODO: Changer ce système de clé produit pourri
                //Open an input box and ask the user to enter the product key
                sProductKey = Microsoft.VisualBasic.Interaction.InputBox("Cet outil est verouillé. Introduisez la clé que vous avez reçu d'ExcelTab.", "Clé Produit", "", -1, -1);
                Settings.Default.productkey = sProductKey;
                Settings.Default.Save();
            }
            string sConString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Buffer;Persist Security Info=False;User ID=BotERP;Password=" + sProductKey + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return sConString;
        }

        public static void LoadDataFromBuffferToMain()
        {
            /// <summary>
            ///  Checks for updates in the Buffer database and updates the main database accordingly
            /// </summary>
            /// 

            // Connection strings for Buffer and Main databases
            string bufferConnectionString = BufferConnectionString();
            string mainConnectionString = MainConnectionString();

            List<string> updatedTables = new List<string>();

            // Establish connections
            using (SqlConnection bufferConnection = new SqlConnection(bufferConnectionString))
            using (SqlConnection mainConnection = new SqlConnection(mainConnectionString))
            using (SqlConnection catalogConnection = new SqlConnection(mainConnectionString))
            {
                bufferConnection.Open();
                mainConnection.Open();
                catalogConnection.Open();

                // Query to fetch tables from CATALOG_TABLES
                string tableQuery = "SELECT TRIM(Table_Name) AS TableName FROM CATALOG_TABLES";

                // Execute the query on Main database to get the list of tables
                SqlCommand tableCommand = new SqlCommand(tableQuery, catalogConnection);
                SqlDataReader catalogReader = tableCommand.ExecuteReader();

                while (catalogReader.Read())
                {
                    string tableName = catalogReader.GetString(0);

                    // Query to check modification dates for the current table
                    string modifyQuery = $@"
                        SELECT modify_date AS BDate, (SELECT COUNT(*) FROM {tableName}) AS BRows
                        FROM sys.tables
                        WHERE name = '{tableName}'";

                    // Execute the query on Buffer database
                    SqlCommand bufferCommand = new SqlCommand(modifyQuery, bufferConnection);
                    SqlDataReader bufferReader = bufferCommand.ExecuteReader();

                    int bufferRowCount = 0;
                    if (bufferReader.Read())
                    {
                        //DateTime bufferModifyDate = bufferReader.GetDateTime(0);
                        bufferRowCount = bufferReader.GetInt32(1);
                    }
                    bufferReader.Close();

                    // Execute the query on Main database
                    //SqlCommand mainCommand = new SqlCommand(modifyQuery, mainConnection);
                    //SqlDataReader mainReader = mainCommand.ExecuteReader();

                    //if (mainReader.Read())
                    //{
                    //    DateTime mainModifyDate = mainReader.GetDateTime(0);
                    //    int mainRowCount = mainReader.GetInt32(1);
                    //}
                    //mainReader.Close();

                    // Check if the Buffer table meets the criteria
                    if (bufferRowCount >= 2)
                    { 
                        //bufferModifyDate < DateTime.Now.AddMinutes(-10) &&
                        //bufferModifyDate > mainModifyDate)

                        // Deletes previous data from Main database
                        string deleteQuery = $"DELETE FROM dbo.{tableName}";
                        SqlCommand deleteCommand = new SqlCommand(deleteQuery, mainConnection);
                        deleteCommand.ExecuteNonQuery();

                        // Select data from Buffer database
                        string selectQuery = $"SELECT * FROM dbo.{tableName}";
                        SqlCommand selectCommand = new SqlCommand(selectQuery, bufferConnection);

                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand))
                        {
                            DataTable bufferData = new DataTable();
                            dataAdapter.Fill(bufferData);

                            // Insert this buffered data into Main database on the corresponding data table
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(mainConnection))
                            {
                                bulkCopy.DestinationTableName = tableName;
                                bulkCopy.WriteToServer(bufferData);
                            }
                        }
                        //TODO : get to a delete of the buffer later
                        // Deletes the data from Buffer database
                        //SqlCommand deleteBufferCommand = new SqlCommand(deleteQuery, bufferConnection);
                        //deleteBufferCommand.ExecuteNonQuery();

                        //TODO : also add the date at which the table was updated in the CATALOG_TABLES table
                        // Add the table to the list of updated tables
                        updatedTables.Add(tableName);
                    }                   
                }
                catalogReader.Close();
            }
            //show in a message box the list of updated tables
            string message = "Les tables suivantes ont été mises à jour : \n";
            foreach (string table in updatedTables)
            {
                message += table + "\n";
            }
            MessageBox.Show(message);

            
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
                using (SqlConnection connection = new SqlConnection(MainConnectionString()))
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
                using (SqlConnection connection = new SqlConnection(MainConnectionString()))
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

        public static void ExecuteQuery(string query, Dictionary<string, object> parameters = null)
            {
                using (SqlConnection connection = new SqlConnection(MainConnectionString()))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        if (parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in parameters)
                            {
                                if (parameter.Value == null)
                                {
                                    command.Parameters.AddWithValue(parameter.Key, DBNull.Value);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                                }
                            }
                        }
                    command.ExecuteNonQuery();
                    }
                }
            }
        }

    }

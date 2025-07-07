    using Mint.Properties;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;



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
        public delegate void StatusUpdateDelegate(string message);
        public static event StatusUpdateDelegate StatusUpdate;

        public static string MainConnectionString()
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
            string sConString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Main;Persist Security Info=False;User ID=BotERP;Password=" + sProductKey + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=240;";
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
            string sConString = "Server=tcp:exceltab.database.windows.net,1433;Initial Catalog=Hexoa Buffer;Persist Security Info=False;User ID=BotERP;Password=" + sProductKey + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=240;";
            return sConString;
        }

        public static async Task LoadDataFromBufferToMain(bool asyncUpdate = false)
        {
            /// <summary>
            ///  Checks for updates in the Buffer database and updates the main database accordingly
            /// </summary>
            /// 

            await Task.Run(() =>
            {

                

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
                    string tableQuery = "SELECT * FROM CATALOG_TABLES WHERE update_on_launch = 1";

                    // Execute the query on Main database to get the list of tables
                    SqlCommand tableCommand = new SqlCommand(tableQuery, catalogConnection);
                    SqlDataReader catalogReader = tableCommand.ExecuteReader();
                   
                    while (catalogReader.Read())
                    {
                        string tableName = catalogReader.GetString("Table_Name").Trim();
                        StatusUpdate?.Invoke(($"{tableName} - Lecture des données structurelles..."));


                        // skip if tablename is null
                        if (tableName == null) { continue; }
                        Boolean isPull = catalogReader.GetBoolean("is_pull");
                        Boolean isUpdate = catalogReader.GetBoolean("is_update");
                        DateTime lastTransferDate = catalogReader.GetDateTime("Last_Transfer_Date");
                        string colID1 = catalogReader.IsDBNull(catalogReader.GetOrdinal("col_identity_1"))
                            ? ""
                            : catalogReader.GetString("col_identity_1");

                        string colID2 = catalogReader.IsDBNull(catalogReader.GetOrdinal("col_identity_2"))
                            ? ""
                            : catalogReader.GetString("col_identity_2");

                        string colID3 = catalogReader.IsDBNull(catalogReader.GetOrdinal("col_identity_3"))
                            ? ""
                            : catalogReader.GetString("col_identity_3");


                        //if is pull, set the source connection to the buffer connection and the destination connection to the main connection
                        SqlConnection sourceConnection = isPull ? bufferConnection : mainConnection;
                        SqlConnection destinationConnection = isPull ? mainConnection : bufferConnection;

                        #region ChangeDBStructure
                        // Check if there are new columns added in the source table
                        string columnQuery = $@"
                        SELECT COLUMN_NAME, DATA_TYPE
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = '{tableName}'";

                        // Retrieve columns from the source table
                        SqlCommand columnCommand = new SqlCommand(columnQuery, sourceConnection);
                        SqlDataReader columnReader = columnCommand.ExecuteReader();
                        List<(string, string)> sourceColumns = new List<(string, string)>();
                        while (columnReader.Read())
                        {
                            string sourceColumnName = columnReader.GetString(0);
                            string sourceDataType = columnReader.GetString(1);
                            sourceColumns.Add((sourceColumnName, sourceDataType));
                        }
                        columnReader.Close();

                        // Retrieve columns from the destination table
                        columnCommand = new SqlCommand(columnQuery, destinationConnection);
                        columnReader = columnCommand.ExecuteReader();
                        List<(string, string)> destinationColumns = new List<(string, string)>();
                        while (columnReader.Read())
                        {
                            string destinationColumnName = columnReader.GetString(0);
                            string destinationDataType = columnReader.GetString(1);
                            destinationColumns.Add((destinationColumnName, destinationDataType));
                        }
                        columnReader.Close();

                        // Identify the missing columns
                        List<(string, string)> missingColumns = sourceColumns
                            .Where(bc => !destinationColumns.Any(mc => mc.Item1 == bc.Item1))
                            .ToList();

                        // Add the missing columns to the destination table
                        foreach (var missingColumn in missingColumns)
                        {
                            StatusUpdate?.Invoke($"{tableName} - Changement de l'architecture...");
                            string columnName = missingColumn.Item1;
                            string dataType = missingColumn.Item2;

                            string alterTableQuery = $"ALTER TABLE {tableName} ADD {columnName} {dataType}";
                            SqlCommand alterTableCommand = new SqlCommand(alterTableQuery, destinationConnection);
                            //TODO : will not work if the user does not have the right to alter the table
                            alterTableCommand.ExecuteNonQuery();
                        }
                        #endregion
                        // Check if date_add or date_update is more recent than the Last_Transfer_Date from the CATALOG_TABLES table
                        StatusUpdate?.Invoke($"{tableName} - Vérification des dates d'ajout et de modification...");
                        string dateQuery = $@"SELECT MAX(MaxDate) AS MaxDate FROM (SELECT CASE WHEN date_add >= date_update OR date_update is null THEN date_add ELSE date_update END AS MaxDate FROM {tableName}) AS Subquery";
                        SqlCommand dateCommand = new SqlCommand(dateQuery, sourceConnection);
                        DateTime? lastModifiedDate = dateCommand.ExecuteScalar() as DateTime?;

                        // skip if lastTransferDate is higher than lastModifiedDate is null
                        if (lastTransferDate.AddSeconds(1) < lastModifiedDate.Value || lastModifiedDate == null)
                        {
                            string slastTransferDate;
                            if (lastModifiedDate.HasValue)
                            {
                                slastTransferDate = lastModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                // Use slastPushDate as needed
                            }
                            else
                            {
                                // Handle the case where lastPushDate is null
                                continue;
                            }

                            if (isUpdate)
                            {
                                // Do not delete the whole table, instead update values of old primary keys, and insert new ones in the destination table
                                // Select rows from source that need to be updated or inserted
                                string selectQuery = $"SELECT * FROM dbo.{tableName} WHERE date_update > @lastTransferDate OR date_add > @lastTransferDate";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, sourceConnection);
                                selectCommand.Parameters.AddWithValue("@lastTransferDate", lastTransferDate);
                                selectCommand.CommandTimeout = 120;

                                StatusUpdate?.Invoke($"{tableName} - Chargement des nouvelles données...");
                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    //if the reader has no rows, then skip to the next table
                                    if (!reader.HasRows) { continue; }
                                    //store all the values in the codID1 column in a list
                                    List<string> colID1Values = new List<string>();
                                    List<string> colID2Values = new List<string>();
                                    List<string> colID3Values = new List<string>();
                                    List<string> batchIds1 = new List<string>();
                                    List<string> batchIds2 = new List<string>();
                                    List<string> batchIds3 = new List<string>();
                                    while (reader.Read())
                                    {
                                        colID1Values.Add(reader.GetValue(colID1).ToString().Trim());
                                        if (colID2 != "") { colID2Values.Add(reader.GetValue(colID2).ToString().Trim()); }
                                        if (colID3 != "") { colID3Values.Add(reader.GetValue(colID3).ToString().Trim()); }
                                    }

                                    int SQLbatchSize = 100; //Adjust if SQL is laggy or buggy

                                    for (int i = 0; i < colID1Values.Count; i += SQLbatchSize)
                                    {
                                        batchIds1 = colID1Values.Skip(i).Take(SQLbatchSize).ToList();
                                        if (colID2 != "") { batchIds2 = colID2Values.Skip(i).Take(SQLbatchSize).ToList(); }
                                        if (colID3 != "") { batchIds3 = colID3Values.Skip(i).Take(SQLbatchSize).ToList(); }

                                        // Generate the delete query with the batchsize to delete the rows in the destination table
                                        string deleteQuery = $"DELETE FROM dbo.{tableName} WHERE " + colID1 + " IN (" + string.Join(",", batchIds1) + ")";
                                        if (colID2 != "") { deleteQuery += $" AND " + colID2 + " IN (" + string.Join(",", batchIds2) + ")"; }
                                        if (colID3 != "") { deleteQuery += $" AND " + colID3 + " IN (" + string.Join(",", batchIds3) + ")"; }
                                        SqlCommand deleteOneCommand = new SqlCommand(deleteQuery, destinationConnection);
                                        StatusUpdate?.Invoke($"{tableName} - Suppression des anciennes données : {i}/{colID1Values.Count} ...");
                                        deleteOneCommand.ExecuteNonQuery();
                                    }
                                }
                                //Insert all the new rows or newly updated rows
                                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand))
                                {
                                    StatusUpdate?.Invoke($"{tableName} - Insertion des nouvelles données chargées...");
                                    DataTable sourceData = new DataTable();
                                    dataAdapter.Fill(sourceData);

                                    // Insert this source data into destination database on the corresponding data table
                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                                    {
                                        bulkCopy.DestinationTableName = tableName;
                                        bulkCopy.WriteToServer(sourceData);
                                    }
                                }

                                //Corrects the table if some values exists in Buffer but not in Main
                                // Sélectionnez tous les ID de la table Buffer et stockez-les dans une DataTable sourceTable
                                StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Obtention des ID copiés...");
                                string queryNew = $"SELECT {colID1} FROM {tableName}";
                                using (SqlCommand selectNew = new SqlCommand(queryNew, destinationConnection))
                                {
                                    DataTable importedRecords = new DataTable();
                                    using (SqlDataAdapter Newadapter = new SqlDataAdapter(selectNew))
                                    {
                                        Newadapter.Fill(importedRecords);
                                    }

                                    // Assuming you already have a DataTable "sourceTable" with the IDs from the source database


                                    // Step 2: Create a temp table in the destination database to store the IDs
                                    StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Création de la table temporaire...");
                                    string createTempTableQuery = $"CREATE TABLE #TempSourceIDs ({colID1} INT)";
                                        using (SqlCommand createTempTableCmd = new SqlCommand(createTempTableQuery, sourceConnection))
                                        {
                                            createTempTableCmd.ExecuteNonQuery();
                                        }

                                    // Step 3: Use SqlBulkCopy to dump the IDs from sourceTable into the temp table
                                    StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Ecriture de la table temporaire...");
                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sourceConnection))
                                        {
                                            bulkCopy.DestinationTableName = "#TempSourceIDs";
                                            bulkCopy.ColumnMappings.Add(colID1,colID1);  // Assuming "ID" is the column name in your sourceTable
                                            bulkCopy.WriteToServer(importedRecords);
                                        }

                                    // Step 4: Perform the NOT IN query to find missing records in the Main table
                                    StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Comparaison des IDs...");
                                    string notInQuery = $@"
                                            SELECT * FROM dbo.{tableName} 
                                            WHERE {colID1} NOT IN (SELECT {colID1} FROM #TempSourceIDs)";
    
                                        DataTable missingRecords = new DataTable();
                                        using (SqlCommand notInCmd = new SqlCommand(notInQuery, sourceConnection))  // Use the sourceConnection to fetch missing records from the source DB
                                        {
                                            using (SqlDataAdapter adapter = new SqlDataAdapter(notInCmd))
                                            {
                                                adapter.Fill(missingRecords);  // Fill the missing records into DataTable
                                            }
                                        }

                                        // Step 5: Insert the missing records into the Main table
                                        if (missingRecords.Rows.Count > 0)
                                        {
                                        StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Transfert des ID manquants : {missingRecords.Rows.Count}...");
                                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                                            {
                                                bulkCopy.DestinationTableName = tableName;  // Replace with your Main table name
                                                bulkCopy.WriteToServer(missingRecords);     // Insert missing records into the destination (Main) table
                                            }
                                        }

                                    // Step 6: Drop the temp table after use
                                    StatusUpdate?.Invoke($"{tableName} - Vérification de la copie - Suppression de la table temporaire...");

                                    string dropTempTableQuery = "DROP TABLE #TempSourceIDs";
                                        using (SqlCommand dropTempTableCmd = new SqlCommand(dropTempTableQuery, sourceConnection))
                                        {
                                            dropTempTableCmd.ExecuteNonQuery();
                                        }

                                        
                                    
                                    //FIN DU CODE IMPORTE
                                }
                            }

                            else
                            {
                                // Deletes previous data from Destination database
                                string deleteQuery = $"DELETE FROM dbo.{tableName}";
                                SqlCommand deleteCommand = new SqlCommand(deleteQuery, destinationConnection);
                                StatusUpdate?.Invoke($"{tableName} - Suppression de l'ancienne table...");
                                deleteCommand.CommandTimeout = 120;
                                deleteCommand.ExecuteNonQuery();

                                // Select data from Buffer database
                                string selectQuery = $"SELECT * FROM dbo.{tableName}";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, sourceConnection);
                                selectCommand.CommandTimeout = 120;

                                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand))
                                {
                                    DataTable sourceData = new DataTable();
                                    StatusUpdate?.Invoke($"{tableName} - Insertion de la nouvelle table...");
                                    dataAdapter.Fill(sourceData);

                                    // Insert this source data into destination database on the corresponding data table
                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                                    {
                                        bulkCopy.BulkCopyTimeout = 120;
                                        bulkCopy.DestinationTableName = tableName;
                                        bulkCopy.WriteToServer(sourceData);
                                    }
                                }
                            }


                            //also add the date at which the table was updated in the CATALOG_TABLES table

                            string changeDateQuery = $"UPDATE CATALOG_TABLES SET Last_Transfer_Date = '{slastTransferDate}' WHERE Table_Name = '{tableName}'";
                            //TODO : update because we should use the catalog connection but it is already in use
                            SqlCommand changeDateCommand = new SqlCommand(changeDateQuery, mainConnection);
                            changeDateCommand.ExecuteNonQuery();

                            // Add the table to the list of updated tables
                            updatedTables.Add(tableName);
                        }

                    }
                    catalogReader.Close();
                    StatusUpdate?.Invoke(null);
                }


                if (asyncUpdate == false)
                {
                    //show in a message box the list of updated tables
                    string message = "Les tables suivantes ont été mises à jour : \n";
                    foreach (string table in updatedTables)
                    {
                        message += table + "\n";
                    }
                    MessageBox.Show(message);
                }
            });


        }


        // TODO: Chose if we use Using or declare connections in these 2 similar LoadData and LoadTable methods
        public static List<string> LoadDatabases()
        {
            List<string> DatabaseList = new List<string>();

            //// Connect to the server
            //string connectionString = "Server=" + ServerName + ";User ID=" + Properties.Settings.Default.login + ";Password=" + Properties.Settings.Default.password;
            //SqlConnection connection = new SqlConnection(connectionString);
            //try
            //{
            //    connection.Open();

            //    // Retrieve the list of databases
            //    DataTable databases = connection.GetSchema("Databases");
            //    foreach (DataRow database in databases.Rows)
            //    {
            //        string databaseName = database.Field<string>("database_name");
            //        DatabaseList.Add(databaseName);
            //    }
            //}
            ////catch (Exception ex)
            //{
            //MessageBox.Show("You don't have master access to the server : " + ex.Message);
            DatabaseList.Add("Hexoa Main");
            DatabaseList.Add("Hexoa Buffer");
            //}
            //finally
            //{
            //    connection.Close();
            //}

            return DatabaseList;
        }

        public static List<string> LoadDatatables(bool bBuffer = false)
        {
            // Load all tables from a database
            List<string> DatatableList = new List<string>();
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(sConnString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.tables order by name", connection);
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

        public static DataTable LoadData(string sCommand, bool bBuffer = false, Dictionary<string, object> parameters = null)
        {
            // Load all data from a specific SQL Command
            DataTable dt = new DataTable();
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            string orderByClause = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(sConnString))
                {
                    connection.Open();

                    // Remove ORDER BY clause if present at the end of the command
                    if (sCommand.TrimEnd().Contains("ORDER BY", StringComparison.OrdinalIgnoreCase))
                    {
                        int index = sCommand.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
                        orderByClause = sCommand.Substring(index);
                        sCommand = sCommand.Substring(0, index).Trim();
                    }
                    SqlCommand command = new SqlCommand(sCommand, connection);
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
                    command.CommandTimeout = 120;
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
            // Sort the DataTable on the user's computer if ORDER BY clause was found
            if (!string.IsNullOrEmpty(orderByClause) && dt.Rows.Count > 0)
            {
                // Extract column names and sorting directions from the ORDER BY clause
                string[] orderByColumns = orderByClause
                    .ToUpper()
                    .Replace("ORDER BY", "")
                    .Split(',')
                    .Select(s => s.Trim())
                    .ToArray();

                // Apply sorting to DataTable columns
                for (int i = orderByColumns.Length - 1; i >= 0; i--)
                {
                    string orderByColumn = orderByColumns[i];
                    string columnName = "";
                    string sortDirection = "";
                    if (orderByColumn.EndsWith(" ASC"))
                    {
                        columnName = orderByColumn.Replace(" ASC", "");
                        sortDirection = "ASC";
                    }
                    else if (orderByColumn.EndsWith(" DESC"))
                    {
                        columnName = orderByColumn.Replace(" DESC", "");
                        sortDirection = "DESC";
                    }
                    else
                    {
                        columnName = orderByColumn;
                        sortDirection = "ASC";
                    }

                    // Apply sorting to the DataTable
                    if (dt.Columns.Contains(columnName.Replace("[","").Replace("]","")))
                    {
                        dt.DefaultView.Sort = $"{columnName} {sortDirection}";
                        dt = dt.DefaultView.ToTable();
                    }
                }
            }

            return dt;
        }

        public static string GetID(string sTable, string sField, string sValue, bool bBuffer = false, string sWhereClause = "")
        {
            // Get the ID of a specific value in a specific table
            string sID = "0";
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(sConnString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM {sTable} WHERE {sField} = @Value";
                    if (!string.IsNullOrEmpty(sWhereClause)) { query += $" AND {sWhereClause}"; }
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Value", sValue.Trim());
                    SqlDataReader reader = command.ExecuteReader();
                    // store the first element of the reader as a string
                    if (reader.Read())
                    {
                        sID = reader[0].ToString();
                    }
                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while getting ID for {sField} as equal to {sValue} in {sTable}: {ex.Message}");
            }
            return sID;
        }

        public static string GetFieldFromID(string sTable, string sIDField, string sIDValue, string sField, bool bBuffer = false, string sWhereClause = "")
        {
            // Get the ID of a specific value in a specific table
            string sValue = "";
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(sConnString))
                {
                    connection.Open();
                    string query = $"SELECT {sField} FROM {sTable} WHERE {sIDField} = {sIDValue}";
                    if (sWhereClause != "") { query += $" AND {sWhereClause}"; }
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    //store the first element of the reader as a string
                    if (reader.Read())
                    {
                        sValue = reader[0].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while getting value for {sIDField} as equal to {sIDValue} in {sTable}: {ex.Message}");
            }
            return sValue;
        }

        public static string GetFieldFromField(string sTable, string sField, string sFieldValue, string sFieldToGet, bool bBuffer = false, string sWhereClause = "")
        {
            // Get the ID of a specific value in a specific table
            string sValue = "";
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(sConnString))
                {
                    connection.Open();
                    string query = $"SELECT {sFieldToGet} FROM {sTable} WHERE {sField} = @FieldValue";
                    if (!string.IsNullOrEmpty(sWhereClause)) { query += $" AND {sWhereClause}"; }
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FieldValue", sFieldValue.Trim());
                    SqlDataReader reader = command.ExecuteReader();
                    // store the first element of the reader as a string
                    if (reader.Read())
                    {
                        sValue = reader[0].ToString();
                    }
                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while getting the {sFieldToGet} value for {sField} as equal to {sValue} in {sTable}: {ex.Message}");
            }
            return sValue;
        }

        public static void ExecuteQuery(string query, Dictionary<string, object> parameters = null, bool bBuffer = false)
        {
            string sConnString = bBuffer ? BufferConnectionString() : MainConnectionString();
            using (SqlConnection connection = new SqlConnection(sConnString))
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

        public static string GetViewDefinition(string sqlQuery)
        {
            string query = $"SELECT VIEW_DEFINITION FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = '{sqlQuery}'";
            string viewText = LoadData(query).Rows[0][0].ToString();
            viewText = viewText.Replace($"CREATE VIEW dbo.{sqlQuery}\r\nAS\r\n","");
            viewText = viewText.Replace("TOP (100) PERCENT ", "");
            return viewText;
        }
        public static List<string[]> ExtractColumns(string sqlQuery)
        {
            string viewText = GetViewDefinition(sqlQuery);
            List<string[]> columns = new List<string[]>();

            // Utilisez une expression régulière pour extraire les noms de colonnes entre SELECT et FROM
            string pattern = @"SELECT\s+(.*?)\s+FROM";
            Match match = Regex.Match(viewText, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success)
            {
                string columnList = match.Groups[1].Value;
                columnList = columnList.Replace("dbo.", "").Replace("[", "").Replace("]", "").Replace("\r\n", "");

                // Split the string based on comma while considering parentheses
                string[] finalParts = SplitQueryColumns(columnList);

                foreach (string field in finalParts)
                {


                    // finalParts contient maintenant les morceaux divisés en tenant compte des parenthèses
                
                    string[] fieldSplit = field.Split(new string[] { " AS " }, StringSplitOptions.None);
                    string[] fieldInfo = new string[4];

                    if (BasicFunctions.CountStringInString(fieldSplit[0].Trim(),".")==1)
                    {
                        fieldInfo[0] = fieldSplit[0].Split('.')[0].Trim();
                        fieldInfo[1] = fieldSplit[0].Split('.')[1].Trim();
                    }
                    else
                    {
                        fieldInfo[1] = fieldSplit[0].Trim();
                        fieldInfo[0] = "No Table";
                    }

                    fieldInfo[2] = fieldSplit[1].Trim();
                    //store mono or multi in the type field if it is the start of the fieldinfo 2
                    if (fieldInfo[2].StartsWith("mono"))
                    {
                        fieldInfo[3] = "mono";
                        fieldInfo[2] = fieldInfo[2].Replace("mono_", "").Trim();
                    }
                    else if (fieldInfo[2].StartsWith("multi"))
                    {
                        fieldInfo[3] = "multi";
                        fieldInfo[2] = fieldInfo[2].Replace("multi_", "").Trim();
                    }
                    else if (fieldInfo[2].StartsWith("calcul"))
                    {
                        fieldInfo[3] = "calcul";
                        fieldInfo[2] = fieldInfo[2].Replace("calcul_", "").Trim();
                    }
                    else if (fieldInfo[2].StartsWith("icon"))
                    {
                        fieldInfo[3] = "icon";
                        fieldInfo[2] = fieldInfo[2].Replace("icon_", "").Trim();
                    }
                    else if (fieldInfo[2].StartsWith("hidden"))
                    {
                        fieldInfo[3] = "hidden";
                        fieldInfo[2] = fieldInfo[2].Replace("hidden_", "").Trim();
                    }
                    else
                    {
                        fieldInfo[3] = "";
                    }

                    columns.Add(fieldInfo);
                }
            }

            return columns;




        }

        private static string[] SplitQueryColumns(string input)
        {
            List<string> parts = new List<string>();
            int openParenthesesCount = 0;
            int closeParenthesesCount = 0;
            int startIndex = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    openParenthesesCount++;
                }
                else if (input[i] == ')')
                {
                    closeParenthesesCount++;
                }
                else if (input[i] == ',' && openParenthesesCount == closeParenthesesCount)
                {
                    parts.Add(input.Substring(startIndex, i - startIndex).Trim());
                    startIndex = i + 1;
                }
            }

            // Add the last part
            parts.Add(input.Substring(startIndex).Trim());

            return parts.ToArray();
        }
    }



}
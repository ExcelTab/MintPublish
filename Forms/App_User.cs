using Mint.Code;
using Mint.Controls;
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

namespace Mint.Forms
{
    public partial class App_User : Form
    {
        #region Local Variables
        DataTable dtViewDefinition = new DataTable();
        DoubleBufferedDGV dataGridView_Selected;
        string viewQuery;
        string viewQuerySQL;
        #endregion

        public App_User()
        {
            InitializeComponent();
            dataGridView_Selected = dataGridView_Users;
            viewQuery = "ViewUsers";

            //Set up the View Definition Datatable to display monos, multis, and save types
            dtViewDefinition.Columns.Add("Table", typeof(string));
            dtViewDefinition.Columns.Add("Column", typeof(string));
            dtViewDefinition.Columns.Add("Alias", typeof(string));
            dtViewDefinition.Columns.Add("Select", typeof(string));
        }

        #region Procedures to launch on start
        private void LoadViewDefinition()
        {
            //Retrieve the exact SQL query from the view
            viewQuerySQL = Database.GetViewDefinition(viewQuery);
            //store the columns in a datatable
            dtViewDefinition.Clear();
            List<string[]> columns = Database.ExtractColumns(viewQuery);
            foreach (string[] column in columns)
            {
                dtViewDefinition.Rows.Add(column);
            }
        }
        #endregion

        #region Overall procedures used accross the App
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Change the selected dgv and query
            int selectedIndex = tabControl1.SelectedIndex;
            switch (selectedIndex)
            {
                case 0: // Liste des utilisateurs Mint
                    dataGridView_Selected = dataGridView_Users;
                    viewQuery = "ViewUsers";
                    break;
                case 1: // Liste des Ateliers de production
                    dataGridView_Selected = dataGridView_Ateliers;
                    viewQuery = "ViewAteliers";
                    break;
                case 2: // Liste des Artistes
                    dataGridView_Selected = dataGridView_Artistes;
                    viewQuery = "ViewArtists";
                    break;

                default:
                    // No change
                    return;
            }
        }
        #endregion

        #region Procedures to manage the list views in DGVs
        private void Refresh_List()
        {
            //TODO : permettre au user de sauver son layout (ordre des colonnes, largeur des colonnes, etc)

            //Load the ViewProducts query in the datagridview on the product list tabpage
            this.Cursor = Cursors.WaitCursor;

            // Select the appropriate datagridView

            //string query = $"SELECT * FROM {viewQuery} ORDER BY calcul_ID";
            // Initialize the DataTable
            DataTable dtToTreat = Database.LoadData(viewQuerySQL);
            DataTable dt = dtToTreat.Clone();

            // Make first column accept string and int values (for NewValues)
            dt.Columns[0].DataType = typeof(string);

            // Make all columns nullable
            foreach (DataColumn column in dt.Columns)
            {
                column.AllowDBNull = true;
            }

            foreach (DataRow row in dtToTreat.Rows)
            {
                DataRow newRow = dt.NewRow();
                newRow.ItemArray = row.ItemArray;
                dt.Rows.Add(newRow);
            }

            //Treat all the columns of the Data Table
            int columnCount = dt.Columns.Count; // Stocke le nombre de colonnes
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                DataColumn column = dt.Columns[columnIndex];
                if (column.ColumnName.StartsWith("icon_"))
                {
                    DataColumn newImageColumn = new DataColumn(column.ColumnName.Replace("icon_", ""), typeof(Image));
                    dt.Columns.Add(newImageColumn);
                    newImageColumn.SetOrdinal(columnIndex + 1);
                    foreach (DataRow row in dt.Rows)
                    {
                        int is_Detail = Convert.ToInt32(row[column.ColumnName]);
                        Image detailImage = is_Detail == 1 ? Properties.Resources.mini_detail : null;
                        row[newImageColumn] = detailImage;
                    }

                    dt.Columns.RemoveAt(columnIndex);
                }

                column.ColumnName = column.ColumnName.Replace("mono_", "").Replace("multi_", "").Replace("calcul_", "").Replace("hidden_", "");
            }

            // Bind the DataTable to the DataGridView
            dataGridView_Selected.DataSource = dt;

            //Format the datagridview
            foreach (DataGridViewColumn column in dataGridView_Selected.Columns)
            {
                // Change format of Money columns 
                if (column.HeaderText.StartsWith("Cout") || column.HeaderText.StartsWith("Prix"))
                {
                    column.DefaultCellStyle.Format = "C2"; // Format en euros (Money)
                }
                // Change format of Ratio columns
                else if (column.HeaderText.StartsWith("Ratio") || column.HeaderText.StartsWith("%"))
                {
                    column.DefaultCellStyle.Format = "P0"; // Format en pourcentage (Percent)
                }


                //find the correspondance in the queryviewproductsdefinition
                DataRow[] rows = dtViewDefinition.Select($"Alias = '{column.Name}'");
                if (rows.Length > 0)
                {
                    //TODO : add legend on the form to explain the colors
                    string columnType = rows[0]["Select"].ToString();
                    if (columnType == "multi")
                    {
                        column.DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    }
                    else if (columnType == "mono")
                    {
                        column.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else if (columnType == "calcul")
                    {
                        column.DefaultCellStyle.BackColor = Color.LightGray;
                        column.DefaultCellStyle.ForeColor = Color.Gray;
                        column.ReadOnly = true;
                    }
                    else if (columnType == "icon")
                    {
                        column.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else if (columnType == "hidden")
                    {
                        column.Visible = false;

                    }

                }
            }
            this.Cursor = Cursors.Default;
        }
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            dataGridView_Selected.pauseEvents = true;
            LoadViewDefinition();
            dataGridView_Selected.ClearUnsavedData();
            Refresh_List();
            dataGridView_Selected.pauseEvents = false;
        }
        private void button_SaveList_Click(object sender, EventArgs e)
        {
            dataGridView_Selected.pauseEvents = true;

            //Save the values from the datagridview in the database
            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    MessageBox.Show("Impossible de se connecter à la base de données. Vérifiez votre connection Internet puis recommencez.");
                    dataGridView_Selected.pauseEvents = false;
                    return;
                }
                LoadViewDefinition();
                //Create a dictionnary of 2 strings to store new ids
                Dictionary<string, string> newIDs = new Dictionary<string, string>();
                List<string> deletedIDs = new List<string>();

                foreach (DataRow change in dataGridView_Selected.recordedChanges.Rows)
                {
                    string ID = (string)change["ID"];
                    string NewID = ""; //to store potential new IDs when using insert
                    string columnName = (string)change["ColumnName"];
                    object newValue = change["NewValue"];

                    //Skip if ID is in deletedIDs
                    if (deletedIDs.Contains(ID) || ID == "")
                    {
                        continue;
                    }

                    //Get the row of queryViewProductsDefinition where Alias is the column name
                    DataRow[] queryDefRow = dtViewDefinition.Select($"Alias = '{columnName}'");
                    string tableName = (string)queryDefRow[0]["Table"];
                    string fieldName = (string)queryDefRow[0]["Column"];
                    string selectType = (string)queryDefRow[0]["Select"];

                    tableName = tableName.Replace("_1", "").Replace("_2", "").Replace("_3", "");

                    string baseTableName = "";
                    string idField = "";
                    switch (viewQuery)
                    {
                        case "ViewUsers":
                            baseTableName = "TBL_USERS";
                            idField = "id_user";
                            break;
                        case "ViewAteliers":
                            baseTableName = "TBL_ATELIER";
                            idField = "id_atelier";
                            break;
                        case "ViewArtists":
                            baseTableName = "TBL_MANUFACTURER_EXTENDED";
                            idField = "id_manufacturer";

                            //check if ID exists in Extended, and if not, then create it
                            string selectQuery = $"SELECT COUNT(*) FROM {baseTableName} WHERE {idField} = {ID}";
                            using (SqlCommand command = new SqlCommand(selectQuery, connection))
                            {
                                int count = (int)command.ExecuteScalar();
                                if (count == 0)
                                {
                                    //Create the new line
                                    string insertQuery = $"INSERT INTO {baseTableName} (id_manufacturer, date_add) VALUES ({ID}, GETDATE())";
                                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                                    {
                                        insertCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            break;
                    }

                    //if ID is new, then first create a new line and get the ID
                    if (ID.StartsWith("NewID"))
                    {
                        //Check to see if the ID is inside the Dictionnary
                        if (newIDs.ContainsKey(ID))
                        {
                            ID = newIDs[ID];
                        }
                        else
                        {
                            //Create the new line
                            string insertQuery = $"INSERT INTO {baseTableName}";
                            switch (viewQuery)
                            {
                                case "ViewArtists":
                                    MessageBox.Show($@"Ajoutez de nouveaux artistes depuis Prestashop, puis fermez et rouvrez Mint pour les importer dans cette vue.");
                                    dataGridView_Selected.pauseEvents = false;
                                    continue;
                                case "ViewAteliers":
                                case "ViewUsers":
                                    insertQuery += $" (date_add) VALUES (GETDATE())";
                                    break;
                            }
                            insertQuery += "; SELECT SCOPE_IDENTITY()";
                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                try
                                {
                                    if (viewQuery == "ViewArtists")
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        NewID = command.ExecuteScalar().ToString();
                                        newIDs.Add(ID, NewID);
                                        ID = NewID;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"La commande {insertQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                    continue;
                                }
                            }
                        }
                    }
                    
                    
                    //Create the update query
                    string updateQuery = "";

                    //Check if this is a modification on the product ID
                    if (fieldName == idField)
                    {
                        //No need to manage NEW as already taken into account above
                        if (newValue.ToString().StartsWith("Delete_"))
                        {
                            //Depends on the data that is displayed.
                            string IDtoDelete = newValue.ToString().Replace("Delete_", "");
                            List<string> deleteQueries = new List<string>();
                            switch (viewQuery)
                            {
                                case "ViewUsers":
                                    deleteQueries.Add("DELETE FROM TBL_USERS WHERE id_user = " + IDtoDelete);
                                    break;
                                case "ViewAteliers":
                                    deleteQueries.Add("DELETE FROM TBL_ATELIERS WHERE id_atelier = " + IDtoDelete);
                                    break;
                                case "ViewArtists":
                                    MessageBox.Show($@"Choisissez les artistes comme inactifs depuis Prestashop, puis fermez et rouvrez Mint pour les supprimer de cette vue.");
                                    dataGridView_Selected.pauseEvents = false;
                                    continue;
                            }
                            foreach (string deleteQuery in deleteQueries)
                            {
                                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                                {
                                    try
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"La commande {deleteQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                        continue;
                                    }
                                }
                            }
                            //add the ID to the deletedId list
                            deletedIDs.Add(IDtoDelete);
                        }
                    }
                    else
                    {
                        if (selectType == "") //Simple update to the tables
                        {
                            updateQuery = $"UPDATE {baseTableName} SET {fieldName} = @newValue, date_update = GETDATE() WHERE {idField} = {ID}";
                            using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            {
                                try
                                {
                                    command.Parameters.AddWithValue("@newValue", newValue);
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"La commande {updateQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                    continue;
                                }
                            }
                        }
                        else //Update to another table (features, categories, etc.)
                        {
                            if (tableName.StartsWith("TBL")) //get id of that table and store it in the base table
                            {
                                string idColumn = "";
                                switch (columnName)
                                {
                                    case "Artiste": idColumn = "id_manufacturer"; break;
                                    case "Thème": idColumn = "id_category_default"; break;
                                    case "Mot clé": idColumn = "id_keyword"; break;
                                    case "Lien maillage": idColumn = "id_website_url"; break;
                                    case "Couleur dominante": idColumn = "id_color_default"; break;
                                    case "Type de produit": idColumn = "id_type"; break;
                                    case "Format": idColumn = "id_format"; break;
                                    case "Support": case "Support unique": idColumn = "id_support"; break;
                                    case "Dimension": case "Dimension unique": idColumn = "id_dimension"; break;
                                    case "Atelier": idColumn = "id_atelier"; break;
                                    case "Article": case "Article unique": idColumn = "id_article"; break;

                                }
                                string join_id = Database.GetID(tableName, fieldName, newValue.ToString());
                                if (join_id != "0" || newValue.ToString() == "")
                                {
                                    updateQuery = $"UPDATE {baseTableName} SET {idColumn} = @id, date_update = GETDATE() WHERE {idField} = {ID}";
                                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                    {
                                        try
                                        {
                                            command.Parameters.AddWithValue("@id", join_id);
                                            command.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($"La commande {updateQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Refresh_List();
            dataGridView_Selected.pauseEvents = false;
            dataGridView_Selected.newIDCounter = 1;
            MessageBox.Show(dataGridView_Selected.recordedChanges.Rows.Count + " changements enregistrés !", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information); //Show a message box to confirm the changes have been saved
            dataGridView_Selected.recordedChanges.Clear(); // Clear the recorded changes after updating the database


        }
        private void selectDGVRow(object sender, EventArgs e)
        {
            if (dataGridView_Selected.pauseEvents == true) { return; }
            if (dataGridView_Selected.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView_Selected.SelectedCells[0].RowIndex;
                if (rowIndex == -1) { return; }
                if (rowIndex != dataGridView_Selected.previousSelectedRowIndex)
                {
                    DataGridViewRow selectedRow = dataGridView_Selected.Rows[rowIndex];
                    dataGridView_Selected.previousSelectedRowIndex = rowIndex;

                }
            }
        }
        private void doubleClickDGVCell(object sender, DataGridViewCellEventArgs e)
        {
            //find the correspondance in the queryviewproductsdefinition
            bool bShowDialog = false;
            bool bMultiSelect = false;
            string sTable = "";
            string sAlias = "";
            string idArticle = dataGridView_Selected.Rows[e.RowIndex].Cells[0].Value.ToString();
            string sDeclinaison = dataGridView_Selected.Rows[e.RowIndex].Cells[1].Value.ToString();

            DataRow[] rows = dtViewDefinition.Select($"Alias = '{dataGridView_Selected.Columns[e.ColumnIndex].Name}'");

            if (rows.Length > 0)
            {
                //TODO : add legend on the form to explain the colors
                sTable = rows[0]["Table"].ToString();
                sAlias = rows[0]["Alias"].ToString();
                string SelectType = rows[0]["Select"].ToString();
                if (SelectType == "multi")
                {
                    bShowDialog = true;
                    bMultiSelect = true;
                }
                else if (SelectType == "mono")
                {
                    bShowDialog = true;
                    bMultiSelect = false;
                }
            }

            if (bShowDialog)
            {
                BlackOverlay blackOverlay = new BlackOverlay();
                blackOverlay.Show();

                ListTable list_Select = new ListTable();

                //TODO : mettre ceci dans un bloc plus propre
                switch (sAlias)
                {
                    case "Thème":
                        list_Select.ViewMode = "TreeView";
                        list_Select.MultiSelect = false;
                        list_Select.Load_List("SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position");
                        list_Select.Set_Title("Choix de catégorie");
                        break;

                    default:
                        list_Select.ViewMode = "ListView";
                        list_Select.MultiSelect = false;
                        list_Select.Load_List($"SELECT * FROM {sTable} order by name Asc");
                        list_Select.Set_Title(sTable.Replace("TBL_", ""));
                        break;

                }
                list_Select.ShowDialog();

                if (list_Select.Text_Selected != null)
                {
                    // Get the corresponding cell in the DataGridView
                    DataGridViewCell cell = dataGridView_Selected.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Set the cell value and tag
                    cell.Value = list_Select.Text_Selected;
                }

                //exit the cell editing
                blackOverlay.Close();
                dataGridView_Selected.EndEdit();
            }

        }
        private void changeDGVCell(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Récupère la colonne du DataGridView par son index de colonne
                DataGridViewColumn column = dataGridView_Selected.Columns[e.ColumnIndex];
                string columnName = column.Name;
                string ID = "";
                DataGridViewCell idCell = dataGridView_Selected.Rows[e.RowIndex].Cells[0];

                if (idCell.Value != null) { ID = idCell.Value.ToString(); }

                if (ID == "")
                {
                    ID = "NewID_" + dataGridView_Selected.newIDCounter.ToString();
                    dataGridView_Selected.CellValueChanged -= changeDGVCell;
                    dataGridView_Selected.Rows[e.RowIndex].Cells[0].Value = ID;
                    dataGridView_Selected.CellValueChanged += changeDGVCell;
                    dataGridView_Selected.newIDCounter++;
                }

                object newValue = dataGridView_Selected.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                //Remplacer les points-virgules par des virgules (pour éviter les problèmes lors de la génération du CSV
                if (newValue != null && newValue.GetType() == typeof(string)) { newValue = newValue.ToString().Replace(";", ","); }
                dataGridView_Selected.recordedChanges.Rows.Add(ID, columnName, newValue);

                //Change cell forecolor to blue and set the text to italic
                dataGridView_Selected.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                dataGridView_Selected.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font(dataGridView_Selected.Font, FontStyle.Italic);

            }
        }
        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+V
            if (e.Control && e.KeyCode == Keys.V)
            {
                // Obtient le contenu du presse-papiers
                string clipboardText = Clipboard.GetText();

                // Sépare les lignes du contenu du presse-papiers
                string[] lines = clipboardText.Split(new[] { "\r\n", "\r" }, StringSplitOptions.None);
                //Remove last item if blank
                if (lines[lines.Length - 1] == "") { lines = lines.Take(lines.Length - 1).ToArray(); }

                //Vérifie si on est sur une seule cellule ou sur plusieurs
                if (dataGridView_Selected.SelectedCells.Count == 1)
                {
                    // Récupère la cellule active
                    DataGridViewCell cell = dataGridView_Selected.CurrentCell;
                    int rowIndex = cell.RowIndex;
                    int columnIndex = cell.ColumnIndex;
                    bool addNewRow = false;

                    // Obtenez la DataTable liée au DataGridView
                    DataTable dt = (DataTable)dataGridView_Selected.DataSource;

                    // Boucle sur les lignes du presse-papiers
                    foreach (string line in lines)
                    {
                        // Sépare les colonnes de chaque ligne
                        string[] values = line.Split('\t'); // Vous pouvez utiliser '\t' si les données sont tabulées

                        // Assurez-vous qu'il y a suffisamment de colonnes dans le DataGridView
                        if (columnIndex + values.Length <= dataGridView_Selected.ColumnCount)
                        {
                            //Si on est au bout du DGV, on ajoute une ligne
                            if (rowIndex == dataGridView_Selected.RowCount - 1) { addNewRow = true; };

                            if (addNewRow)
                            {
                                DataRow newRow = dt.NewRow();
                                dataGridView_Selected.pauseEvents = true;
                                dt.Rows.Add(newRow);
                                dataGridView_Selected.pauseEvents = false;
                            }
                            // Boucle sur les valeurs et colle-les dans les cellules appropriées
                            for (int i = 0; i < values.Length; i++)
                            {
                                dataGridView_Selected[columnIndex + i, rowIndex].Value = values[i];
                            }
                            //if (bNewRow) 
                            //{
                            //    // Access the header cell of the first column
                            //    //DataGridViewColumn firstColumn = dataGridView_Selected.Columns[0];

                            //    //// Check if the header cell is of your custom type
                            //    //if (firstColumn.HeaderCell is CustomColumnHeaderCell customHeaderCell)
                            //    //{
                            //    //    // Call the Filter method on your custom header cell
                            //    //    customHeaderCell.Filter();
                            //    //    dataGridView_Selected.Refresh();
                            //    //}
                            //}
                            // Passe à la ligne suivante si nécessaire
                        }
                        rowIndex++;

                    }
                }
                else if (lines.Length == 1)
                {
                    string[] values = lines[0].Split('\t'); // Vous pouvez utiliser '\t' si les données sont tabulées
                    //Copier la ligne pour toutes les cellules sélectionnées
                    foreach (DataGridViewCell cell in dataGridView_Selected.SelectedCells)
                    {
                        int rowIndex = cell.RowIndex;
                        int columnIndex = cell.ColumnIndex;
                        for (int i = 0; i < values.Length; i++)
                        {
                            dataGridView_Selected[columnIndex + i, rowIndex].Value = values[i];
                        }
                    }
                }



            }
            // Ctrl+C
            else if (e.Control && e.KeyCode == Keys.C)
            {
                StringBuilder clipboardText = new StringBuilder();

                int columnIndex = -1; // Utilisé pour détecter le changement de colonne
                foreach (DataGridViewCell cell in dataGridView_Selected.SelectedCells)
                {
                    // Si nous changeons de colonne, nous ajoutons une tabulation
                    if (cell.ColumnIndex != columnIndex)
                    {
                        clipboardText.Append('\t');
                        columnIndex = cell.ColumnIndex;
                    }

                    clipboardText.Append(cell.Value);
                    clipboardText.Append('\t'); // Utilisez '\t' pour séparer les valeurs par des tabulations
                }

                clipboardText.Replace("\t\r\n", "\r\n"); // Remplace la dernière tabulation par un saut de ligne

                Clipboard.SetText(clipboardText.ToString());
            }

            // Delete
            else if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in dataGridView_Selected.SelectedCells)
                {
                    //Detect if we are in an ID column
                    if (cell.OwningColumn.Name == "ID")
                    {
                        //Depends on the data that is displayed.
                        int ID = Convert.ToInt32(cell.Value);
                        switch (viewQuery)
                        {
                            case "ViewProducts":
                                //Check if product is integrated in Prestashop, so if product id > 9000000
                                //Check if the is_Prestashop column of the same row of cell is checked
                                bool isPrestashop = Convert.ToBoolean(dataGridView_Selected.Rows[cell.RowIndex].Cells["Intégré Prestashop"].Value);
                                if (isPrestashop)
                                {
                                    MessageBox.Show("Le produit n°" + ID.ToString() + " est intégré dans Prestashop. Vous ne pouvez pas le supprimer.", "Supression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    cell.Value = "Delete_" + cell.Value;
                                }
                                break;

                            case "ViewDeclinaisons":
                                //Check if this article was already sold at least once
                                string query_article = "SELECT COUNT(*) FROM TBL_SALES WHERE id_article = " + ID;
                                DataTable dt = Database.LoadData(query_article);
                                int nbVente = Convert.ToInt32(dt.Rows[0][0]);
                                if (nbVente > 0)
                                {
                                    MessageBox.Show("La déclinaison n°" + ID.ToString() + " a déjà été vendue " + nbVente.ToString() + " fois. Vous ne pouvez pas la supprimer.", "Supression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    //Also Check if this article is flagged as basic article for a product
                                    query_article = "SELECT COUNT(*) FROM TBL_NEWPRODUCT WHERE id_article = " + ID;
                                    dt = Database.LoadData(query_article);
                                    int nbProduct = Convert.ToInt32(dt.Rows[0][0]);
                                    if (nbProduct > 0)
                                    {
                                        MessageBox.Show("La déclinaison n°" + ID.ToString() + " est référencée comme article unique dans " + nbVente.ToString() + " produits. Vous ne pouvez pas la supprimer.", "Supression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        cell.Value = "Delete_" + cell.Value;
                                    }
                                }

                                break;
                            case "ViewRawMat":
                                //Check if this raw material is in a recipe
                                string query_rawmat = "SELECT COUNT(*) FROM TBL_RECETTES WHERE id_rawmat = " + ID;
                                DataTable dtr = Database.LoadData(query_rawmat);
                                int nbRecette = Convert.ToInt32(dtr.Rows[0][0]);
                                if (nbRecette > 0)
                                {
                                    MessageBox.Show("La matière première n°" + ID.ToString() + " est incluse dans " + nbRecette.ToString() + " recettes. Modifiez ces recettes avant de supprimer cette matière première.", "Supression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    cell.Value = "Delete_" + cell.Value;
                                }
                                break;
                            case "ViewURL":
                            case "ViewNegoce":
                                //No checks to perform
                                cell.Value = "Delete_" + cell.Value;
                                break;
                            default:
                                //do nothing
                                break;
                        }
                    }
                    else
                    {
                        cell.Value = null;
                    }

                }
            }

            // Ctrl+F
            else if (e.Control && e.KeyCode == Keys.F)
            {
                string searchValue = "";
                // Show search form
                using (InputBox formSearch = new InputBox())
                {
                    formSearch.Text = "Rechercher";
                    formSearch.label_Title = "Encodez une partie du contenu de la cellule à trouver";
                    formSearch.ShowDialog();
                    searchValue = formSearch.UserInput;
                }
                if (searchValue != "")
                {
                    // Search for the value in the DataGridView
                    foreach (DataGridViewRow row in dataGridView_Selected.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && cell.Value.ToString().Contains(searchValue))
                            {
                                cell.Selected = true;
                                dataGridView_Selected.FirstDisplayedScrollingRowIndex = cell.RowIndex;
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void DGV_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Annule l'erreur pour éviter l'affichage du message d'erreur par défaut
            e.Cancel = true;
            if (dataGridView_Selected.pauseEvents == true) { return; }
            if (e.ColumnIndex == -1) { return; }
            if (e.RowIndex > dataGridView_Selected.Rows.Count) { return; }
            if (dataGridView_Selected[0, e.RowIndex].Value is null) { return; }

            bool isNullableColumn = dataGridView_Selected.Columns[e.ColumnIndex].ValueType == typeof(int?);

            // Affiche un message d'erreur personnalisé si la colonne est nullable
            if (isNullableColumn)
            {
                dataGridView_Selected.CancelEdit(); // Annule l'édition en cours
                dataGridView_Selected[e.ColumnIndex, e.RowIndex].Value = dataGridView_Selected[e.ColumnIndex, e.RowIndex].DefaultNewRowValue; // Rétablit la valeur par défaut
            }
            else
            {

                // Affiche un message d'erreur personnalisé
                MessageBox.Show("Cette donnée n'est pas au format attendu : " + e.Exception.Message, "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Error);

                dataGridView_Selected.CancelEdit(); // Annule l'édition en cours
                dataGridView_Selected[e.ColumnIndex, e.RowIndex].Value = dataGridView_Selected[e.ColumnIndex, e.RowIndex].Value; // Rétablit la valeur précédente
            }

        }
        #endregion
    }
}

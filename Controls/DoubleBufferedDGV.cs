using iText.Layout.Borders;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using static iText.Svg.SvgConstants;


namespace Mint.Controls
{
    public class DoubleBufferedDGV : DataGridView
    {
        public Dictionary<string, string> filters = new Dictionary<string, string>();
        public bool pauseEvents = false;
        public event EventHandler FiltersChanged;
        public int previousSelectedRowIndex = -1;
        public DataTable recordedChanges = new DataTable();
        public int newIDCounter = 1;


        public DoubleBufferedDGV()
        {
            DoubleBuffered = true;
            pauseEvents = false;
            // Assurez-vous que la colonne de votre DataGridView utilise CustomColumnHeaderCell
            this.ColumnAdded += DoubleBufferedDataGridView_ColumnAdded;
            this.ColumnHeadersHeight = 40;

            //Set up the product Change Datatable to record all changes
            recordedChanges.Columns.Add("ID", typeof(string));
            recordedChanges.Columns.Add("ColumnName", typeof(string));
            recordedChanges.Columns.Add("NewValue", typeof(object));
        }

        public void ClearUnsavedData()
        {
            previousSelectedRowIndex = -1;
            newIDCounter = 1;
            recordedChanges.Clear();
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            //Ajouter les bordures sur le DGV pour la ligne sélectionnée
            if (pauseEvents == false)
            {

                if (this.SelectionMode == DataGridViewSelectionMode.CellSelect)
                {
                    if (this.SelectedCells.Count > 0)
                    {
                        int rowIndex = this.SelectedCells[0].RowIndex;
                        if (rowIndex != previousSelectedRowIndex)
                        {
                            if (previousSelectedRowIndex >= 0 && previousSelectedRowIndex < this.Rows.Count)
                            {
                                this.Rows[previousSelectedRowIndex].DividerHeight = 0;
                                if (previousSelectedRowIndex >= 1)
                                {
                                    this.Rows[previousSelectedRowIndex - 1].DividerHeight = 0;
                                }
                            }
                            DataGridViewRow selectedRow = this.Rows[rowIndex];
                            selectedRow.DividerHeight = 4;
                            if (rowIndex >= 1)
                            {
                                this.Rows[rowIndex - 1].DividerHeight = 4;
                            }
                            base.OnSelectionChanged(e);
                            previousSelectedRowIndex = rowIndex;
                        }
                    }
                    else
                    {
                        if (previousSelectedRowIndex >= 0 && previousSelectedRowIndex < this.Rows.Count)
                        {
                            this.Rows[previousSelectedRowIndex].DividerHeight = 0;
                            if (previousSelectedRowIndex >= 1)
                            {
                                this.Rows[previousSelectedRowIndex - 1].DividerHeight = 0;
                            }
                        }
                        previousSelectedRowIndex = -1;
                    }
                }
                else { base.OnSelectionChanged(e); }
            }

        }
        public void RecolorFilteredColumns()
        {
            foreach (DataGridViewColumn column in this.Columns)
            {
                if (filters.ContainsKey(column.DataPropertyName))
                {
                    column.HeaderCell.Style.BackColor = Color.LightGreen;
                }
                else
                {
                    column.HeaderCell.Style.BackColor = Color.White;
                }
            }
        }
        private void DoubleBufferedDataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.HeaderCell = new CustomColumnHeaderCell();
        }
    }
    public class CustomColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        private bool isFiltered;
        private Button filterButton;
        public bool IsFiltered
        {
            get { return isFiltered; }
            set
            {
                if (isFiltered != value)
                {
                    isFiltered = value;
                    UpdateHeaderColor();
                }
            }
        }
        public CustomColumnHeaderCell()
        {

        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            ((DoubleBufferedDGV)this.DataGridView).pauseEvents = true;
            if (e.Button == MouseButtons.Right)
            {
                // Affichez le menu contextuel ici.

                ShowFilterMenu();
                
            }
            else
            {
                base.OnMouseDown(e);
            }
            ((DoubleBufferedDGV)this.DataGridView).pauseEvents = false;
        }

        public void UpdateHeaderColor()
        {
            ((DoubleBufferedDGV)this.DataGridView).RecolorFilteredColumns();
        }

        private DataTable GetColumnData(string columnName)
        {
            if (this.DataGridView.DataSource is DataTable dataTable)
            {
                DataColumn column = dataTable.Columns[columnName];

                if (column != null)
                {
                    var distinctData = (from DataRow row in dataTable.Rows
                                        select row[columnName]).Distinct();

                    DataTable resultTable = new DataTable();
                    resultTable.Columns.Add(columnName);

                    foreach (var item in distinctData)
                    {
                        resultTable.Rows.Add(item);
                    }

                    //Order resultTable ascending on column 0
                    DataView dv = resultTable.DefaultView;
                    dv.Sort = columnName + " ASC";

                    return resultTable;
                }
                else { return new DataTable(); }
            }
            else { return new DataTable(); }
        }

        private void ShowFilterMenu()
        {
            // Créez le formulaire popup en passant les données de la colonne.
            var columnName = this.OwningColumn.DataPropertyName; // Obtenez le nom de la colonne.
            DataTable dt = GetColumnData(columnName);
  
            // Calculez les coordonnées du formulaire en fonction du header cell.
            var form = new Mint.Forms.DGVFilterMenu(); // Remplacez par le nom de votre formulaire.
            int columnIndex = this.OwningColumn.Index; // Obtenez l'indice de la colonne cliquée.
            Rectangle cellDisplayRectangle = this.DataGridView.GetCellDisplayRectangle(columnIndex, -1, false);
            int formX = cellDisplayRectangle.Right + 2 - form.Width;
            int formY = cellDisplayRectangle.Bottom;
            form.Location = new Point(formX, formY);

            // Affichez le formulaire.
            form.Text = this.OwningColumn.HeaderText;
            form.Location = this.DataGridView.PointToScreen(new Point(formX, formY));
            form.itemDT = dt;
            form.LoadList(dt);


            form.FormClosed += (sender, e) =>
            {
                ((DoubleBufferedDGV)this.DataGridView).pauseEvents = true;
                if (form.sort == "ASC" || form.sort == "DESC")
                {
                    SortBy(form.sort);
                    Filter();
                }
                               
                else if (form.removeAllFilters)
                {
                    RemoveAllFilters();
                    Filter();
                }
                else if (form.removeThisFilter)
                {
                    RemoveThisFilter();
                    Filter();
                }

                else if(form.selectedItems.Count>0)
                {
                    AddFilter(form.selectedItems);
                    Filter();
                }

                else if (form.extractList)
                {
                    form.extractList = false;
                    Filter();
                    ExtractList();
                    
                }
                ((DoubleBufferedDGV)this.DataGridView).pauseEvents = false;
            };

            form.Show();
        }

        public void SortBy(string sortOrder)
        {
            if (this.DataGridView.DataSource is DataTable dataTable)
            {
                this.DataGridView.Cursor = Cursors.WaitCursor;
                string columnName = this.OwningColumn.DataPropertyName;
                DataColumn column = dataTable.Columns[columnName];
                //only do if column type is string

                //TODO : réparere ceci, ça ne marchait pas sur les colonnes calcul_ID
                //                if (column.DataType == typeof(string))
                //                {

                //                    // Cloner la DataTable
                //                    DataTable numericDataTable = dataTable.Clone();
                //                    DataTable alphaNumericDataTable = dataTable.Clone();
                //                    // Ajouter une colonne temporaire pour stocker les valeurs numériques
                //                    numericDataTable.Columns.Add(columnName + "_TEMPNUM", typeof(double));

                //                    foreach (DataRow row in dataTable.Rows)
                //                    {
                //                        // Vérifiez si la valeur est numérique ou non
                //                        string value = row[columnName].ToString();
                //                        if (Code.BasicFunctions.IsNumeric(value))
                //                        {
                //                            // Ajoutez la ligne avec la valeur numérique
                //                            DataRow newRow = numericDataTable.NewRow();
                //                            newRow.ItemArray = row.ItemArray; // Copiez toutes les colonnes
                //                            newRow[columnName + "_TEMPNUM"] = Convert.ToDouble(value.Trim())+1;
                //                            numericDataTable.Rows.Add(newRow);
                //                        }
                //                        else
                //                        {
                //                            // Ajoutez la ligne avec la valeur alphanumérique
                //                            alphaNumericDataTable.ImportRow(row);
                //                        }
                //                    }

                //                    // Triez les DataTables sur la colonne temporaire
                //                    numericDataTable.DefaultView.Sort = string.Empty;
                //                    numericDataTable.DefaultView.Sort = "[" + columnName + "_TEMPNUM] " + sortOrder;
                //                    alphaNumericDataTable.DefaultView.Sort = string.Empty;
                //                    alphaNumericDataTable.DefaultView.Sort = "[" + columnName + "] " + sortOrder;

                //                    // Supprimez la colonne temporaire
                //                    numericDataTable.Columns.Remove(columnName + "_TEMPNUM");

                //                    // Fusionnez les DataTables triées
                //                    dataTable.Rows.Clear();

                //                    if (sortOrder == "ASC")
                //                    {
                //                        dataTable.Merge(numericDataTable);
                //                        dataTable.Merge(alphaNumericDataTable);
                //                    }
                //                    else
                //                    {
                //                        dataTable.Merge(alphaNumericDataTable);
                //                        dataTable.Merge(numericDataTable);
                //                    }
                //;

                //                    // Utilisez la DataTable fusionnée pour mettre à jour le DataGridView
                //                    this.DataGridView.DataSource = dataTable;
                //                }
                //                else
                //                {
                //                    // Triez normalement ASC ou DESC
                //                    dataTable.DefaultView.Sort = "[" + columnName + "] " + sortOrder;

                //                }
                dataTable.DefaultView.Sort = "[" + columnName + "] " + sortOrder;
                this.DataGridView.Cursor = Cursors.Default;
          }
        }


        public void ExtractList()
        {
            //Copie les éléments affichés de la table et les envoie en excl via la class XLSX
            if (this.DataGridView.DataSource is DataTable dataTable)
            {
                // Créez une nouvelle DataTable pour stocker les éléments affichés
                DataTable displayedItems = dataTable.Clone();
                DataView view = dataTable.DefaultView;

                // Parcourez les lignes de la DataView et ajoutez-les à la nouvelle DataTable
                foreach (DataRowView rowView in view)
                {
                    DataRow row = rowView.Row;
                    if (row.RowState != DataRowState.Deleted)
                    {
                        displayedItems.ImportRow(row);
                    }
                }

                // Créez un objet XLSX et exportez la DataTable
                Code.XLSX.OpenInExcel(displayedItems);
            }
        }


        public void Filter()
        {
            // Loop through all filters and apply them to the parent DataGridView
            if (this.DataGridView.DataSource is DataTable dataTable)
            {
                DataView view = dataTable.DefaultView;
                string filterExpression = string.Empty;

                // Check if the DataTable contains a column named "ID"
                bool hasIDColumn = dataTable.Columns.Contains("ID");
                if (hasIDColumn) { if (dataTable.Columns["ID"].DataType != typeof(string)) { hasIDColumn = false; } }

                foreach (KeyValuePair<string, string> filter in ((DoubleBufferedDGV)this.DataGridView).filters)
                {
                    if (filterExpression == string.Empty) { filterExpression = filter.Value; } 
                    else { filterExpression += string.Format(" AND {0}", filter.Value); }
                }
                // Add the ID condition only if the DataTable has a column named "ID"
                if (hasIDColumn)
                {
                    string filterByID = "ID is null OR SUBSTRING(ID, 1, 6) = 'NewID_'";
                    if (filterExpression != string.Empty) { filterExpression += " OR " + filterByID; }
                }
                ((DoubleBufferedDGV)this.DataGridView).pauseEvents = true;
                view.RowFilter = filterExpression;
                ((DoubleBufferedDGV)this.DataGridView).pauseEvents = false;
            }
        }


        public void RemoveAllFilters()
        {
            // Clear the filter dictionary
            ((DoubleBufferedDGV)this.DataGridView).filters.Clear();

            // Set IsFiltered to false for all CustomColumnHeaderCell instances in the parent DataGridView.
            foreach (DataGridViewColumn column in this.DataGridView.Columns)
            {
                if (column.HeaderCell is CustomColumnHeaderCell customHeaderCell)
                {
                    customHeaderCell.IsFiltered = false;
                }
            }
        }

        public void RemoveThisFilter()
        {
            //Remove the columnname index from the filter dictionary
            string columnName = this.OwningColumn.DataPropertyName;
            ((DoubleBufferedDGV)this.DataGridView).filters.Remove(columnName);
            this.IsFiltered = false;
        }

        public void AddFilter(List<string> selectedItems)
        {
            DoubleBufferedDGV DGV = (DoubleBufferedDGV)this.DataGridView;
            if (DGV.DataSource is DataTable dataTable)
            {
                string columnName = this.OwningColumn.DataPropertyName;
                //TODO : améliorer ceci pour tous types de colonnes et autres SQL injections
                // Créez une expression de filtre en fonction des éléments sélectionnés.
                

                string filterExpression = string.Format("[{0}] IN ('{1}')", columnName, string.Join("','", selectedItems));
                if (filterExpression.Contains("---blank---")) { 
                    filterExpression += string.Format(" OR [{0}] is null", columnName);
                    filterExpression = filterExpression.Replace("---blank---", "False");
                }

                //Check if already in filter list, if not create line
                if (DGV.filters.ContainsKey(columnName))
                {
                    DGV.filters[columnName] = filterExpression;
                }
                else
                {
                    DGV.filters.Add(columnName, filterExpression);
                }
                this.IsFiltered = true;
            }
        }

    }

}

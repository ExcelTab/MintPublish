//using iText.Layout.Element;
using Mint.Code;
using Mint.Controls;
using Org.BouncyCastle.Bcpg.Sig;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Transactions;


namespace Mint.Forms
{
    public partial class App_Products : Form
    {
        #region Global Events
        public delegate void StatusUpdateDelegate(string message);
        public static event StatusUpdateDelegate StatusUpdate;
        #endregion

        #region Local Variables
        DataTable categoryDataTable;
        DataTable dtViewDefinition = new DataTable();
        DoubleBufferedDGV dataGridView_Selected;
        ImageFromWebBox image_Selected;
        ComboBox imageCombobox_Selected;
        private ToolTip recipeTooltip = new ToolTip();
        string listType;
        string viewQuery;
        string viewQuerySQL;
        private int hoverRow = -1;
        #endregion

        public App_Products()
        {
            InitializeComponent();
            LoadCategoryData();
            PopulateTreeView();
            LoadFeatureList();
            LoadKeywordAndURL();
            RefreshProductListUpdateLabel();

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
        private void LoadCategoryData()
        {
            // Create a command to retrieve the category data
            string query = "SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position";
            categoryDataTable = Database.LoadData(query);
        }

        #endregion

        #region Overall procedures used accross the App
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Change the selected dgv and query
            int selectedIndex = tabControl1.SelectedIndex;
            switch (selectedIndex)
            {
                case 1: // Liste des produits simples
                    dataGridView_Selected = dataGridView_ProductList;
                    image_Selected = imageFromWebBox_ProductList;
                    imageCombobox_Selected = comboBox_Image_ProductList;
                    viewQuery = "ViewProducts";
                    break;
                case 2: // Liste des déclinaisons
                    dataGridView_Selected = dataGridView_Declinaisons;
                    viewQuery = "ViewDeclinaisons";
                    break;
                case 3: // URLs
                    dataGridView_Selected = dataGridView_URL;
                    viewQuery = "ViewURL";
                    break;
                case 4: // Raw MAts
                    dataGridView_Selected = dataGridView_RawMat;
                    viewQuery = "ViewRawMat";
                    break;
                case 5: // Finished Goods
                    dataGridView_Selected = dataGridView_Negoce;
                    viewQuery = "ViewNegoce";
                    break;

                default:
                    // No change
                    return;
            }
        }
        private void RefreshProductListUpdateLabel()
        {
            //Load the date on the label_SynchroPrestashop
            string lastSynchro = Database.GetFieldFromField("CATALOG_TABLES", "Table_Name", "TBL_PRODUCT", "Last_Transfer_Date");
            //the lastYnchro string contains a date in server time. Convert to local time
            DateTime lastSynchroDate = DateTime.Parse(lastSynchro);
            lastSynchro = lastSynchroDate.ToLocalTime().ToString();
            label_SynchroPrestashop.Text = $"Dernière synchro : {lastSynchro}";
        }
        #endregion

        #region Procedures to manage TreeViews
        private void PopulateTreeView()
        {
            // Create the root node of the tree
            TreeNode root = new TreeNode("Accueil");

            // Get the root rows from the datatable
            DataRow[] rootRows = categoryDataTable.Select("id_Parent = 2");

            // Populate the tree view recursively
            foreach (DataRow row in rootRows)
            {
                PopulateTreeView(root, row);
            }

            // Créez des copies distinctes du nœud racine pour chaque TreeView
            TreeNode root1 = (TreeNode)root.Clone();
            TreeNode root2 = (TreeNode)root.Clone();
            TreeNode root3 = (TreeNode)root.Clone();
            TreeNode root4 = (TreeNode)root.Clone();
            TreeNode root5 = (TreeNode)root.Clone();

            // Ajoutez les nœuds racines aux TreeView controls respectifs
            Trv_Category.Nodes.Add(root);
            treeView_SC_Theme.Nodes.Add(root1);
            treeView_SC_CatAssoc.Nodes.Add(root2);
            treeView_SC_CatRule.Nodes.Add(root3);
            treeView_SC_CatKeyword.Nodes.Add(root4);
            treeView_SC_CatURL.Nodes.Add(root5);

            // Expand the root nodes
            Trv_Category.Nodes[0].Expand();
            treeView_SC_Theme.Nodes[0].Expand();
            treeView_SC_CatAssoc.Nodes[0].Expand();
            treeView_SC_CatRule.Nodes[0].Expand();
            treeView_SC_CatKeyword.Nodes[0].Expand();
            treeView_SC_CatURL.Nodes[0].Expand();
        }
        private void PopulateTreeView(TreeNode parentNode, DataRow parentRow)
        {
            int categoryId = Convert.ToInt32(parentRow["id_Category"]);

            // Create a new TreeNode for the current category
            TreeNode node = new TreeNode(parentRow["Name"].ToString());
            node.Tag = parentRow["id_Category"];

            // Add the node to the parent node
            parentNode.Nodes.Add(node);

            // Get the child rows for the current category
            DataRow[] childRows = categoryDataTable.Select($"id_Parent = {categoryId}");

            // Recursively populate the child nodes
            foreach (DataRow childRow in childRows)
            {
                PopulateTreeView(node, childRow);
            }
        }
        private void CheckCategories(TreeNodeCollection nodes, DataTable assignedCategories)
        {

            foreach (TreeNode node in nodes)
            {
                // Check if the category ID is in the assigned categories list
                int categoryId = Convert.ToInt32(node.Tag);
                if (assignedCategories.AsEnumerable().Any(row => categoryId == row.Field<int>(0)))
                {
                    node.Checked = true;
                }

                // Recursively check child nodes
                CheckCategories(node.Nodes, assignedCategories);
            }
        }
        private void ClearCategories(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                // Recursively check child nodes
                ClearCategories(node.Nodes);
            };
        }
        private void ReadCategories(TreeNode parentNode, List<string> categoryList)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Checked)
                {
                    categoryList.Add(node.Tag.ToString());
                }
                // Recursively process child nodes
                ReadCategories(node, categoryList);
            }
        }
        private void LoadCatRuleTreeView()
        {
            ComboBoxItemWithTag selectedType = (ComboBoxItemWithTag)comboBox_RuleType.SelectedItem;
            string tagType = selectedType.Tag;
            ComboBoxItemWithTag selectedValue = (ComboBoxItemWithTag)comboBox_RuleValue.SelectedItem;
            string tagValue = selectedValue.Tag;

            if (tagType != "0" && tagType != "" && tagValue != "0" && tagValue != "")
            {
                //Change categories
                ClearCategories(treeView_SC_CatRule.Nodes);

                string query = $@"SELECT id_category From TBL_CATEGORY_RULE_VALUES
                    Where id_category_rule = 2 and id_rule_value = {tagValue} and id_type_product = {tagType}"; //1 = cat associée
                DataTable dt = Database.LoadData(query);
                if (dt.Rows.Count > 0)
                {
                    CheckCategories(treeView_SC_CatRule.Nodes, dt);
                    label_CountCatRule.Text = $"{dt.Rows.Count} catégories associées";
                }
                else { label_CountCatRule.Text = $"Aucune catégorie associée"; }
            }
        }
        #endregion

        #region Procedures to manage the first tab (add/modify one product)
        private void ClearForm()
        {
            Grp_Product.Text = "NOUVEAU PRODUIT";

            //Clear all textboxes
            foreach (Control control in Grp_Product.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                    textBox.Tag = string.Empty;
                }
            }

            //Remove the image
            Img_Product.Image = null;

            //Uncheck all categories in the TreeView
            ClearCategories(Trv_Category.Nodes);

            //Uncheck all features in the TabControl
            foreach (TabPage tabPage in Tab_Features.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is ListView listView)
                    {
                        // clear all listview checks
                        foreach (ListViewItem item in listView.Items)
                        {
                            item.Checked = false;
                        }
                    }
                }
            }
        }
        private void Btn_Add_Click(object sender, EventArgs e)
        {
            ClearForm();

        }
        private void Btn_Modify_Click(object sender, EventArgs e)
        {

            BlackOverlay blackOverlay = new BlackOverlay();
            //blackOverlay.Size = mainForm.Size;
            //blackOverlay.Location = mainForm.Location;
            blackOverlay.Show();

            ListTable list_product = new ListTable();
            list_product.Load_List("SELECT [calcul_ID] as [ID produit],Référence,[Titre spécifique], [mono_Type de produit] as Type, [mono_Thème] as Thème From ViewProducts order by Référence");
            //list_product.Load_List("SELECT * FROM ViewProducts");
            list_product.Set_Title("Choisissez un produit à modifier");
            list_product.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            //this.Show();
            if (list_product.ID_Selected == null) { return; }
            int ProductID = int.Parse(list_product.ID_Selected);

            ClearForm();

            //Load product info from the database
            string query = $@"SELECT * FROM TBL_NEWPRODUCT WHERE TBL_NEWPRODUCT.id_product = {ProductID.ToString()}";
            DataTable dt = Database.LoadData(query);

            //Set the textbox value from the datatable
            foreach (DataRow reader in dt.Rows)
            {
                // set the  textbox values
                Grp_Product.Text = ProductID.ToString();

                // Fill in the textbox values
                foreach (Control control in Grp_Product.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        if (control.Name.StartsWith("Txt_"))
                        {
                            if (control.Name.StartsWith("Txt_id_"))
                            {
                                string fieldInfo = control.Name.Replace("Txt_id_", "");
                                switch (fieldInfo)
                                {
                                    case "manufacturer":
                                        control.Tag = reader["id_manufacturer"].ToString();
                                        if (control.Tag != "") { control.Text = Database.GetFieldFromID("TBL_MANUFACTURER", "id_manufacturer", control.Tag.ToString(), "name"); }
                                        break;
                                    case "category_default":
                                        control.Tag = reader["id_category_default"].ToString();
                                        if (control.Tag != "") { control.Text = Database.GetFieldFromID("TBL_CATEGORY", "id_category", control.Tag.ToString(), "name"); }
                                        break;
                                    case "keyword":
                                        control.Tag = reader["id_keyword"].ToString();
                                        if (control.Tag != "") { control.Text = Database.GetFieldFromID("TBL_KEYWORD", "id_keyword", control.Tag.ToString(), "name"); }
                                        break;
                                    case "color_default":
                                        control.Tag = reader["id_color_default"].ToString();
                                        if (control.Tag != "") { control.Text = Database.GetFieldFromID("TBL_FEATURES_VALUES", "id_feature_value", control.Tag.ToString(), "name"); }
                                        break;
                                    case "website_url":
                                        control.Tag = reader["id_website_url"].ToString();
                                        if (control.Tag != "") { control.Text = Database.GetFieldFromID("TBL_WEBSITE_URL", "id_website_url", control.Tag.ToString(), "name"); }
                                        break;
                                }
                            }
                            else
                            {
                                control.Text = reader[control.Name.Replace("Txt_", "")].ToString();
                            }

                        }
                    }
                }

                //get the image product from the URL
                string imageUrl = reader["url_image"].ToString();
                if (imageUrl != null)
                {
                    Img_Product.ImageLocation = imageUrl;
                }
                else
                {
                    Img_Product.ImageLocation = null;
                }



            }
            string queryCategory = "SELECT id_Category FROM TBL_CATEGORY_PRODUCT WHERE id_product = " + ProductID.ToString();
            DataTable dtCategory = Database.LoadData(queryCategory);
            CheckCategories(Trv_Category.Nodes, dtCategory);

            //Load the feature list
            string queryFeature = "SELECT id_feature_value FROM TBL_FEATURE_PRODUCT WHERE id_product = " + ProductID.ToString();
            DataTable dtFeature = Database.LoadData(queryFeature);
            foreach (TabPage tabPage in Tab_Features.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is ListView listView)
                    {
                        // clear all listview checks
                        foreach (ListViewItem item in listView.Items)
                        {
                            int featureId = Convert.ToInt32(item.Tag);
                            if (dtFeature.AsEnumerable().Any(row => featureId == row.Field<int>(0)))
                                item.Checked = true;
                        }
                    }
                }
            }
        }
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            //TODO : prévoir le coup des artistes, catégories, etc ajoutées alors qu'elles n'existent pas encore dans la base de données

            //VALUES
            Dictionary<string, string> productValues = new Dictionary<string, string>();
            Dictionary<string, string> productExtendedValues = new Dictionary<string, string>();
            foreach (Control control in Grp_Product.Controls)
            {
                if (control is TextBox textBox)
                {
                    if (control.Name.StartsWith("Txt_"))
                    {
                        string fieldname = control.Name.Substring(4);
                        string fieldValue = "";
                        if (control.Text != "")
                        {
                            if (control.Name == ("Txt_id_keyword") ||
                                control.Name == ("Txt_id_website_url"))
                            {
                                fieldValue = control.Tag.ToString();
                                productExtendedValues.Add(fieldname, fieldValue);
                            }
                            else if (control.Name.StartsWith("Txt_id_"))
                            {
                                fieldValue = control.Tag.ToString();
                                productValues.Add(fieldname, fieldValue);
                            }
                            else
                            {
                                fieldValue = control.Text;
                                productValues.Add(fieldname, fieldValue.Replace(";", ","));
                            }

                        }
                        else { productValues.Add(fieldname, ""); } //vider le champ
                    }
                }
            }

            //CATEGORIES
            List<string> categories = new List<string>();
            foreach (TreeNode node in Trv_Category.Nodes)
            {
                if (node.Checked) { categories.Add(node.Tag.ToString()); }
                ReadCategories(node, categories);
            }

            //FEATURES
            Dictionary<string, string> features = new Dictionary<string, string>();
            foreach (TabPage tabPage in Tab_Features.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is ListView listView)
                    {
                        foreach (ListViewItem item in listView.Items)
                        {
                            if (item.Checked) { features.Add(tabPage.Tag.ToString(), item.Tag.ToString()); }
                        }
                    }
                }
            }

            //IS DECLINABLE
            string isDeclinable = "0";
            if (checkBox_is_declinable.Checked) { isDeclinable = "1"; }

            //Check to see if its a NOUVEAU PRODUIT (so Insert) or an existing product (so Update)
            string query = "";
            string queryExtended = "";
            string PrestashopID = Grp_Product.Text;
            if (PrestashopID == "NOUVEAU PRODUIT") //INSERT
            {

                //Get the temporary produt id. Formula is the +1 of the false Prestashop, or the highest prestashop + 1000
                query = "SELECT MAX(id_product) FROM TBL_NEWPRODUCT WHERE is_Prestashop = 0";
                DataTable dt = Database.LoadData(query);
                int newPrestashopId = 0;
                if (dt.Rows[0][0] != DBNull.Value)
                {
                    newPrestashopId = Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
                else
                {
                    query = "SELECT MAX(id_product) FROM TBL_NEWPRODUCT";
                    dt = Database.LoadData(query);
                    if (dt.Rows[0][0] != DBNull.Value)
                    {
                        newPrestashopId = Convert.ToInt32(dt.Rows[0][0]) + 1000;
                    }
                }
                PrestashopID = newPrestashopId.ToString();
                //Create the add query
                query = $@"INSERT INTO TBL_NEWPRODUCT (id_product,is_Prestashop,{string.Join(",", productValues.Keys)},is_declinable,date_add)
                        VALUES ({PrestashopID}, 0,@{string.Join(",@", productValues.Keys)},{isDeclinable},GETDATE())";
                queryExtended = $@"INSERT INTO TBL_NEWPRODUCT (id_product,{string.Join(",", productExtendedValues.Keys)},date_add)
                        VALUES ({PrestashopID}, @{string.Join(",@", productValues.Keys)},GETDATE())";
            }
            else //Update
            {
                //Delete the existing links with categories and features
                query = $"DELETE FROM TBL_CATEGORY_PRODUCT WHERE id_product = {PrestashopID}";
                Database.ExecuteQuery(query);
                query = $"DELETE FROM TBL_FEATURE_PRODUCT WHERE id_product = {PrestashopID}";
                Database.ExecuteQuery(query);

                query = "UPDATE TBL_NEWPRODUCT SET ";
                foreach (KeyValuePair<string, string> productValue in productValues)
                {
                    query += $"{productValue.Key} = @{productValue.Key},";
                }
                query += $"date_update = GETDATE() WHERE id_product = {PrestashopID}";
            }

            //Execute the update/insert query
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, string> productValue in productValues)
            {
                parameters.Add("@" + productValue.Key, productValue.Value);
            }
            Database.ExecuteQuery(query, parameters);
            parameters.Clear();
            foreach (KeyValuePair<string, string> productValue in productExtendedValues)
            {
                parameters.Add("@" + productValue.Key, productValue.Value);
            }
            Database.ExecuteQuery(queryExtended, parameters);

            //Add the links with categories and features
            foreach (string category in categories)
            {
                query = $"INSERT INTO TBL_CATEGORY_PRODUCT (id_category,id_product,date_add,date_update) VALUES ({category},{PrestashopID}, GETDATE(), GETDATE())";
                Database.ExecuteQuery(query);
            }
            foreach (KeyValuePair<string, string> feature in features)
            {
                query = $"INSERT INTO TBL_FEATURE_PRODUCT (id_feature,id_feature_value,id_product,date_add,date_update) VALUES ({feature.Key},{feature.Value},{PrestashopID},GETDATE(), GETDATE())";
                Database.ExecuteQuery(query);
            }
            //vider les champs
            ClearForm();

        }
        private void Load_Artist_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_choice = new ListTable();
            list_choice.Load_List("SELECT id_manufacturer as ID,name as Artist FROM TBL_MANUFACTURER order by name Asc");
            list_choice.Set_Title("Artiste");
            list_choice.ShowDialog();

            blackOverlay.Close();
            Txt_id_manufacturer.Text = list_choice.Text_Selected;
            Txt_id_manufacturer.Tag = list_choice.ID_Selected;
        }
        private void Load_Main_Category_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_choice = new ListTable();
            list_choice.ViewMode = "TreeView";
            list_choice.Load_List("SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position");
            list_choice.Set_Title("Catégorie de base");
            list_choice.ShowDialog();

            blackOverlay.Close();
            Txt_id_category_default.Text = list_choice.Text_Selected;
            Txt_id_category_default.Tag = list_choice.ID_Selected;
        }
        private void Load_Keyword_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_choice = new ListTable();
            list_choice.ViewMode = "ListView";
            list_choice.Load_List("SELECT id_keyword as ID, name as [Mot clé] FROM TBL_KEYWORD order by name Asc");
            list_choice.Set_Title("Mot clé");
            list_choice.ShowDialog();

            blackOverlay.Close();
            Txt_id_keyword.Text = list_choice.Text_Selected;
            Txt_id_keyword.Tag = list_choice.ID_Selected;
        }
        private void Load_Color_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_choice = new ListTable();
            list_choice.ViewMode = "ListView";
            list_choice.Load_List("SELECT id_feature_value as ID, name as [Couleur] FROM TBL_FEATURES_VALUES where id_feature = 18 order by name Asc");
            list_choice.Set_Title("Couleur dominante");
            list_choice.ShowDialog();

            blackOverlay.Close();
            Txt_id_color_default.Text = list_choice.Text_Selected;
            Txt_id_color_default.Tag = list_choice.ID_Selected;
        }
        private void Load_Website_url_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_choice = new ListTable();
            list_choice.ViewMode = "ListView";
            list_choice.Load_List("SELECT id_website_url as ID, name as [Lien maillage] FROM TBL_WEBSITE_URL order by name Asc");
            list_choice.Set_Title("Lien maillage");
            list_choice.ShowDialog();

            blackOverlay.Close();
            Txt_id_website_url.Text = list_choice.Text_Selected;
            Txt_id_website_url.Tag = list_choice.ID_Selected;
        }
        private void LoadFeatureList()
        {
            //Load the product types in the SC combobox
            string query = $@"SELECT id_feature_value, name FROM TBL_FEATURES_VALUES
                            WHERE id_feature = 19"; //type de produit
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string text = row["name"].ToString();
                    string tag = row["id_feature_value"].ToString();
                    comboBox_RuleType.Items.Add(new ComboBoxItemWithTag(text, tag));
                }
                comboBox_RuleType.SelectedItem = comboBox_RuleType.Items[0];
            }

            //Load the list of all features, and create a new tab page for each feature
            query = "SELECT id_feature, name FROM TBL_FEATURES";
            DataTable dtFeatures = Database.LoadData(query);

            //For each feature, create a new tab page with a fully docked listview
            foreach (DataRow row in dtFeatures.Rows)
            {
                string text = row["name"].ToString();
                string tag = row["id_feature"].ToString();
                comboBox_RuleFeature.Items.Add(new ComboBoxItemWithTag(text, tag));

                TabPage tabPage = new TabPage(text);
                tabPage.Tag = tag;
                tabPage.AutoScroll = true;
                ListView listView = new ListView();
                listView.Dock = DockStyle.Fill;
                listView.CheckBoxes = true;
                listView.View = View.Details;
                listView.Columns.Add("a virer");

                query = $"SELECT id_feature_value, name FROM TBL_FEATURES_VALUES WHERE id_feature = {tag}";
                DataTable dtValues = Database.LoadData(query);
                //fill in the listview with the feature values
                foreach (DataRow valueRow in dtValues.Rows)
                {
                    ListViewItem item = new ListViewItem(valueRow["name"].ToString());
                    item.Tag = valueRow["id_feature_value"].ToString();
                    listView.Items.Add(item);
                }
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //remove the header row from the listview
                listView.HeaderStyle = ColumnHeaderStyle.None;
                tabPage.Controls.Add(listView);
                Tab_Features.TabPages.Add(tabPage);
            }
            comboBox_RuleFeature.SelectedItem = comboBox_RuleFeature.Items[0];
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            if (pictureBox.Image != null)
            {
                // Create a new form
                Form imageForm = new Form();
                imageForm.FormBorderStyle = FormBorderStyle.FixedSingle;


                // Create a PictureBox control on the new form
                PictureBox enlargedPictureBox = new PictureBox();
                enlargedPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                enlargedPictureBox.Image = pictureBox.Image;

                // Set the size of the new form based on the image size

                int ratio = enlargedPictureBox.Image.Height / enlargedPictureBox.Image.Width;
                //if the height of the image is bigger than the screen height, we resize the image to fit the screen
                if (enlargedPictureBox.Image.Height > Screen.PrimaryScreen.Bounds.Height)
                {
                    imageForm.Size = new Size((Screen.PrimaryScreen.Bounds.Height - 100) / (ratio), Screen.PrimaryScreen.Bounds.Height - 100);
                    enlargedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    imageForm.Size = new Size(enlargedPictureBox.Image.Width + 100, enlargedPictureBox.Image.Height + 100);
                    enlargedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }

                imageForm.StartPosition = FormStartPosition.CenterParent;
                enlargedPictureBox.Dock = DockStyle.Fill;



                // Add the PictureBox to the new form
                imageForm.Controls.Add(enlargedPictureBox);

                // Show the new form
                BlackOverlay blackOverlay = new BlackOverlay();
                blackOverlay.Show();
                imageForm.ShowDialog();
                blackOverlay.Close();
            }
        }
        private void Txt_Artist_TextChanged(object sender, EventArgs e)
        {
            //Txt_id_manufacturer.Tag = null;

        }
        private void Txt_MainCategory_TextChanged(object sender, EventArgs e)
        {
            //Txt_id_category_default.Tag = null;
        }
        #endregion

        #region Procedures to manage product Extracts to StoreCommander or other lists
        private async void Btn_Extract_Click(object sender, EventArgs e)
        {
            //sortir tout le catalogue produit pour StoreCommander
            string filter = "";
            if (radioButton_ListingNew.Checked) { filter = "and is_Prestashop = 0"; }
            if (radioButton_ListingOld.Checked) { filter = "and is_Prestashop = 1"; }
            await Task.Run(async () => { await ExtractProductToCSV(filter); });

        }
        private async Task ExtractProductToCSV(string filter)
        {
            //Get the Template data table
            StatusUpdate?.Invoke($"Sortie du catalogue des produits au format StoreCommander...");
            DataTable dtStoreCommander = Database.LoadData("SELECT * FROM TEMPLATES where output_name = 'StoreCommander' order by sequence_num");

            //Load all data tables that we will need
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_PRODUCT...");
            DataTable dtProducts = Database.LoadData($@"SELECT * FROM TBL_NEWPRODUCT where name is not null 
            and name <> '' and id_type is not null and id_type <> 0 {filter} ORDER BY id_product");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_FEATURES_PRODUCT...");
            DataTable dtFeatures = Database.LoadData("SELECT * FROM TBL_FEATURE_PRODUCT");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_CATEGORY_PRODUCT...");
            DataTable dtCategories = Database.LoadData("SELECT * FROM TBL_CATEGORY_PRODUCT");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_ARTICLE...");
            //Modification 07 APR 2025 : ne sortir que les déclinaisons encore actives
            DataTable dtArticles = Database.LoadData("SELECT * FROM TBL_ARTICLE where is_activesc = 1");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_CATEGORY_RULE_VALUES...");
            DataTable dtRules = Database.LoadData("SELECT * FROM TBL_CATEGORY_RULE_VALUES");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_KEYWORD...");
            DataTable dtKeywords = Database.LoadData("SELECT * FROM TBL_KEYWORD");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_WEBSITE_URL...");
            DataTable dtWebsiteURLs = Database.LoadData("SELECT * FROM TBL_WEBSITE_URL");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_URL...");
            DataTable dtURLs = Database.LoadData("SELECT * FROM TBL_URL");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_ATTRIBUTE_VALUES...");
            DataTable dtAttributes = Database.LoadData("SELECT * FROM TBL_ATTRIBUTE_VALUES");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_MANUFACTURER...");
            DataTable dtManufacturer = Database.LoadData("SELECT * FROM TBL_MANUFACTURER");
            StatusUpdate?.Invoke($"Chargement des tables nécessaires pour Store Commander : TBL_NEGOCE...");
            DataTable dtNegoce = Database.LoadData("SELECT * FROM TBL_NEGOCE");

            //Create a recieving datatable that has the columns defined by the rows of dtStoreCommander
            StatusUpdate?.Invoke($"Création de la table de sortie...");
            DataTable dtOutput = new DataTable();
            foreach (DataRow row in dtStoreCommander.Rows)
            {
                dtOutput.Columns.Add(row["header_name"].ToString());
            }
            int counter = 0;
            //Loop through each product in dtProducts
            foreach (DataRow product in dtProducts.Rows)
            {
                counter++;
                StatusUpdate?.Invoke($"Itération sur les produits : {counter}/{dtProducts.Rows.Count}...");

                //Get mapping values
                int id_product = Convert.ToInt32(product["id_product"]);
                int id_category_default = (product["id_category_default"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_category_default"]);
                int id_type = (product["id_type"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_type"]);
                int id_format = (product["id_format"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_format"]);
                int id_support = (product["id_support"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_support"]);
                int id_dimension = (product["id_dimension"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_dimension"]);
                int id_article = (product["id_article"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_article"]);
                int id_artiste = (product["id_manufacturer"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_manufacturer"]);
                int id_keyword = (product["id_keyword"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_keyword"]);
                int id_website_url = (product["id_website_url"] == DBNull.Value) ? 0 : Convert.ToInt32(product["id_website_url"]);
                string reference = product["reference"].ToString();
                string title = product["name"].ToString().Trim().Replace("\n", " ");
                // Correction 07 APR 2025 : remplacement de tous les ; par des : dans la description
                string websiteText = product["website_description_long"].ToString().Replace("\n", " ").Replace(";",":");
                string motCliquable = product["website_keyword"].ToString().Replace("\n", " ");

                //Get related features
                DataRow[] features = dtFeatures.Select("id_product = " + id_product);
                List<int> featureList = features.Select(row => Convert.ToInt32(row["id_feature_value"])).ToList();
                //Dictionary<int, string> featureDictionary = features
                //    .ToDictionary(row => Convert.ToInt32(row["id_feature"]), row => string.Join(", ", features.Where(valueRow => Convert.ToInt32(valueRow["id_feature"]) == Convert.ToInt32(row["id_feature"])).Select(valueRow => valueRow["id_feature_value"].ToString())));

                Dictionary<int, string> featureDictionary = features
                    .GroupBy(row => Convert.ToInt32(row["id_feature"]))
                    .ToDictionary(
                        group => group.Key,
                        group => string.Join(", ", group.Select(row => row["id_feature_value"].ToString()))
                    );
                //Get related categories
                DataRow[] categories = dtCategories.Select("id_product = " + id_product);
                List<int> categoryList = categories.Select(row => Convert.ToInt32(row["id_category"])).ToList();
                categoryList.Add(id_category_default);

                //Load associated categories from ruleset 1
                List<int> categoryListFromRule1 = dtRules.Select("id_category_rule = 1 AND id_rule_value IN (" + string.Join(",", categoryList) + ")").Select(row => Convert.ToInt32(row["id_category"])).ToList();
                List<int> categoryListFromRule2;
                if (id_type != 0)
                {
                    string selectFeatures = "id_category_rule = 2 AND id_rule_value IN(";
                    if (featureList.Count > 0) { selectFeatures += string.Join(",", featureList) + ","; }
                    selectFeatures += id_type + "," + id_format + ") AND id_type_product = " + id_type;

                    categoryListFromRule2 = dtRules.Select(selectFeatures).Select(row => Convert.ToInt32(row["id_category"])).ToList();
                }
                else
                {
                    categoryListFromRule2 = new List<int>();
                }
                List<int> categoryListFinal = categoryList.Union(categoryListFromRule1.Union(categoryListFromRule2)).ToList();
                // Remove items that are exactly zero
                categoryListFinal.RemoveAll(item => item == 0);
                categoryListFinal = categoryListFinal.Distinct().ToList();

                //Get additionnal mapping values
                string keyword = "";
                if (id_keyword != 0)
                {
                    DataRow[] rows = dtKeywords.Select("id_keyword = " + id_keyword.ToString());

                    if (rows.Length > 0)
                    {
                        keyword = rows[0]["name"].ToString().Trim();
                    }
                }
                string lienMaillage = "";
                if (id_website_url != 0)
                {
                    DataRow[] rows = dtWebsiteURLs.Select("id_website_url = " + id_website_url.ToString());

                    if (rows.Length > 0)
                    {
                        lienMaillage = rows[0]["name"].ToString().Trim();
                    }
                }

                // Define all applicable declinaisons
                var articleFilters = new List<string>();
                if (id_type != 0) { articleFilters.Add("id_type = " + id_type); }
                if (id_format != 0) { articleFilters.Add("(id_format = " + id_format + " or id_format is null or id_format = 0)"); }
                if (id_support != 0) { articleFilters.Add("(id_support = " + id_support + " or id_support is null or id_support = 0)"); }
                if (id_dimension != 0) { articleFilters.Add("(id_dimension = " + id_dimension + " or id_dimension is null or id_dimension = 0)"); }
                if (id_article != 0) { articleFilters.Add("id_article = " + id_article); }

                // Construct the filter string by joining individual filters with 'AND'
                string articleFilter = string.Join(" AND ", articleFilters);

                // Get applicable declinaisons using the constructed filter
                DataRow[] declinaisons = dtArticles.Select(articleFilter);

                //Start adding the header row in dtOutput
                DataRow outputRow = dtOutput.NewRow();
                foreach (DataRow col in dtStoreCommander.Rows)
                {
                    string colName = col["header_name"].ToString();
                    if (col["formula"] is null && col["table_name"] is null)
                    {
                        //Do nothing, leave the cell empty
                    }
                    else if (Convert.ToBoolean(col["is_formula"]) == true)
                    {
                        //Apply the right formula
                        switch (col["formula"].ToString())
                        {
                            case "CollateTitleAndKeyword":
                                outputRow[colName] = title;
                                if (keyword != "")
                                {
                                    outputRow[colName] += " - " + keyword;
                                }
                                //Put the first character of the string in uppercase
                                if (outputRow[colName] is string && !string.IsNullOrEmpty(outputRow[colName].ToString()))
                                {
                                    outputRow[colName] = char.ToUpper(outputRow[colName].ToString().Trim()[0]) + outputRow[colName].ToString().Trim().Substring(1);
                                }

                                break;
                            case "URLFromTitle":
                                outputRow[colName] = Code.BasicFunctions.ReplaceSpecialChars((keyword + "-" + title)
                                    .Replace(" ", "-")
                                    .Replace("'", "-"));
                                //If it starts or ends by -, remove it
                                if (outputRow[colName].ToString().StartsWith("-")) { outputRow[colName] = outputRow[colName].ToString().Substring(1); }
                                if (outputRow[colName].ToString().EndsWith("-")) { outputRow[colName] = outputRow[colName].ToString().Substring(0, outputRow[colName].ToString().Length - 1); }
                                break;
                            case "GetAllCategories":
                                outputRow[colName] = string.Join(", ", categoryListFinal);
                                break;
                            case "HTMLDescription":
                                if (websiteText != "") // && motCliquable != "")
                                {
                                    if (motCliquable != "" && lienMaillage != "")
                                    {
                                        outputRow[colName] = "<p><br />" + websiteText.Replace("\n", "<br />").Replace(motCliquable, $"<a href=\"{lienMaillage}\">{motCliquable}</a>") + "</p>";
                                    }
                                    else
                                    {
                                        outputRow[colName] = "<p><br />" + websiteText.Replace("\n", "<br />") + "</p>";
                                    }

                                }
                                break;
                            case "Equal20IfNoChild":
                                if (declinaisons.Length < 2) { outputRow[colName] = 20; }
                                break;
                            case "Equal1":
                                outputRow[colName] = 1;
                                break;
                            case "GetURL1" or "GetURL2" or "GetURL3" or "GetURL4" or "GetURL5" or "GetURL6" or "GetURL7" or "GetURL8" or "GetURL9" or "GetURL10":
                                int urlNumber = Convert.ToInt32(col["formula"].ToString().Replace("GetURL", ""));
                                DataRow[] url = dtURLs.Select($"id_type={id_type} and (id_support is null or id_support = 0) and (id_dimension is null or id_dimension = 0) and id_urlnum={urlNumber}");
                                if (url.Length > 0)
                                {
                                    string urlToPaste = url[0]["url_prefix"].ToString();
                                    if (url[0]["url_suffix"].ToString() != "") { urlToPaste += reference + url[0]["url_suffix"].ToString(); }
                                    outputRow[colName] = urlToPaste;
                                }
                                break;
                            case "GetAccessoire":
                                string accessoire = "";
                                switch (id_type)
                                {
                                    //modification 07 APR 2025 : ajout du 699 et 700
                                    case 681 or 699 or 700: //Papier peint sur mesure, Tapisserie, Tapisserie Hexoa
                                        accessoire = "5KT";
                                        break;
                                    case 682: //Crédence sur mesure
                                        accessoire = "PSE-CD";
                                        break;
                                }
                                outputRow[colName] = accessoire;
                                break;
                        }
                    }
                    else
                    {
                        //Select the right table with a switch case
                        switch (col["table_name"].ToString())
                        {
                            case "TBL_NEWPRODUCT":
                                outputRow[colName] = product[col["column_name"].ToString()];
                                if (outputRow[colName].ToString() == "0") { outputRow[colName] = ""; }
                                if (outputRow[colName].ToString().Contains("\n")) { outputRow[colName] = outputRow[colName].ToString().Replace("\n", " "); }
                                if (outputRow[colName].ToString().Contains(";")) { outputRow[colName] = outputRow[colName].ToString().Replace(";", ":"); }

                                //Put the first character of the string in uppercase
                                if (outputRow[colName] is string && !string.IsNullOrEmpty(outputRow[colName].ToString()))
                                {
                                    outputRow[colName] = char.ToUpper(outputRow[colName].ToString().Trim()[0]) + outputRow[colName].ToString().Trim().Substring(1);
                                }
                                break;
                            case "TBL_FEATURE_PRODUCT":
                                string columnValue;
                                if (featureDictionary.TryGetValue(Convert.ToInt32(col["column_name"]), out columnValue))
                                {
                                    if (columnValue != "0") { outputRow[colName] = columnValue; }
                                }
                                break;
                            case "TBL_MANUFACTURER":
                                DataRow[] artiste = dtManufacturer.Select("id_manufacturer = " + id_artiste);
                                if (artiste.Length > 0)
                                {
                                    outputRow[colName] = artiste[0][col["column_name"].ToString()];
                                    if (outputRow[colName].ToString() == "0") { outputRow[colName] = ""; }
                                }
                                break;
                        }

                    }

                }
                dtOutput.Rows.Add(outputRow);

                //Add the child items of the product
                if (declinaisons.Length > 0 && id_type != 0)
                {
                    DataTable dtChilds = declinaisons.CopyToDataTable();

                    //Loop on all remaining child items
                    foreach (DataRow childRow in dtChilds.Rows)
                    {
                        //Get mapping values
                        string article = childRow["name"].ToString();
                        bool is_default = (childRow["is_default"] == DBNull.Value) ? false : Convert.ToBoolean(childRow["is_default"]);
                        int id_support_child = (childRow["id_support"] == DBNull.Value) ? 0 : Convert.ToInt32(childRow["id_support"]);
                        int id_dimension_child = (childRow["id_dimension"] == DBNull.Value) ? 0 : Convert.ToInt32(childRow["id_dimension"]);
                        double prix_HT = (childRow["prix_public_HT"] == DBNull.Value) ? 0 : Convert.ToDouble(childRow["prix_public_HT"]);
                        double poids_prestashop = (childRow["poids_prestashop"] == DBNull.Value) ? 0 : Convert.ToDouble(childRow["poids_prestashop"]);

                        //If only 1 child, consider the product as Simple product for PrestaShop
                        if (dtChilds.Rows.Count != 1) { outputRow = dtOutput.NewRow(); }
                        foreach (DataRow col in dtStoreCommander.Rows)
                        {
                            string colName = col["header_name"].ToString();
                            if (col["child_is_formula"] is null || col["child_is_formula"] == DBNull.Value)
                            {
                                // Do nothing, leave the cell empty
                            }
                            else if ((col["child_formula"] is null || (col["child_formula"] == DBNull.Value)) &&
                                        (col["child_table_name"] is null || (col["child_table_name"] == DBNull.Value)))
                            {
                                // Do nothing, leave the cell empty
                            }
                            else if (Convert.ToBoolean(col["child_is_formula"]) == true)
                            {
                                //Apply the right formula
                                switch (col["child_formula"].ToString())
                                {
                                    case "CopyParent":
                                        if (dtChilds.Rows.Count > 1)
                                        {
                                            int rowParent = dtOutput.Rows.Count - 1; // Corrected rowParent calculation
                                            if (rowParent >= 0) // Checking if rowParent is a valid index
                                            {
                                                DataRow previousRow = dtOutput.Rows[rowParent];
                                                outputRow[colName] = previousRow[colName]; // Assigning value to outputRow
                                            }
                                        }
                                        break;
                                    case "AddRefToArticle":
                                        string[] articleParts = article.Split('-');
                                        if (id_type == 690 || id_type == 695) //cas particulier pour les cadres seuls et échantillons : pas d'image. Ne pas utiliser la référence
                                        {
                                            outputRow[colName] = article;
                                        }
                                        else if (reference.StartsWith(articleParts[0]) && reference.EndsWith(articleParts[1]))
                                        {
                                            outputRow[colName] = reference;
                                        }
                                        else if (reference != article && !reference.StartsWith(article.Replace("-", "")))
                                        {
                                            outputRow[colName] = article.Replace("-", $"-{reference}");
                                        }
                                        else
                                        {
                                            outputRow[colName] = reference;
                                        }
                                        break;
                                    case "GetDefaultDeclinaison":
                                        if (is_default) { outputRow[colName] = 1; }
                                        break;
                                    case "GetAttributeSupport":
                                        //Find name corresponding to id_support in dtAttributes
                                        if (id_support_child > 0)
                                        {
                                            DataRow[] support = dtAttributes.Select("id_attribute = " + id_support_child);
                                            //Modification 07 APR 2025 : si type produit = 699 ou 700 (tapisseries), on ne sort pas le support
                                            if (support.Length > 0 && id_type != 699 && id_type != 700)
                                            {
                                                outputRow[colName] = support[0]["name"];
                                            }
                                        }
                                        break;
                                    case "GetAttributeDimension":
                                        //Find name corresponding to id_dimension in dtAttributes
                                        if (id_dimension_child > 0)
                                        {
                                            DataRow[] dimension = dtAttributes.Select("id_attribute = " + id_dimension_child);
                                            //Modification 07 APR 2025 : si type produit = 699 ou 700 (tapisseries), on ne sort pas la dimension
                                            if (dimension.Length > 0 && id_type != 699 && id_type != 700)
                                            {
                                                outputRow[colName] = dimension[0]["name"];
                                            }
                                        }
                                        break;
                                    case "Equal20":
                                        outputRow[colName] = 20;
                                        break;
                                    case "GetURL1" or "GetURL2" or "GetURL3" or "GetURL4" or "GetURL5" or "GetURL6" or "GetURL7" or "GetURL8" or "GetURL9" or "GetURL10":
                                        int urlNumber = Convert.ToInt32(col["formula"].ToString().Replace("GetURL", ""));
                                        string selectString = $"id_type={id_type} and id_urlnum={urlNumber}";
                                        if (id_support_child == 0 && id_dimension_child == 0) { selectString += " and (id_support IS NULL OR id_support = 0) and id_dimension is null"; }
                                        else if (id_support_child != 0 && id_dimension_child == 0) { selectString += $" and id_support={id_support_child}"; }
                                        else if (id_support_child == 0 && id_dimension_child != 0) { selectString += $" and id_dimension={id_dimension_child}"; }
                                        else if (id_support_child != 0 && id_dimension_child != 0)
                                        {
                                            selectString += $@" and ((id_support={id_support_child} and (id_dimension IS NULL OR id_dimension = 0)) or
                                                                    ((id_support IS NULL OR id_support = 0) and id_dimension={id_dimension_child}) or
                                                                    (id_support={id_support_child} and id_dimension={id_dimension_child}))";
                                        }
                                        DataRow[] url = dtURLs.Select(selectString);
                                        if (url.Length > 0)
                                        {
                                            string urlToPaste = url[0]["url_prefix"].ToString();
                                            if (url[0]["url_suffix"].ToString() != "") { urlToPaste += reference + url[0]["url_suffix"].ToString(); }
                                            outputRow[colName] = urlToPaste;
                                        }
                                        break;
                                    case "GetPrixHT":
                                        DataRow[] negoce = dtNegoce.Select("name = '" + outputRow["ref image"].ToString() + "'");
                                        if (negoce.Length > 0)
                                        {
                                            double prix_HT_negoce = (negoce[0]["prix_public_HT"] == DBNull.Value) ? 0 : Convert.ToDouble(negoce[0]["prix_public_HT"]);
                                            if (prix_HT_negoce > 0) { prix_HT = prix_HT_negoce; }
                                        }
                                        outputRow[colName] = Math.Round(prix_HT, 2);
                                        break;
                                    case "PoidsSiNull":
                                        if (poids_prestashop > 0) { outputRow[colName] = poids_prestashop.ToString(); }
                                        else { outputRow[colName] = childRow["poids"].ToString(); }
                                        break;
                                }
                            }
                            else
                            {
                                //Select the right table with a switch case
                                switch (col["child_table_name"].ToString())
                                {
                                    case "TBL_NEWPRODUCT":
                                        outputRow[colName] = product[col["child_column_name"].ToString()];
                                        if (outputRow[colName].ToString().Contains("\n")) { outputRow[colName] = outputRow[colName].ToString().Replace("\n", " "); }
                                        if (outputRow[colName].ToString().Contains(";")) { outputRow[colName] = outputRow[colName].ToString().Replace(";", ":"); }

                                        //Put the first character of the string in uppercase
                                        if (outputRow[colName] is string && !string.IsNullOrEmpty(outputRow[colName].ToString()))
                                        {
                                            outputRow[colName] = char.ToUpper(outputRow[colName].ToString().Trim()[0]) + outputRow[colName].ToString().Trim().Substring(1);
                                        }
                                        break;
                                    case "TBL_ARTICLE":
                                        outputRow[colName] = childRow[col["child_column_name"].ToString()];
                                        break;
                                    case "TBL_FEATURE_PRODUCT":
                                        string columnValue;
                                        if (featureDictionary.TryGetValue(Convert.ToInt32(col["child_column_name"]), out columnValue))
                                        {
                                            outputRow[colName] = columnValue;
                                        }
                                        break;
                                }

                            }
                        }
                        //only add if not a simple product
                        if (dtChilds.Rows.Count != 1) { dtOutput.Rows.Add(outputRow); }
                    }
                }
            }
            StatusUpdate?.Invoke($"Sortie du catalogue des produits au format StoreCommander...");
            CSV.CreateCsv(dtOutput, "output.csv", true);
            StatusUpdate?.Invoke($"");
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
                else if (column.HeaderText.StartsWith("Ratio"))
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
            if (sender.Equals(button_RefreshProductList))
            {
                RefreshProductListUpdateLabel();
            }
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

                //Modification 11/03/2024 ; order the changes by ID
                dataGridView_Selected.recordedChanges.DefaultView.Sort = "ID ASC";
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
                        case "ViewProducts":
                            baseTableName = "TBL_NEWPRODUCT";
                            idField = "id_product";
                            break;
                        case "ViewDeclinaisons":
                            baseTableName = "TBL_ARTICLE";
                            idField = "id_article";
                            break;
                        case "ViewRawMat":
                            baseTableName = "TBL_RAWMAT";
                            idField = "id_rawmat";
                            break;
                        case "ViewURL":
                            baseTableName = "TBL_URL";
                            idField = "id_url";
                            break;
                        case "ViewNegoce":
                            baseTableName = "TBL_NEGOCE";
                            idField = "id_negoce";
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
                                case "ViewProducts":
                                    //Create the product in TBL_PRODUCT
                                    //First get MaxID :
                                    string maxIDquery = "SELECT CASE WHEN MAX(id_product) > 9000000 THEN MAX(id_product) + 1 ELSE 9000001 END AS NewMaxID FROM TBL_NEWPRODUCT";
                                    using (SqlCommand command = new SqlCommand(maxIDquery, connection))
                                    {
                                        try
                                        {
                                            NewID = command.ExecuteScalar().ToString();
                                            newIDs.Add(ID, NewID);
                                            ID = NewID;
                                            insertQuery += $@" (id_product, date_add)
                                                    VALUES ({ID},
                                                    GETDATE())";
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($@"La commande {maxIDquery} est invalide. Erreur : {ex.Message}");
                                            dataGridView_Selected.pauseEvents = false;
                                            continue;
                                        }
                                    }

                                    break;
                                case "ViewDeclinaisons":
                                case "ViewRawMat":
                                case "ViewURL":
                                case "ViewNegoce":
                                    insertQuery += $" (date_add) VALUES (GETDATE())";
                                    break;
                            }
                            insertQuery += "; SELECT SCOPE_IDENTITY()";
                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                try
                                {
                                    if (viewQuery == "ViewProducts")
                                    {
                                        command.ExecuteNonQuery();
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
                                case "ViewProducts":
                                    deleteQueries.Add("DELETE FROM TBL_NEWPRODUCT WHERE id_product = " + IDtoDelete);
                                    deleteQueries.Add("DELETE FROM TBL_FEATURE_PRODUCT WHERE id_product = " + IDtoDelete);
                                    deleteQueries.Add("DELETE FROM TBL_CATEGORY_PRODUCT WHERE id_product = " + IDtoDelete);
                                    break;
                                case "ViewDeclinaisons":
                                    deleteQueries.Add("DELETE FROM TBL_ARTICLE WHERE id_article = " + IDtoDelete);
                                    deleteQueries.Add("DELETE FROM TBL_RECETTES WHERE id_article = " + IDtoDelete);
                                    break;
                                case "ViewURL":
                                    deleteQueries.Add("DELETE FROM TBL_URL WHERE id_url = " + IDtoDelete);
                                    break;
                                case "ViewRawMat":
                                    deleteQueries.Add("DELETE FROM TBL_RAWMAT WHERE id_rawmat = " + IDtoDelete);
                                    break;
                                case "ViewNegoce":
                                    deleteQueries.Add("DELETE FROM TBL_NEGOCE WHERE id_negoce = " + IDtoDelete);
                                    break;
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
                        if (selectType == "") //Simple update to the PRODUCT or ARTICLES or RAWMAT table
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
                                    case "Support Pressing": idColumn = "id_siteflowcode"; break;
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
                            else if (tableName.StartsWith("Link")) //erase all links for that id and add the new ones
                            {
                                //Split all the values of the new value by comma into a list
                                List<string> valueName = newValue.ToString().Split(",").ToList();
                                List<string> valueID = new List<string>();

                                if (columnName == "Catégories associées") //categories
                                {
                                    tableName = "TBL_CATEGORY_PRODUCT";
                                    //get the id of the category for each value in the list
                                    foreach (string valueCat in valueName)
                                    {
                                        string cat_id = Database.GetID("TBL_CATEGORY", "name", valueCat);
                                        if (cat_id != "")
                                        {
                                            //add the id to the valueID list
                                            valueID.Add(cat_id);
                                        }
                                    }

                                    //delete all the links for that product
                                    string deleteQuery = $"DELETE FROM {tableName} WHERE {idField} = {ID}";
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

                                    //Insert all the new links in the table
                                    foreach (string cat_id in valueID)
                                    {
                                        string insertQuery = $"INSERT INTO {tableName} (id_category,id_product,date_add,date_update) VALUES ({cat_id},{ID},GETDATE(),GETDATE())";
                                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                        {
                                            try
                                            {
                                                command.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"La commande {insertQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                                continue;
                                            }
                                        }
                                    }

                                }
                                else //features
                                {
                                    tableName = "TBL_FEATURE_PRODUCT";
                                    string parentName = "";
                                    switch (columnName)
                                    {
                                        //TODO : demander à Mathieu de changer ça sur Prestashop pour que ça match
                                        case "Couleurs": parentName = "Couleur"; break;
                                        case "Pièces": parentName = "Pièce"; break;
                                        case "Styles": parentName = "Styles"; break;
                                    }

                                    //get the id of the parent in TBL_FEATURES
                                    string parentID = Database.GetID("TBL_FEATURES", "name", parentName);

                                    //get the id of the feature for each value in the list
                                    foreach (string valueFeature in valueName)
                                    {
                                        string feat_id = Database.GetID("TBL_FEATURES_VALUES", "name", valueFeature, false, $"id_feature = {parentID}");
                                        if (feat_id != "")
                                        {
                                            //add the id to the valueID list
                                            valueID.Add(feat_id);
                                        }
                                    }

                                    //Remove all Features of that parent for this product
                                    string deleteQuery = $"DELETE FROM {tableName} WHERE id_product = {ID} AND id_feature = {parentID}";
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

                                    //Insert all the new features id in the table
                                    foreach (string feat_id in valueID)
                                    {
                                        string insertQuery = $"INSERT INTO {tableName} (id_feature,id_feature_value,id_product,date_add,date_update) VALUES ({parentID},{feat_id},{ID},GETDATE(),GETDATE())";
                                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                        {
                                            try
                                            {
                                                command.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show($"La commande {insertQuery} est invalide. Merci de prendre un screenshot de ce message et de le transmettre à ExcelTab  - {ex.Message} ");
                                                continue;
                                            }
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
        private void ShowRecipeInTootTip(int eRow, int eColumn)
        {
            // Vérifiez si la cellule actuelle contient la colonne "Recette"
            if (eRow >= 0 && eColumn >= 0 && dataGridView_Selected.Columns[eColumn].Name == "Recette")
            {
                // Vérifiez si la valeur de la cellule a changé depuis la dernière cellule
                if (hoverRow != eRow)
                {
                    // Mettez à jour le contenu du ToolTip avec la nouvelle valeur
                    hoverRow = eRow;

                    // Récupérez le contenu de la colonne "Hidden_Recette" pour la cellule survolée
                    object hiddenRecetteValue = dataGridView_Selected.Rows[eRow].Cells["Detail recette"].Value;

                    // Vérifiez si la valeur n'est pas null
                    if (hiddenRecetteValue != null)
                    {
                        // Créez un ToolTip avec le contenu de "Hidden_Recette"
                        //recipeTooltip.Hide(dataGridView_Selected);
                        recipeTooltip.SetToolTip(dataGridView_Selected, hiddenRecetteValue.ToString().Replace("||", "\n")); ;
                        recipeTooltip.AutoPopDelay = 10000; // Durée d'affichage du ToolTip en millisecondes (3 secondes dans cet exemple)

                    }
                    else
                    {
                        recipeTooltip.Hide(dataGridView_Selected);
                    }
                }
            }
            else
            {
                recipeTooltip.Hide(dataGridView_Selected);
            }
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

                    // afficher l'image
                    if (viewQuery == "ViewProducts")
                    {
                        if (imageCombobox_Selected.Text != "Aucune")
                        {
                            // Récupérez les données de la ligne pour obtenir l'URL de l'image (ajustez cela en fonction de votre structure de données)
                            string imageReference = selectedRow.Cells["Référence"].Value.ToString();
                            string imageURL = selectedRow.Cells["URL image"].Value.ToString();
                            if (imageReference != "" && imageURL != "")
                            {
                                image_Selected.Visible = true;
                                image_Selected.LoadPictures(imageReference, imageURL);
                                ChangeImageLocation(image_Selected, imageCombobox_Selected.Text);
                            }
                            else
                            {
                                image_Selected.Visible = false;
                            }
                        }
                    }
                }
            }
            else
            {
                // masquer l'image
                image_Selected.Visible = false;
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
                else if (SelectType == "icon" && sAlias == "Recette")
                {
                    bShowDialog = true;
                    bMultiSelect = false;
                }
            }

            if (bShowDialog)
            {

                //Open the login form and dim the main form
                //MainForm mainForm = (MainForm)this.Owner;
                BlackOverlay blackOverlay = new BlackOverlay();
                //blackOverlay.Size = mainForm.Size;
                //blackOverlay.Location = mainForm.Location;
                blackOverlay.Show();

                if (sAlias == "Recette")
                {
                    Recipe recipe = new Recipe();
                    recipe.loadData(idArticle, sDeclinaison);
                    recipe.ShowDialog();

                }
                else
                {
                    ListTable list_Select = new ListTable();

                    //Get the value of the Theme column
                    string iTheme = "";
                    string sTheme = "";
                    if (viewQuery == "ViewProducts")
                    {
                        sTheme = dataGridView_Selected.Rows[e.RowIndex].Cells["Thème"].Value.ToString();
                        if (sTheme != "")
                        {
                            iTheme = Database.GetID("TBL_CATEGORY", "name", sTheme);
                        }
                    }


                    //TODO : mettre ceci dans un bloc plus propre
                    switch (sAlias)
                    {
                        case "Thème":
                            list_Select.ViewMode = "TreeView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position");
                            list_Select.Set_Title("Choix de catégorie");
                            break;

                        case "Catégories associées":
                            list_Select.ViewMode = "TreeView";
                            list_Select.MultiSelect = true;
                            list_Select.Load_List("SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position");
                            list_Select.Set_Title("Choix de catégorie");
                            break;

                        case "Type de produit":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Type de produit] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Type de produit' order by id_feature_value");
                            list_Select.Set_Title("Choix du type de produit (Feature)");
                            break;

                        case "Format":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Format] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Format' order by id_feature_value");
                            list_Select.Set_Title("Choix du format (Feature)");
                            break;

                        case "Couleur dominante":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Couleur] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Couleur' order by id_feature_value");
                            list_Select.Set_Title("Choix de la couleur dominante (Feature)");
                            break;

                        case "Couleurs":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = true;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Couleur] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Couleur' order by id_feature_value");
                            list_Select.Set_Title("Choix des couleurs associées (Feature)");
                            break;

                        case "Pièces":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = true;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Pièce] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Pièce' order by id_feature_value");
                            list_Select.Set_Title("Choix des pièces (Feature)");

                            break;

                        case "Styles":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = true;
                            list_Select.Load_List("SELECT TBL_FEATURES_VALUES.id_feature_value as ID, TBL_FEATURES_VALUES.name as [Style] FROM TBL_FEATURES_VALUES Left join TBL_FEATURES on TBL_FEATURES_VALUES.id_feature = TBL_FEATURES.id_feature where TBL_FEATURES.name = 'Styles' order by id_feature_value");
                            list_Select.Set_Title("Choix des styles (Feature)");
                            break;

                        case "Support":
                        case "Support unique":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_ATTRIBUTE_VALUES.id_attribute as ID, TBL_ATTRIBUTE_VALUES.name as [Support] FROM TBL_ATTRIBUTE_VALUES Left join TBL_ATTRIBUTE_GROUP on TBL_ATTRIBUTE_VALUES.id_attribute_group = TBL_ATTRIBUTE_GROUP.id_attribute_group where TBL_ATTRIBUTE_GROUP.name = 'support' order by TBL_ATTRIBUTE_VALUES.name asc");
                            list_Select.Set_Title("Choix du support (Attribute)");
                            break;
                        case "Support Pressing":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_ATTRIBUTE_VALUES.id_attribute as ID, TBL_ATTRIBUTE_VALUES.name as [Support Pressing] FROM TBL_ATTRIBUTE_VALUES Left join TBL_ATTRIBUTE_GROUP on TBL_ATTRIBUTE_VALUES.id_attribute_group = TBL_ATTRIBUTE_GROUP.id_attribute_group where TBL_ATTRIBUTE_GROUP.name = 'support pressing' order by TBL_ATTRIBUTE_VALUES.name asc");
                            list_Select.Set_Title("Choix du support pressing (Attribute)");
                            break;

                        case "Dimension":
                        case "Dimension unique":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            list_Select.Load_List("SELECT TBL_ATTRIBUTE_VALUES.id_attribute as ID, TBL_ATTRIBUTE_VALUES.name as [Dimension] FROM TBL_ATTRIBUTE_VALUES Left join TBL_ATTRIBUTE_GROUP on TBL_ATTRIBUTE_VALUES.id_attribute_group = TBL_ATTRIBUTE_GROUP.id_attribute_group where TBL_ATTRIBUTE_GROUP.name = 'dimensions' order by TBL_ATTRIBUTE_VALUES.id_attribute asc");
                            list_Select.Set_Title("Choix de la dimension (Attribute)");
                            break;

                        case "Mot clé":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            string filteredQuery = "";
                            string titleWindow = "Choix du mot clé";
                            if (iTheme != "" && iTheme != "0")
                            {
                                //Check if there is at least one keyword for the selected theme
                                filteredQuery = $@"SELECT TBL_KEYWORD.id_keyword as ID, TBL_KEYWORD.name as [Mot clé] FROM TBL_KEYWORD 
                                            inner join TBL_CATEGORY_RULE_VALUES on TBL_KEYWORD.id_keyword = TBL_CATEGORY_RULE_VALUES.id_rule_value 
                                            where TBL_CATEGORY_RULE_VALUES.id_category = {iTheme} and TBL_CATEGORY_RULE_VALUES.id_category_rule = 3
                                            order by TBL_KEYWORD.name asc";
                                titleWindow += $" pour {sTheme}";
                            }
                            else
                            {
                                filteredQuery = $"SELECT TBL_KEYWORD.id_keyword as ID, TBL_KEYWORD.name as [Mot clé] FROM TBL_KEYWORD order by TBL_KEYWORD.name asc";
                            }
                            list_Select.Load_List(filteredQuery);
                            list_Select.Set_Title(titleWindow);
                            break;

                        case "Lien maillage":
                            list_Select.ViewMode = "ListView";
                            list_Select.MultiSelect = false;
                            filteredQuery = "";
                            titleWindow = "Choix du lien";
                            if (iTheme != "" && iTheme != "0")
                            {
                                //Check if there is at least one keyword for the selected theme
                                filteredQuery = $@"SELECT TBL_WEBSITE_URL.id_website_url as ID, TBL_WEBSITE_URL.name as [Lien] FROM TBL_WEBSITE_URL 
                                            inner join TBL_CATEGORY_RULE_VALUES on TBL_WEBSITE_URL.id_website_url = TBL_CATEGORY_RULE_VALUES.id_rule_value 
                                            where TBL_CATEGORY_RULE_VALUES.id_category = {iTheme} and TBL_CATEGORY_RULE_VALUES.id_category_rule = 4
                                            order by TBL_WEBSITE_URL.name asc";
                                titleWindow += $" pour {sTheme}";
                            }
                            else
                            {
                                filteredQuery = $"SELECT TBL_WEBSITE_URL.id_website_url as ID, TBL_WEBSITE_URL.name as [Lien] FROM TBL_WEBSITE_URL order by TBL_WEBSITE_URL.name asc";
                            }
                            list_Select.Load_List(filteredQuery);
                            list_Select.Set_Title(titleWindow);
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
        private void DGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowRecipeInTootTip(e.RowIndex, e.ColumnIndex);
        }
        private void DGV_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ShowRecipeInTootTip(e.RowIndex, e.ColumnIndex);
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
                                    DialogResult result = MessageBox.Show("Le produit n°" + ID.ToString() + " est intégré dans Prestashop. Voulez-vous vraiment le supprimer ?", "Confirmation de suppression", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                                    if (result == DialogResult.OK)
                                    {
                                        // User clicked OK, proceed with deletion
                                        cell.Value = "Delete_" + cell.Value;
                                    }
                                    // If the user clicked Cancel, nothing will be done
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
        private void comboBox_Image_Product_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeImageLocation(imageFromWebBox_ProductList, comboBox_Image_ProductList.Text);
        }
        private void ChangeImageLocation(ImageFromWebBox pictureBox, string imageLocation)
        {
            if (imageLocation == "Aucune")
            {
                pictureBox.Visible = false;
            }
            else
            {
                pictureBox.Visible = true;
                int decalage = 20;
                switch (imageLocation)
                {
                    case "Haut - Gauche":
                        pictureBox.Location = new System.Drawing.Point(dataGridView_Selected.Left + decalage, dataGridView_Selected.Top + decalage);
                        break;
                    case "Haut - Droite":
                        pictureBox.Location = new System.Drawing.Point(dataGridView_Selected.Left + dataGridView_Selected.Width - decalage - pictureBox.Width, dataGridView_Selected.Top + decalage);
                        break;
                    case "Bas - Gauche":
                        pictureBox.Location = new System.Drawing.Point(dataGridView_Selected.Left + decalage, dataGridView_Selected.Top + dataGridView_Selected.Height - decalage - pictureBox.Height);
                        break;
                    case "Bas - Droite":
                        pictureBox.Location = new System.Drawing.Point(dataGridView_Selected.Left + dataGridView_Selected.Width - decalage - pictureBox.Width, dataGridView_Selected.Top + dataGridView_Selected.Height - decalage - pictureBox.Height);
                        break;

                }
            }
        }
        private async void button_SynchroPrestashop_Click(object sender, EventArgs e)
        {
            await Task.Run(async () => { await CompareProductLists(); });
        }
        public static async Task CompareProductLists()
        {
            //Get the Template data table
            StatusUpdate?.Invoke($"Préparation des tables...");
            //Load all tables we will work with
            StatusUpdate?.Invoke($"Préparation des tables Prestashop: TBL_PRODUCT");
            DataTable dtBufferP = Database.LoadData("SELECT * FROM TBL_PRODUCT where is_Prestashop = 1 and reference is not null and not reference = ''", true);
            //Remove the non-used column
            dtBufferP.Columns.Remove("is_declinable");
            dtBufferP.Columns.Remove("date_add");
            dtBufferP.Columns.Remove("date_update");
            StatusUpdate?.Invoke($"Préparation des tables Prestashop: TBL_FEATURE_PRODUCT");
            DataTable dtBufferFP = Database.LoadData("SELECT * FROM TBL_FEATURE_PRODUCT", true);
            dtBufferFP.Columns.Remove("date_add");
            dtBufferFP.Columns.Remove("date_update");
            StatusUpdate?.Invoke($"Préparation des tables Prestashop: TBL_CATEGORY_PRODUCT");
            DataTable dtBufferCP = Database.LoadData("SELECT * FROM TBL_CATEGORY_PRODUCT", true);
            dtBufferCP.Columns.Remove("date_add");
            dtBufferCP.Columns.Remove("date_update");
            StatusUpdate?.Invoke($"Préparation des tables Mint: TBL_PRODUCT");
            DataTable dtMainP = Database.LoadData("SELECT * FROM TBL_NEWPRODUCT");
            StatusUpdate?.Invoke($"Préparation des tables Mint: TBL_FEATURE_PRODUCT");
            DataTable dtMainFP = Database.LoadData("SELECT * FROM TBL_FEATURE_PRODUCT");
            StatusUpdate?.Invoke($"Préparation des tables Mint: TBL_CATEGORY_PRODUCT");
            DataTable dtMainCP = Database.LoadData("SELECT * FROM TBL_CATEGORY_PRODUCT");

            int count = 0;
            List<string> AddedProducts = new List<string>();
            List<string> UpdatedProducts = new List<string>();
            List<string> ChangeFeatureProducts = new List<string>();
            int countChangeFeatureProducts = 0;
            List<string> ChangeCategoryProducts = new List<string>();
            int countChangeCategoryProducts = 0;

            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                connection.Open();

                foreach (DataRow pProduct in dtBufferP.Rows)
                {
                    StatusUpdate?.Invoke($"Comparaison des produits...{count}/{dtBufferP.Rows.Count}");
                    string PrestaID = pProduct["id_product"].ToString();
                    DataRow[] types = dtBufferFP.Select($"id_product = {pProduct["id_product"]} and id_feature = 19");
                    DataRow[] formats = dtBufferFP.Select($"id_product = {pProduct["id_product"]} and id_feature = 8");
                    DataRow[] features = dtBufferFP.Select($"id_product = {pProduct["id_product"]} and id_feature not in(19, 8)");
                    DataRow[] categories = dtBufferCP.Select($"id_product = {pProduct["id_product"]}");

                    //look for the row in the datarow features where id_feature = 19
                    int id_type = 0;
                    if (types.Length > 0) { id_type = (types[0]["id_feature_value"] == DBNull.Value) ? 0 : Convert.ToInt32(types[0]["id_feature_value"]); }
                    int id_format = 0;
                    if (formats.Length > 0) { id_format = (formats[0]["id_feature_value"] == DBNull.Value) ? 0 : Convert.ToInt32(formats[0]["id_feature_value"]); }

                    //Check if the product id exists in the main database
                    DataRow[] pMainProduct = dtMainP.Select($"id_product = {pProduct["id_product"]}");

                    if (pMainProduct.Length == 0)
                    {
                        //Look if the equivalent can be found by reference and type
                        string PrestaRef = pProduct["reference"].ToString();
                        pMainProduct = dtMainP.Select($"reference = '{PrestaRef}' AND id_type = {id_type}");

                        if (pMainProduct.Length == 0)
                        {
                            //Look if the equivalent can be found by reference of 1st child and type
                            if (PrestaRef.Contains("-")) { PrestaRef = PrestaRef.Substring(PrestaRef.IndexOf("-") + 1); }
                            if (PrestaRef.Contains("HX")) { PrestaRef = PrestaRef.Substring(0, PrestaRef.IndexOf("HX")); }
                            pMainProduct = dtMainP.Select($"reference = '{PrestaRef}' AND id_type = {id_type}");
                        }
                        if (pMainProduct.Length == 0)
                        {
                            //The product does not exist in the main database, we add it
                            string insertQuery = "Insert into TBL_NEWPRODUCT ";
                            string columns = "(";
                            string values = "(";

                            using (SqlCommand Command = new SqlCommand(insertQuery, connection))
                            {
                                foreach (DataColumn column in dtBufferP.Columns)
                                {
                                    columns += column.ColumnName + ",";
                                    values += $"@{column.ColumnName},";
                                    Command.Parameters.AddWithValue($"@{column.ColumnName}", pProduct[column.ColumnName]);
                                }

                                columns += "id_type, id_format, date_add)";
                                values += "@id_type, @id_format, GETDATE())";

                                insertQuery += columns + " VALUES " + values;
                                Command.CommandText = insertQuery;
                                Command.Parameters.AddWithValue("@id_type", id_type);
                                Command.Parameters.AddWithValue("@id_format", id_format);
                                Command.ExecuteNonQuery();
                            }
                            AddedProducts.Add(pProduct["reference"].ToString());

                            //Add features and categories
                            if (features.Length > 0)
                            {
                                //Create a new datatable with the features
                                DataTable dtFeatures = features.CopyToDataTable();
                                foreach (DataRow row in dtFeatures.Rows)
                                {
                                    insertQuery = "Insert into TBL_FEATURE_PRODUCT";
                                    columns = "(";
                                    values = "(";
                                    using (SqlCommand Command = new SqlCommand(insertQuery, connection))
                                    {
                                        foreach (DataColumn column in dtFeatures.Columns)
                                        {
                                            columns += column.ColumnName + ",";
                                            values += $"{row[column.ColumnName]},";
                                        }
                                        columns += "date_add)";
                                        values += $"GETDATE())";
                                        insertQuery += columns + " VALUES " + values;
                                        Command.CommandText = insertQuery;
                                        Command.ExecuteNonQuery();
                                    }
                                }
                            }
                            if (categories.Length > 0)
                            {
                                //Create a new datatable with the categories
                                DataTable dtCategories = categories.CopyToDataTable();
                                foreach (DataRow row in dtCategories.Rows)
                                {
                                    insertQuery = "Insert into TBL_CATEGORY_PRODUCT";
                                    columns = "(";
                                    values = "(";
                                    using (SqlCommand Command = new SqlCommand(insertQuery, connection))
                                    {
                                        foreach (DataColumn column in dtCategories.Columns)
                                        {
                                            columns += column.ColumnName + ",";
                                            values += $"{row[column.ColumnName]},";
                                        }
                                        columns += "date_add)";
                                        values += $"GETDATE())";
                                        insertQuery += columns + " VALUES " + values;
                                        Command.CommandText = insertQuery;
                                        Command.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //The product exists but under a different id, we update it
                            string MainID = pMainProduct[0]["id_product"].ToString();
                            string updateQuery = "Update TBL_NEWPRODUCT Set ";
                            string setValues = "";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                foreach (DataColumn column in dtBufferP.Columns)
                                {
                                    if (column.ColumnName == "reference") { continue; } //do not change the reference                                    
                                    if (pProduct[column.ColumnName] == DBNull.Value) { continue; }//Modification 21/08/2024 : if the column is empty, do not update it
                                    setValues += $"{column.ColumnName} = @{column.ColumnName}, ";
                                    updateCommand.Parameters.AddWithValue($"@{column.ColumnName}", pProduct[column.ColumnName]);
                                }

                                // Add type and format as parameters
                                if (id_type != 0)
                                {
                                    setValues += $"id_type = @id_type, ";
                                    updateCommand.Parameters.AddWithValue("@id_type", id_type);
                                }
                                if (id_format != 0)
                                {
                                    setValues += $"id_format = @id_format, ";
                                    updateCommand.Parameters.AddWithValue("@id_format", id_format);
                                }

                                setValues += "date_update = GETDATE() ";
                                updateQuery += setValues + $" WHERE id_product = {MainID}";


                                updateCommand.CommandText = updateQuery;
                                updateCommand.ExecuteNonQuery();
                            }

                            UpdatedProducts.Add(pProduct["reference"].ToString());

                            //Change features and categories
                            updateQuery = $"Update TBL_FEATURE_PRODUCT Set id_product = {PrestaID}, date_update = GETDATE() WHERE id_product = {MainID}";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.ExecuteNonQuery();
                            }
                            updateQuery = $"Update TBL_CATEGORY_PRODUCT Set id_product = {PrestaID}, date_update = GETDATE() WHERE id_product = {MainID}";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        //The product exists in the main database, we check if it has been modified
                        //We can't rely on the date_update, we need to check each column
                        bool modified = false;
                        foreach (DataColumn column in dtBufferP.Columns)
                        {
                            if (pProduct[column.ColumnName].ToString() != pMainProduct[0][column.ColumnName].ToString())
                            {
                                modified = true;
                                break;
                            }
                        }
                        if (modified)
                        {
                            //The product has been modified, we update it
                            string updateQuery = "Update TBL_NEWPRODUCT Set ";
                            string setValues = "";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                foreach (DataColumn column in dtBufferP.Columns)
                                {
                                    setValues += $"{column.ColumnName} = @{column.ColumnName}, ";
                                    updateCommand.Parameters.AddWithValue($"@{column.ColumnName}", pProduct[column.ColumnName]);
                                }

                                // Add type and format as parameters
                                if (id_type != 0)
                                {
                                    setValues += $"id_type = @id_type, ";
                                    updateCommand.Parameters.AddWithValue("@id_type", id_type);
                                }
                                if (id_format != 0)
                                {
                                    setValues += $"id_format = @id_format, ";
                                    updateCommand.Parameters.AddWithValue("@id_format", id_format);
                                }

                                setValues += "date_update = GETDATE() ";
                                updateQuery += setValues + $" WHERE id_product = {PrestaID}";


                                updateCommand.CommandText = updateQuery;
                                updateCommand.ExecuteNonQuery();
                            }

                            UpdatedProducts.Add(pProduct["reference"].ToString());
                        }
                        //Also compare the features and categories
                        //Use dtMainCP and dtMainFP with a query on id_product = PrestaID to compare to features and categories

                        if (features.Length > 0)
                        {
                            DataRow[] featuresM = dtMainFP.Select($"id_product = {pProduct["id_product"]} and id_feature not in(19, 8)");
                            // Construct a single query for inserting missing features
                            var insertQueries = features.AsEnumerable()
                                .Where(row => !featuresM.AsEnumerable()
                                    .Any(mRow => mRow.Field<int>("id_feature_value") == row.Field<int>("id_feature_value")))
                                .Select(row => $@"INSERT INTO TBL_FEATURE_PRODUCT (id_feature, id_feature_value, id_product, date_add)
                            VALUES ({row.Field<int>("id_feature")}, {row.Field<int>("id_feature_value")},
                                    {row.Field<int>("id_product")}, GETDATE())");


                            // Construct a single query for deleting obsolete features
                            var deleteQueries = featuresM.AsEnumerable()
                                .Where(mRow => !features.AsEnumerable()
                                    .Any(row => row.Field<int>("id_feature_value") == mRow.Field<int>("id_feature_value")))
                                .Select(mRow => $@"DELETE FROM TBL_FEATURE_PRODUCT 
                            WHERE id_product = {mRow.Field<int>("id_product")} 
                            AND id_feature_value = {mRow.Field<int>("id_feature_value")}");

                            // Combine all insert and delete queries into one SQL command
                            string combinedQuery = string.Join(";", insertQueries.Concat(deleteQueries));

                            if (combinedQuery != "")
                            {
                                // Execute the combined query
                                using (SqlCommand command = new SqlCommand(combinedQuery, connection))
                                {
                                    //Return how many rows are affected
                                    int rowsAffected = command.ExecuteNonQuery();
                                    countChangeFeatureProducts += rowsAffected;
                                    ChangeFeatureProducts.Add(pProduct["id_product"].ToString());
                                }
                            }
                        }
                        if (categories.Length > 0)
                        {
                            DataRow[] categoriesM = dtMainCP.Select($"id_product = {pProduct["id_product"]}");
                            // Construct a single query for inserting missing features
                            var insertQueries = categories.AsEnumerable()
                                .Where(row => !categoriesM.AsEnumerable()
                                    .Any(mRow => mRow.Field<int>("id_category") == row.Field<int>("id_category")))
                                .Select(row => $@"INSERT INTO TBL_CATEGORY_PRODUCT (id_category, id_product, date_add)
                            VALUES ({row.Field<int>("id_category")},
                                    {row.Field<int>("id_product")}, GETDATE())");


                            // Construct a single query for deleting obsolete features
                            var deleteQueries = categoriesM.AsEnumerable()
                                .Where(mRow => !categories.AsEnumerable()
                                    .Any(row => row.Field<int>("id_category") == mRow.Field<int>("id_category")))
                                .Select(mRow => $@"DELETE FROM TBL_CATEGORY_PRODUCT 
                            WHERE id_product = {mRow.Field<int>("id_product")} 
                            AND id_category = {mRow.Field<int>("id_category")}");

                            // Combine all insert and delete queries into one SQL command
                            string combinedQuery = string.Join(";", insertQueries.Concat(deleteQueries));

                            if (combinedQuery != "")
                            {
                                // Execute the combined query
                                using (SqlCommand command = new SqlCommand(combinedQuery, connection))
                                {
                                    //Return how many rows are affected
                                    int rowsAffected = command.ExecuteNonQuery();
                                    countChangeCategoryProducts += rowsAffected;
                                    ChangeCategoryProducts.Add(pProduct["id_product"].ToString());
                                }
                            }
                        }
                        count++;
                    }
                }

                //Now, check if any product was removed from prestashop, and unflag the is_Prestashop bit column
                //Check if some is_Prestashop product from dtMainP are not in dtBufferP
                DataTable dtIsPrestashop = dtMainP.Select("is_Prestashop = 1").CopyToDataTable();
                List<object> idProductsInBufferP = dtBufferP.AsEnumerable().Select(row => row["id_product"]).ToList();

                // Filter out rows from dtIsPrestashop that are also in dtBufferP
                DataRow[] rowsNotInBufferP = dtIsPrestashop.AsEnumerable()
                    .Where(row => !idProductsInBufferP.Contains(row["id_product"]))
                    .ToArray();

                int countIsPresta = 1;
                if (rowsNotInBufferP.Length > 0)
                {
                    foreach (DataRow row in rowsNotInBufferP)
                    {
                        StatusUpdate?.Invoke($"Marquage des produits supprimés de prestashop...{countIsPresta}/{rowsNotInBufferP.Length}");
                        string updateQuery = $"Update TBL_NEWPRODUCT Set is_Prestashop = 0, date_update = GETDATE() WHERE id_product = {row["id_product"]}";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.ExecuteNonQuery();

                        }
                        countIsPresta++;
                        UpdatedProducts.Add(row["reference"].ToString());
                    }
                }
                //Update the date in CATALOG_TABLES
                string updateDateQuery = $"UPDATE CATALOG_TABLES SET last_transfer_date = GETDATE() WHERE table_name = 'TBL_PRODUCT'";
                using (SqlCommand updateDateCommand = new SqlCommand(updateDateQuery, connection))
                {
                    updateDateCommand.ExecuteNonQuery();
                }

            }
            StatusUpdate?.Invoke("");
            //RefreshProductListUpdateLabel();

        }
        #endregion

        #region Procedures to add and modify rules to add to products
        private void button_SaveCatAssoc_Click(object sender, EventArgs e)
        {
            //CATEGORIES
            List<string> categories = new List<string>();
            foreach (TreeNode node in treeView_SC_CatAssoc.Nodes)
            {
                if (node.Checked) { categories.Add(node.Tag.ToString()); }
                ReadCategories(node, categories);
            }
            string theme = treeView_SC_Theme.SelectedNode.Tag.ToString();
            //Delete the existing links with categories and features
            string query = $@"DELETE FROM TBL_CATEGORY_RULE_VALUES 
                    WHERE id_category_rule = 1 and id_rule_value = {theme}";
            Database.ExecuteQuery(query);

            foreach (string category in categories)
            {
                query = $@"INSERT INTO TBL_CATEGORY_RULE_VALUES (id_category,id_category_rule,id_rule_value,date_add,date_update) 
                                                    VALUES ({category},1, {theme}, GETDATE(), GETDATE())";
                Database.ExecuteQuery(query);
            }
            label_CountCatAssoc.Text = $"{categories.Count} catégories associées";
            MessageBox.Show($"{categories.Count} catégories associées sauvegardées");
        }
        private void button_SaveCatKeyword_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                connection.Open();

                //KEYWORDS
                List<string> selectedItems = new List<string>();
                foreach (ListViewItem item in listView_CatKeyword.Items)
                {
                    if (item.Checked) { selectedItems.Add(item.Tag.ToString()); }
                }
                string theme = treeView_SC_CatKeyword.SelectedNode.Tag.ToString();
                //Delete the existing links with categories and keywords
                string query = $@"DELETE FROM TBL_CATEGORY_RULE_VALUES 
                            WHERE id_category_rule = 3 and id_category = {theme}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                foreach (string selectedItem in selectedItems)
                {
                    query = $@"INSERT INTO TBL_CATEGORY_RULE_VALUES (id_category,id_category_rule,id_rule_value,date_add,date_update) 
                                                            VALUES ({theme},3,{selectedItem} , GETDATE(), GETDATE())";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                label_CountCatAssoc.Text = $"{selectedItems.Count} mot clés associés";
                MessageBox.Show($"{selectedItems.Count} mot clés associés sauvegardés");
            }
        }
        private void button_SaveCatURL_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                connection.Open();

                //KEYWORDS
                List<string> selectedItems = new List<string>();
                foreach (ListViewItem item in listView_CatURL.Items)
                {
                    if (item.Checked) { selectedItems.Add(item.Tag.ToString()); }
                }
                string theme = treeView_SC_CatURL.SelectedNode.Tag.ToString();
                //Delete the existing links with categories and keywords
                string query = $@"DELETE FROM TBL_CATEGORY_RULE_VALUES 
                            WHERE id_category_rule = 4 and id_category = {theme}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                foreach (string selectedItem in selectedItems)
                {
                    query = $@"INSERT INTO TBL_CATEGORY_RULE_VALUES (id_category,id_category_rule,id_rule_value,date_add,date_update) 
                                                            VALUES ({theme},4,{selectedItem} , GETDATE(), GETDATE())";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                label_CountCatAssoc.Text = $"{selectedItems.Count} URLs associées";
                MessageBox.Show($"{selectedItems.Count} URLs associées sauvegardées");
            }
        }
        private void button_SaveCatRule_Click(object sender, EventArgs e)
        {
            //CATEGORIES
            List<string> categories = new List<string>();
            foreach (TreeNode node in treeView_SC_CatRule.Nodes)
            {
                if (node.Checked) { categories.Add(node.Tag.ToString()); }
                ReadCategories(node, categories);
            }

            //Get type and feature
            ComboBoxItemWithTag selectedType = (ComboBoxItemWithTag)comboBox_RuleType.SelectedItem;
            string tagType = selectedType.Tag;
            ComboBoxItemWithTag selectedValue = (ComboBoxItemWithTag)comboBox_RuleValue.SelectedItem;
            string tagValue = selectedValue.Tag;

            //Delete the existing links with categories and features
            string query = $@"DELETE FROM TBL_CATEGORY_RULE_VALUES 
                        WHERE id_category_rule = 2 and id_rule_value = {tagValue} and id_type_product = {tagType}";
            Database.ExecuteQuery(query);

            foreach (string category in categories)
            {
                query = $@"INSERT INTO TBL_CATEGORY_RULE_VALUES (id_category,id_category_rule,id_rule_value,id_type_product,date_add,date_update) 
                                                        VALUES ({category},2, {tagValue},{tagType}, GETDATE(), GETDATE())";
                Database.ExecuteQuery(query);
            }
            label_CountCatRule.Text = $"{categories.Count} catégories associées";
            MessageBox.Show($"{categories.Count} catégories associées sauvegardées");
        }
        private void treeView_SC_Theme_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer_CatAssoc.Panel2.Enabled = true;
            string catText = e.Node.Text;
            label_CatTheme.Text = $"{catText}";
            ClearCategories(treeView_SC_CatAssoc.Nodes);

            string query = $@"SELECT id_category From TBL_CATEGORY_RULE_VALUES
                Where id_category_rule = 1 and id_rule_value = {e.Node.Tag.ToString()}"; //1 = cat associée
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count > 0)
            {
                CheckCategories(treeView_SC_CatAssoc.Nodes, dt);
                label_CountCatAssoc.Text = $"{dt.Rows.Count} catégories associées";
            }
            else { label_CountCatAssoc.Text = $"Aucune catégorie associée"; }
        }
        private void treeView_SC_CatKeyword_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer_CatKeyword.Panel2.Enabled = true;
            string catText = e.Node.Text;
            label_CatKeyword.Text = $"{catText}";
            ClearCategories(treeView_SC_CatKeyword.Nodes);

            string query = $@"SELECT id_rule_value From TBL_CATEGORY_RULE_VALUES
                Where id_category_rule = 3 and id_category = {e.Node.Tag.ToString()}"; //3 = keyword
            DataTable dt = Database.LoadData(query);

            foreach (ListViewItem item in listView_CatKeyword.Items) { item.Checked = false; }

            if (dt.Rows.Count > 0)
            {
                //check the items from the listview for which the tag matches id_rule_value
                foreach (ListViewItem item in listView_CatKeyword.Items)
                {
                    if (dt.Select($"id_rule_value = {item.Tag.ToString()}").Length > 0)
                    {
                        item.Checked = true;
                    }
                }

                label_CountCatKeyword.Text = $"{dt.Rows.Count} mot clés associés";
            }
            else { label_CountCatKeyword.Text = $"Aucun mot clé associé"; }
        }
        private void treeView_SC_CatURL_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer_CatURL.Panel2.Enabled = true;
            string catText = e.Node.Text;
            label_CatURL.Text = $"{catText}";
            ClearCategories(treeView_SC_CatURL.Nodes);

            string query = $@"SELECT id_rule_value From TBL_CATEGORY_RULE_VALUES
                Where id_category_rule = 4 and id_category = {e.Node.Tag.ToString()}"; //4 = website URL
            DataTable dt = Database.LoadData(query);

            foreach (ListViewItem item in listView_CatURL.Items) { item.Checked = false; }

            if (dt.Rows.Count > 0)
            {
                //check the items from the listview for which the tag matches id_rule_value
                foreach (ListViewItem item in listView_CatURL.Items)
                {
                    if (dt.Select($"id_rule_value = {item.Tag.ToString()}").Length > 0)
                    {
                        item.Checked = true;
                    }
                }
                label_CountCatURL.Text = $"{dt.Rows.Count} URLs associées";
            }
            else { label_CountCatURL.Text = $"Aucune URL associée"; }
        }
        private void comboBox_RuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_RuleType.SelectedItem != null && comboBox_RuleValue.SelectedItem != null)
            {
                LoadCatRuleTreeView();
            }
        }
        private void comboBox_RuleFeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear le combobox value
            comboBox_RuleValue.Items.Clear();

            // Charger le combobox valeur
            if (comboBox_RuleFeature.SelectedItem != null)
            {
                ComboBoxItemWithTag selectedComboBoxItem = (ComboBoxItemWithTag)comboBox_RuleFeature.SelectedItem;
                string tagFeature = selectedComboBoxItem.Tag;

                if (tagFeature == "19")
                {
                    comboBox_RuleValue.Enabled = false;
                    //Copy le type du premier combobox
                    ComboBoxItemWithTag selectedTypeItem = (ComboBoxItemWithTag)comboBox_RuleType.SelectedItem;
                    string text = selectedTypeItem.Text;
                    string tag = selectedTypeItem.Tag;
                    comboBox_RuleValue.Items.Add(new ComboBoxItemWithTag(text, tag));

                }
                else
                {
                    comboBox_RuleValue.Enabled = true;
                    string query = $@"Select id_feature_value,name FROM TBL_FEATURES_VALUES 
                            Where id_feature = {tagFeature}";
                    DataTable dt = Database.LoadData(query);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string text = row["name"].ToString();
                            string tag = row["id_feature_value"].ToString();
                            comboBox_RuleValue.Items.Add(new ComboBoxItemWithTag(text, tag));
                        }
                    }

                }
                comboBox_RuleValue.SelectedItem = comboBox_RuleValue.Items[0];

            }


        }
        private void comboBox_RuleValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_RuleValue.SelectedItem != null && comboBox_RuleType.SelectedItem != null)
            {
                LoadCatRuleTreeView();
            }
        }
        private void LoadKeywordAndURL()
        {
            string query = $"SELECT id_keyword, name FROM TBL_KEYWORD order by name asc";
            DataTable dtValues = Database.LoadData(query);
            foreach (DataRow valueRow in dtValues.Rows)
            {
                ListViewItem item = new ListViewItem(valueRow["name"].ToString());
                item.Tag = valueRow["id_keyword"].ToString();
                listView_CatKeyword.Items.Add(item);
            }
            listView_CatKeyword.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            query = $"SELECT id_website_url, name FROM TBL_WEBSITE_URL order by name asc";
            dtValues = Database.LoadData(query);
            foreach (DataRow valueRow in dtValues.Rows)
            {
                ListViewItem item = new ListViewItem(valueRow["name"].ToString());
                item.Tag = valueRow["id_website_url"].ToString();
                listView_CatURL.Items.Add(item);
            }
            listView_CatURL.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        #endregion


        #region Procedures launched by other forms
        #endregion

        # region One shot procedures
        private void OneShotAddRecipesToSimpleProducts()
        {
            //Get all simple products with no recipe
            string query = $@"SELECT TBL_ARTICLE.id_article,TBL_ARTICLE.name from TBL_ARTICLE LEFT JOIN TBL_RECETTES on TBL_ARTICLE.id_article = TBL_RECETTES.id_article
                where TBL_ARTICLE.is_declinaison = 0 and TBL_RECETTES.id_article is null";
            DataTable dt = Database.LoadData(query);

            if (dt.Rows.Count == 0) { return; }
            using (SqlConnection connection = new SqlConnection(Database.MainConnectionString()))
            {
                connection.Open();
                string insertQuery = "";
                foreach (DataRow row in dt.Rows)
                {
                    if ((int)row["id_article"] > 853)
                    {
                        return;
                    }
                    //Check if rawmat already exists
                    string rawmatID = Database.GetID("TBL_RAWMAT", "name", (string)row["name"]);

                    if (rawmatID == "0")
                    {
                        //Create the rawmat in TBL_RAWMAT
                        insertQuery = $"INSERT INTO TBL_RAWMAT (name,cluster,is_surface,date_add) VALUES ('{(string)row["name"]}','Produit simple',0,GETDATE()); SELECT SCOPE_IDENTITY()";
                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            rawmatID = command.ExecuteScalar().ToString();
                        }
                    }
                    //Create a recipe with this rawmat in TBL_RECETTES
                    insertQuery = $"INSERT INTO TBL_RECETTES (id_article,id_rawmat,quantity,date_add) VALUES ({row["id_article"].ToString()},{rawmatID},1,GETDATE())";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

        }
        #endregion



    }
}

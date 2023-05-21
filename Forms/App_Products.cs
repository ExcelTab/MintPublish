using iText.StyledXmlParser.Jsoup.Nodes;
using Mint.Code;
using Mint.Controls;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Mint.Forms
{
    public partial class App_Products : Form
    {
        DataTable categoryDataTable;
        public App_Products()
        {
            InitializeComponent();
            LoadCategoryData();
            PopulateTreeView();
            LoadFeatureList();
            LoadDeclinaisonList();
        }

        private void LoadCategoryData()
        {
            // Create a command to retrieve the category data
            string query = "SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position";
            categoryDataTable = Database.LoadData(query);
        }

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

            // Set the tree view's root node
            Trv_Category.Nodes.Add(root);
            Trv_Category.Nodes[0].Expand();
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
                double categoryId = Convert.ToDouble(node.Tag);
                if (assignedCategories.AsEnumerable().Any(row => categoryId == row.Field<double>(0)))
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
        private void Btn_Modify_Click(object sender, EventArgs e)
        {

            BlackOverlay blackOverlay = new BlackOverlay();
            //blackOverlay.Size = mainForm.Size;
            //blackOverlay.Location = mainForm.Location;
            blackOverlay.Show();

            ListTable list_product = new ListTable();
            //list_product.Load_List("SELECT id_product,name,reference,category,artist From ViewProducts order by Name");
            list_product.Load_List("SELECT * FROM ViewProducts");
            list_product.Set_Title("Choisissez un produit à modifier");
            list_product.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            //this.Show();
            int ProductID = int.Parse(list_product.ID_Selected);

            ClearForm();

            //Load product info from the database
            string query = "SELECT * FROM ViewProducts WHERE id_product = " + ProductID.ToString();
            DataTable dt = Database.LoadData(query);

            //Set the textbox value from the datatable
            foreach (DataRow reader in dt.Rows)
            {
                // set the  textbox values
                Grp_Product.Text = ProductID.ToString();
                Txt_Name.Text = reader["name"].ToString();
                Txt_Reference.Text = reader["reference"].ToString();
                Txt_Artist.Text = reader["manufacturer"].ToString();
                Txt_Artist.Tag = reader["id_manufacturer"].ToString();
                Txt_EAN13.Text = reader["ean13"].ToString();
                Txt_MainCategory.Text = reader["category"].ToString();
                Txt_MainCategory.Tag = reader["id_category"].ToString();

                //get the image product from the URL
                string imageUrl = "https://hexoa.fr/61376-thickbox_default/affiche-maison-de-pecheur-en-arctique.jpg";
                Img_Product.ImageLocation = imageUrl;

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
                            double featureId = Convert.ToDouble(item.Tag);
                            if (dtFeature.AsEnumerable().Any(row => featureId == row.Field<double>(0)))
                                item.Checked = true;
                        }
                    }
                }
            }
        }

        private void Load_Artist_List(object sender, EventArgs e)
        {
            //Open the login form and dim the main form
            //MainForm mainForm = (MainForm)this.Owner;
            BlackOverlay blackOverlay = new BlackOverlay();
            //blackOverlay.Size = mainForm.Size;
            //blackOverlay.Location = mainForm.Location;
            blackOverlay.Show();

            ListTable list_artist = new ListTable();
            list_artist.Load_List("SELECT id_manufacturer as ID,name as Artist FROM TBL_MANUFACTURER order by name Asc");
            list_artist.Set_Title("Artiste");
            list_artist.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            //this.Show();
            Txt_Artist.Text = list_artist.Text_Selected;
            Txt_Artist.Tag = list_artist.ID_Selected;
        }

        private void Load_Main_Category_List(object sender, EventArgs e)
        {
            BlackOverlay blackOverlay = new BlackOverlay();
            blackOverlay.Show();

            ListTable list_category = new ListTable();
            list_category.ViewMode = 1;
            list_category.Load_List("SELECT id_Category, is_root_category, id_Parent, level_Depth, Name, nleft, nright, position FROM TBL_CATEGORY order by position");
            list_category.Set_Title("Ccatégorie de base");
            list_category.ShowDialog();

            //Once the login form is closed
            blackOverlay.Close();
            //this.Show();
            Txt_MainCategory.Text = list_category.Text_Selected;
            Txt_MainCategory.Tag = list_category.ID_Selected;
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            ClearForm();

        }

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

            //Uncheck all featuires in the TabControl
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
            //Uncheck all declinaisons
            foreach (Control control in Pan_Declinaisons.Controls)
            {
                if (control is GroupListview groupListview)
                {
                    groupListview.Clear_Selection();
                }
            }
        }

        private void LoadFeatureList()
        {
            //Load the list of all features, and create a new tab page for each feature
            string query = "SELECT id_feature, name FROM TBL_FEATURES";
            DataTable dtFeatures = Database.LoadData(query);

            //For each feature, create a new tab page with a fully docked listview
            foreach (DataRow row in dtFeatures.Rows)
            {
                TabPage tabPage = new TabPage(row["name"].ToString());
                tabPage.Tag = row["id_feature"].ToString();
                tabPage.AutoScroll = true;
                ListView listView = new ListView();
                listView.Dock = DockStyle.Fill;
                listView.CheckBoxes = true;
                listView.View = View.Details;
                listView.Columns.Add("a virer");

                query = "SELECT id_feature_value, name FROM TBL_FEATURES_VALUES WHERE id_feature = " + row["id_feature"].ToString();
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
        }

        private void LoadDeclinaisonList()
        {
            string queryGroup = "SELECT DISTINCT id_group, groupe, position FROM ViewDeclinaisons ORDER BY position DESC";
            DataTable dtGroup = Database.LoadData(queryGroup);

            // Load all the declinaisons data into a single DataTable
            string queryDeclinaisons = "SELECT id_group, id_article, référence, longueur, largeur, atelier FROM ViewDeclinaisons ORDER BY référence ASC";
            DataTable dtAllDeclinaisons = Database.LoadData(queryDeclinaisons);

            // Loop through the distinct groups and filter the DataTable for each group
            foreach (DataRow row in dtGroup.Rows)
            {
                string groupId = row["id_group"].ToString();
                string groupName = row["groupe"].ToString();

                // Create a new DataTable with the filtered rows
                DataTable dtDeclinaisons = dtAllDeclinaisons.Select("id_group = " + groupId).CopyToDataTable();
                dtDeclinaisons.Columns.Remove("id_group");

                // Create and add the custom control to the Grp
                GroupListview groupListview = new GroupListview();
                Pan_Declinaisons.Controls.Add(groupListview);
                groupListview.Dock = DockStyle.Top;
                groupListview.Load_Data(groupName, dtDeclinaisons);
            }





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
                imageForm.Size = new Size(enlargedPictureBox.Image.Width + 20, enlargedPictureBox.Image.Height + 20);
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

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            //TODO : Détecter si nouvel artiste, nouvelle catégorie
            if (Txt_Artist.Tag == null)
            {
                MessageBox.Show("Il n'est pas encore possible de créer un nouvel artiste lors de la création/modification de produit");
                Txt_Artist.Tag = "0";
            }
            if (Txt_MainCategory.Tag == null)
            {
                MessageBox.Show("Il n'est pas encore possible de créer une nouvelle catégorie lors de la création/modification de produit");
                Txt_MainCategory.Tag = "0";
            }

            //Lire toutes les catégories cochées
            List<string> categories = new List<string>();
            foreach (TreeNode node in Trv_Category.Nodes)
            {
                if (node.Checked) { categories.Add(node.Tag.ToString()); }
                ReadCategories(node, categories);
            }
            string categoryList = string.Join(",", categories);

            //Lire tous les features cochés
            List<string> features = new List<string>();
            foreach (TabPage tabPage in Tab_Features.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is ListView listView)
                    {
                        foreach (ListViewItem item in listView.Items)
                        {
                            if (item.Checked) { features.Add(item.Tag.ToString()); }
                        }
                    }
                }
            }
            string featureList = string.Join(",", features);

            //Lire toutes les déclinaisons cochées
            List<string> declinaisons = new List<string>();
            foreach (Control control in Pan_Declinaisons.Controls)
            {
                if (control is GroupListview groupListview)
                {
                    declinaisons.AddRange(groupListview.OutputTags);
                }
            }
            string declinaisonList = string.Join(",", declinaisons);

            //créer un nouveau input dans une table NEWPRODUTS
            string query = "INSERT INTO TBL_NEWPRODUCT (prestashop_id,name,ean13,id_category_default,id_manufacturer,reference,category_list,features_list,declinaison_list)" +
                " VALUES (@prestashop_id,@name,@ean13,@id_category_default,@id_manufacturer,@reference,@category_list,@features_list,@declinaison_list)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@prestashop_id", Grp_Product.Text);
            parameters.Add("@name", Txt_Name.Text);
            parameters.Add("@ean13", Txt_EAN13.Text);
            parameters.Add("@id_category_default", Txt_MainCategory.Tag);
            parameters.Add("@id_manufacturer", Txt_Artist.Tag);
            parameters.Add("@reference", Txt_Reference.Text);
            parameters.Add("@category_list", categoryList);
            parameters.Add("@features_list", featureList);
            parameters.Add("@declinaison_list", declinaisonList);
            Database.ExecuteQuery(query, parameters);

            //vider les champs
            ClearForm();

        }

        private void Btn_Extract_Click(object sender, EventArgs e)
        {
            //Mettre la table NEWPRODUCTS dans un fichier excel
            string query = "SELECT * FROM TBL_NEWPRODUCT";
            DataTable dt = Database.LoadData(query);

            string queryDeclinaison = "SELECT id_article, name FROM TBL_ARTICLE";
            DataTable dtArticle = Database.LoadData(queryDeclinaison);

            DataTable dtResult = dt.Copy();

            //Modify the datatable to get one row per declinaison
            foreach (DataRow row in dt.Rows)
            {
                string declinaisonList = row["declinaison_list"].ToString();
                if(declinaisonList != string.Empty)
                {
                    string[] declinaisons = declinaisonList.Split(',');
                    int position = 0;
                    foreach (string declinaison in declinaisons)
                    {
                        position++;
                        string formattedPosition = position.ToString().PadLeft(5, '0');

                        DataRow[] articleRows = dtArticle.Select($"id_article = '{declinaison}'");
                        string articleName = articleRows[0]["name"].ToString();

                        int index = articleName.IndexOf('-');  // Find the position of the '-' character
                                                               //TODO : changer ceci, vérifier si c'est bien ce que Mathieu veut
                        string sDeclinaison = articleName + "-" + row["reference"];
                        if (index != -1)  // If the '-' character is found
                        {
                            sDeclinaison = articleName.Substring(0, index + 1) + row["reference"] + articleName.Substring(index + 1);
                        }

                        DataRow newRow = dtResult.NewRow();
                        newRow["prestashop_id"] = row["prestashop_id"];
                        newRow["name"] = "declinaison";
                        newRow["ean13"] = "000000000";
                        newRow["id_category_default"] = row["id_category_default"];
                        newRow["id_manufacturer"] = row["id_manufacturer"];
                        newRow["reference"] = row["reference"] + "_" + formattedPosition;
                        newRow["category_list"] = "";
                        newRow["features_list"] = "";
                        newRow["declinaison_list"] = sDeclinaison;
                        dtResult.Rows.Add(newRow);
                    }
                }
                
            }
            //Sort the datatable by reference
            dtResult.DefaultView.Sort = "reference ASC";
            DataTable sortedDt = dtResult.DefaultView.ToTable();

            string path = "output.csv";
            CSV.CreateCsv(sortedDt, path, true);
            

            //TODO : prévoir un bouton pour vider la table NEWPRODUCTS sur le formulaire

        }

        private void Txt_Artist_TextChanged(object sender, EventArgs e)
        {
            Txt_Artist.Tag = null;

        }

        private void Txt_MainCategory_TextChanged(object sender, EventArgs e)
        {
            Txt_MainCategory.Tag = null;
        }



    }
}

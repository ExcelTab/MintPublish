using Mint.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mint.Forms
{

    public partial class Recipe : Form
    {
        DataTable dtRawMat = new DataTable();
        public Recipe()
        {
            InitializeComponent();

            string query = "SELECT * FROM TBL_RAWMAT where name is not null Order by Cluster, Name";
            dtRawMat = Database.LoadData(query);

            //Load all raw materials that have the Cluster Surface to the combobox_Surface
            var surfaceMaterials = dtRawMat.AsEnumerable()
                .Where(row => row.Field<string>("Cluster") == "Surface")
                .Select(row => row.Field<string>("Name"))
                .ToList();
            comboBox_Surface.DataSource = surfaceMaterials;

            //Load all raw materials that have the Cluster Extra to the combobox_Extra
            var extraMaterials = dtRawMat.AsEnumerable()
                .Where(row => row.Field<string>("Cluster") == "Extra")
                .Select(row => row.Field<string>("Name"))
                .ToList();
            comboBox_Extra.DataSource = extraMaterials;

            //Load all raw materials that have the Cluster Produit simple to the combobox_ProduitSimple
            var produitSimpleMaterials = dtRawMat.AsEnumerable()
                .Where(row => row.Field<string>("Cluster") == "Produit simple")
                .Select(row => row.Field<string>("Name"))
                .ToList();
            comboBox_ProduitSimple.DataSource = produitSimpleMaterials;

            //Load all other raw mat to the datagridview combobox
            var filteredMaterials = dtRawMat.AsEnumerable()
            .Where(row => row.Field<string>("Cluster") != "Surface" &&
            row.Field<string>("Cluster") != "Produit simple" &&
            row.Field<string>("Cluster") != "Extra")
            .Select(row => row.Field<string>("Name"))
            .ToList();

            // Assuming "RawMat" is the name of the DataGridViewComboBoxColumn
            var comboColumn = dataGridView_OtherRawMat.Columns["RawMat"] as DataGridViewComboBoxColumn;

            if (comboColumn != null)
            {
                comboColumn.DataSource = filteredMaterials;
            }


        }
        public void loadData(string id_article, string declinaison)
        {
            label_idArticle.Text = id_article;
            label_nameArticle.Text = declinaison;

            checkBox_ProduitSimple.Checked = false;
            checkBox_Surface.Checked = false;
            checkBox_OtherRawMat.Checked = false;
            checkBox_Extra.Checked = false;

            string query = $"SELECT * FROM Link_ArticleToRawMat where id_article = {id_article}";
            DataTable dt = Database.LoadData(query);
            if (dt.Rows.Count == 0) { return; }

            //Get Article name and ID in label
            label_idArticle.Text = dt.Rows[0]["id_article"].ToString();
            label_nameArticle.Text = dt.Rows[0]["article"].ToString();

            //Get dimension
            label_dimension.Text = $@"Longueur : {dt.Rows[0]["long"].ToString()} cm ({dt.Rows[0]["debord_long"].ToString()} cm) et Largeur : {dt.Rows[0]["larg"].ToString()} cm ({dt.Rows[0]["debord_larg"].ToString()} cm)";
            label_meters.Text = $@"soit {dt.Rows[0]["meters"].ToString()} m² de ";

            //Get Raw Mats
            foreach (DataRow row in dt.Rows)
            {
                if ((bool)row["is_surface"])
                {
                    checkBox_Surface.Checked = true;
                    comboBox_Surface.Text = row["rawmat"].ToString();
                }
                else if (row["cluster"].ToString() == "Produit simple")
                {
                    checkBox_ProduitSimple.Checked = true;
                    comboBox_ProduitSimple.Text = row["rawmat"].ToString();
                }
                else if (row["cluster"].ToString() == "Extra")
                {
                    checkBox_Extra.Checked = true;
                    comboBox_Extra.Text = row["rawmat"].ToString();
                }
                else if (row["cluster"].ToString() == "Produit simple")
                {
                    checkBox_ProduitSimple.Checked = true;
                    comboBox_ProduitSimple.Text = row["rawmat"].ToString();
                }
                else
                {
                    checkBox_OtherRawMat.Checked = true;
                    dataGridView_OtherRawMat.Rows.Add(row["quantity"].ToString(), row["rawmat"].ToString());
                }
            }

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            //Clear previous recipe}"
            string query = $"DELETE FROM TBL_RECETTES WHERE id_article = {label_idArticle.Text}";
            Database.ExecuteQuery(query);

            //Add the produit Simple
            if (checkBox_ProduitSimple.Checked)
            {
                string idRawMat = Database.GetID("TBL_RAWMAT", "Name", comboBox_ProduitSimple.Text);
                if (idRawMat != "0")
                {
                    query = $"INSERT INTO TBL_RECETTES (id_article, id_rawmat, quantity, date_add,date_update) VALUES ({label_idArticle.Text}, {idRawMat}, 1, GETDATE(),GETDATE())";
                    Database.ExecuteQuery(query);
                }
            }

            //Add the surface raw mat
            if (checkBox_Surface.Checked)
            {
                string idRawMat = Database.GetID("TBL_RAWMAT", "Name", comboBox_Surface.Text);
                if (idRawMat != "0")
                {
                    query = $"INSERT INTO TBL_RECETTES (id_article, id_rawmat, quantity, date_add,date_update) VALUES ({label_idArticle.Text}, {idRawMat}, 0, GETDATE(),GETDATE())";
                    Database.ExecuteQuery(query);
                }
            }

            //Add the Extra
            if (checkBox_Extra.Checked)
            {
                string idRawMat = Database.GetID("TBL_RAWMAT", "Name", comboBox_Extra.Text);
                if (idRawMat != "0")
                {
                    query = $"INSERT INTO TBL_RECETTES (id_article, id_rawmat, quantity, date_add,date_update) VALUES ({label_idArticle.Text}, {idRawMat}, 1, GETDATE(),GETDATE())";
                    Database.ExecuteQuery(query);
                }
            }

            //Add the other raw mats
            if (checkBox_OtherRawMat.Checked)
            {
                foreach (DataGridViewRow row in dataGridView_OtherRawMat.Rows)
                {
                    if (row.Cells[1].Value != null)
                    {
                        string idRawMat = Database.GetID("TBL_RAWMAT", "Name", row.Cells[1].Value.ToString());
                        if (idRawMat != "0")
                        {
                            query = $"INSERT INTO TBL_RECETTES (id_article, id_rawmat, quantity, date_add,date_update) VALUES ({label_idArticle.Text}, {idRawMat}, {row.Cells[0].Value.ToString()}, GETDATE(),GETDATE())";
                            Database.ExecuteQuery(query);
                        }
                    }
                }
            }
            this.Close();
        }

        private void checkBox_ProduitSimple_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_ProduitSimple.Enabled = checkBox_ProduitSimple.Checked;
        }

        private void checkBox_Surface_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Surface.Enabled = checkBox_Surface.Checked;
        }

        private void checkBox_OtherRawMat_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_OtherRawMat.Enabled = checkBox_OtherRawMat.Checked;
        }

        private void checkBox_Extra_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_Extra.Enabled = checkBox_Extra.Checked;
        }

        private void dataGridView_OtherRawMat_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Do nothing
        }

    }
}

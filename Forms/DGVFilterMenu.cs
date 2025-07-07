using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Mint.Forms
{
    public partial class DGVFilterMenu : Form
    {
        public List<string> selectedItems = new List<string>();
        public bool removeThisFilter = false;
        public bool removeAllFilters = false;
        public bool extractList = false;
        public string sort = "";
        public DataTable itemDT = new DataTable();
        public DGVFilterMenu()
        {
            selectedItems.Clear();
            InitializeComponent();
        }


        public void LoadList(DataTable dt)
        {
            // Effacez les éléments existants dans la CheckListBox.
            checkedListBox1.Items.Clear();

            // Ajoutez les éléments de DataTable à la CheckListBox.
            foreach (DataRow row in dt.Rows)
            {
                checkedListBox1.Items.Add(row[0].ToString(), CheckState.Checked);
            }
        }


        private void button_Cancel_Click(object sender, EventArgs e)
        {
            selectedItems.Clear();
            this.Close();
        }
        private void ClickOutsideForm(object sender, MouseEventArgs e)
        {
            if (!this.ClientRectangle.Contains(e.Location))
            {
                selectedItems.Clear();
                this.Close(); // Ferme le formulaire lorsque l'on clique en dehors de celui-ci.
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            // Récupérez les éléments cochés de la CheckListBox.
            if (checkBox_SelectAll.Checked)
            {
                removeThisFilter = true;
                selectedItems.Clear();
            }
            else
            {
                foreach (object item in checkedListBox1.CheckedItems)
                {
                    if (item.ToString() == "")
                    {
                        //TODO: improve this
                        selectedItems.Add("---blank---");
                    }
                    else
                    {
                        selectedItems.Add(item.ToString().Replace("'", "''"));
                    }

                }
            }
            this.Close();
        }

        private void checkBox_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool checkState = checkBox_SelectAll.Checked;

            checkedListBox1.SelectedIndexChanged -= checkedListBox1_SelectedIndexChanged;
            // Parcourez les éléments de la CheckedListBox.
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                // Cochez ou décochez l'élément en fonction de l'état de la case à cocher en dehors de la liste.
                checkedListBox1.SetItemChecked(i, checkState);
            }
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //uncheck the checkBox_SelectAll without triggering the event
            checkBox_SelectAll.CheckedChanged -= checkBox_SelectAll_CheckedChanged;
            checkBox_SelectAll.Checked = false;
            checkBox_SelectAll.CheckedChanged += checkBox_SelectAll_CheckedChanged;
        }

        private void RemoveAll_Click(object sender, EventArgs e)
        {
            removeAllFilters = true;
            selectedItems.Clear();
            this.Close();
        }

        private void textBox_Search_TextChanged(object sender, EventArgs e)
        {
            checkBox_SelectAll.CheckedChanged -= checkBox_SelectAll_CheckedChanged;
            checkBox_SelectAll.Checked = false;
            checkBox_SelectAll.CheckedChanged += checkBox_SelectAll_CheckedChanged;
            string searchText = textBox_Search.Text.ToLower();

            //filter itemDt based on the search text
            DataTable filteredDT = itemDT.Clone();
            foreach (DataRow row in itemDT.Rows)
            {
                if (row[0].ToString().ToLower().StartsWith(searchText))
                {
                    filteredDT.ImportRow(row);
                }
            }

            //load the filtered dt to the checkedListBox
            LoadList(filteredDT);
        }

        private void SortAsc_Click(object sender, EventArgs e)
        {
            //Sort the column by ascending order
            sort = "ASC";
            this.Close();
        }

        private void SortDesc_Click(object sender, EventArgs e)
        {
            sort = "DESC";
            this.Close();
        }

        private void ExtractList_Click(object sender, EventArgs e)
        {
            extractList = true;
            this.Close();
        }
    }
}

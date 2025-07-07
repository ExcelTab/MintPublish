using System.Data;
using static System.Windows.Forms.ListViewItem;

namespace Mint.Controls
{
    public partial class GroupListview : UserControl
    {
        List<string> outputTags = new List<string>();

        public List<string> OutputTags
        {
            get { return outputTags; }
            set { outputTags = value; }
        }
        public GroupListview()
        {
            InitializeComponent();
        }

        public void Load_Data(string header, DataTable dt)
        {
            //Changes the title
            Lbl_Header.Text = header;
            //Load the listview
            // Add columns to the ListView
            dataGridView_List.DataSource = dt;

            ExpandOrColapse();
        }

        public void Clear_Selection()
        {
            // unselects all item from the datagridview
            foreach (DataGridViewRow row in dataGridView_List.Rows)
            {
                row.Selected = false;
            }
        }

        public void Select_Data(string[] data)
        {

            // Loop through the DataGridView and check the items that are in the data array.
            // The value to check is the ID, in the first column (index 0).
            foreach (DataGridViewRow row in dataGridView_List.Rows)
            {
                if (row.Cells[0].Value != null && data.Contains(row.Cells[0].Value.ToString()))
                {
                    // If the data array contains the value in the first column, check the row.
                    row.Selected = true; // Select the row
                    row.Cells[0].Selected = false; // Deselect the cell in the first column (optional)
                }
            }
        }

        private void ExpandOrColapse()
        {
            if (Lbl_PlusMinus.Text == "+")
            {
                Lbl_PlusMinus.Text = "-";
                dataGridView_List.Visible = true;
                ////Resize the list form
                //ajust the height of the listview based on number of items
                int MaxHeight = (dataGridView_List.Rows.Count * dataGridView_List.Rows[0].Height) + dataGridView_List.ColumnHeadersHeight+10;
                int ParentHeight = this.Parent.Height;
                if (MaxHeight + Lbl_PlusMinus.Height > ParentHeight) { MaxHeight = ParentHeight - Lbl_PlusMinus.Height; }
                dataGridView_List.Height = MaxHeight;
                dataGridView_List.Top = Lbl_PlusMinus.Height;
                this.Height = Lbl_PlusMinus.Height + dataGridView_List.Height + 20;
            }
            else
            {
                Lbl_PlusMinus.Text = "+";
                dataGridView_List.Visible = false;
                this.Height = Lbl_PlusMinus.Height;
            }
        }

        private void Lbl_PlusMinus_Click(object sender, EventArgs e)
        {
            ExpandOrColapse();
        }

        private void Lbl_Header_Click(object sender, EventArgs e)
        {

        }
    }
}

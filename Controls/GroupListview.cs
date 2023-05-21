using System.Data;

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
            foreach (DataColumn column in dt.Columns)
            {
                Lvi_List.Columns.Add(column.ColumnName);
            }

            // Add rows to the ListView
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row.ItemArray.Select(x => x.ToString()).ToArray());
                Lvi_List.Items.Add(item);
            }


            ExpandOrColapse();
        }

        public void Clear_Selection()
        {
            Lvi_List.BeginUpdate();
            foreach (ListViewItem item in Lvi_List.Items)
            {
                item.Checked = false;
            }
            Lvi_List.EndUpdate();
        }

        public void Select_Data(string[] data)
        {
            Lvi_List.BeginUpdate();
            //loop through the listview and check the items that are in the data array. THe value to chec is the ID, in the first column
            foreach (ListViewItem item in Lvi_List.Items)
            {
                if (data.Contains(item.SubItems[0].Text))
                {
                    item.Checked = true;
                }
            }
            Lvi_List.EndUpdate();
        }

        private void ExpandOrColapse()
        {
            if (Lbl_PlusMinus.Text == "+")
            {
                Lbl_PlusMinus.Text = "-";
                Lvi_List.Visible = true;
                ////Resize the list form
                Lvi_List.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                //ajust the height of the listview based on number of items
                int MaxHeight = (Lvi_List.Items.Count + 1) * Lvi_List.Items[0].Bounds.Height;
                //if (MaxHeight > 350) { MaxHeight = 350; }
                Lvi_List.Height = MaxHeight;
                this.Height = Lbl_PlusMinus.Height + Lvi_List.Height + 20;
            }
            else
            {
                Lbl_PlusMinus.Text = "+";
                Lvi_List.Visible = false;
                this.Height = Lbl_PlusMinus.Height;
            }
        }

        private void Lbl_PlusMinus_Click(object sender, EventArgs e)
        {
            ExpandOrColapse();
        }

        private void Lvi_List_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item;

            if (item.Checked)
            {
                item.ForeColor = Color.White;  // Set font color to white
                item.BackColor = Color.DarkGreen;  // Set background color to dark green
                outputTags.Add(item.SubItems[0].Text); // Add the first subitem to the output_Tags list
            }
            else
            {
                item.ForeColor = Lvi_List.ForeColor;  // Set font color back to default
                item.BackColor = Lvi_List.BackColor;  // Set background color back to default
                outputTags.Remove(item.SubItems[0].Text); // Remove the first subitem to the output_Tags list
            }
        }

        private void Lvi_List_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewItem item = e.Item;

            if (item.Selected)
            {
                item.Checked = !item.Checked;  // Toggle the checked state
                item.Selected = false;
            }


        }

    }
}

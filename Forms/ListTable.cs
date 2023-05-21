using Mint.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Mint.Forms
{
    public partial class ListTable : Form
    {
        public string Text_Selected;
        public string ID_Selected;
        public int ViewMode;
        private DataTable listDataTable;

        public ListTable()
        {
            InitializeComponent();
        }

        private void Liv_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            ID_Selected = Lvi_List.SelectedItems[0].SubItems[0].Text;
            Text_Selected = Lvi_List.SelectedItems[0].SubItems[1].Text;

            this.Close();
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ID_Selected = e.Node.Tag.ToString();
            Text_Selected = e.Node.Text;

            this.Close();
        }
        public void Set_Title(string title)
        {
            Lbl_ListTitle.Text = title;
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
            DataRow[] childRows = listDataTable.Select($"id_Parent = {categoryId}");

            // Recursively populate the child nodes
            foreach (DataRow childRow in childRows)
            {
                PopulateTreeView(node, childRow);
            }
        }

        int CalculateTreeViewWidth(System.Windows.Forms.TreeView treeView)
        {
            int maxWidth = 0;

            foreach (TreeNode node in treeView.Nodes)
            {
                int nodeWidth = GetNodeWidth(node);
                if (nodeWidth > maxWidth)
                    maxWidth = nodeWidth;
            }

            return maxWidth;
        }

        int GetNodeWidth(TreeNode node)
        {
            int nodeWidth = TextRenderer.MeasureText(node.Text, node.TreeView.Font).Width;

            foreach (TreeNode childNode in node.Nodes)
            {
                int childWidth = GetNodeWidth(childNode);
                if (childWidth > nodeWidth)
                    nodeWidth = childWidth;
            }

            return nodeWidth;
        }




        public void Load_List(string query)
        {
            SqlConnection connection = new SqlConnection(Database.MainConnectionString());
            connection.Open();

            SqlCommand command = new SqlCommand(query, connection);


            switch (ViewMode)
            {
                case 1:
                    //Tree View
                    Trv_List.BringToFront();
                    // Create a data adapter to fill the category data into a DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    listDataTable = new DataTable();
                    adapter.Fill(listDataTable);


                    //TODO : pas lancer ici et dans le form, lancer le treeviewpopulate method qqpart de central
                    // Create the root node of the tree
                    TreeNode root = new TreeNode("Accueil");

                    // Get the root rows from the datatable
                    DataRow[] rootRows = listDataTable.Select("id_Parent = 2");

                    // Populate the tree view recursively
                    foreach (DataRow row in rootRows)
                    {
                        PopulateTreeView(root, row);
                    }

                    // Set the tree view's root node
                    Trv_List.Nodes.Add(root);
                    Trv_List.Nodes[0].Expand();

                    // Usage
                    int treeViewWidth = CalculateTreeViewWidth(Trv_List);
                    Trv_List.Width = treeViewWidth + 10; // Add extra padding if needed


                    if (Trv_List.Width > this.Width)
                    {
                        Trv_List.Width = this.Width;
                    }
                    else
                    {
                        this.Width = Trv_List.Width;
                    }
                    break;

                default:
                    //List View
                    SqlDataReader reader = command.ExecuteReader();
                    Lvi_List.BringToFront();
                    Lvi_List.Items.Clear();
                    Lvi_List.View = View.Details;

                    // Add column headers
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Lvi_List.Columns.Add(reader.GetName(i));
                    }

                    // Add data rows
                    while (reader.Read())
                    {
                        string[] rowData = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            rowData[i] = reader[i].ToString();
                        }
                        Lvi_List.Items.Add(new ListViewItem(rowData));
                    }

                    Lvi_List.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    connection.Close();

                    ////Resize the list form
                    Lvi_List.Width = 0;
                    foreach (ColumnHeader column in Lvi_List.Columns)
                    {
                        Lvi_List.Width += column.Width;
                    }
                    Lvi_List.Width += 30;

                    if (Lvi_List.Width > this.Width)
                    {
                        Lvi_List.Width = this.Width;
                    }
                    else
                    {
                        this.Width = Lvi_List.Width;
                    }
                    break;
            }
            Lbl_ListTitle.Width = this.Width;
        }
    }
}

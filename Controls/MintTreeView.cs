using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mint.Controls
{
    public class MintTreeView : TreeView
    {
        public MintTreeView()
        {
            DoubleBuffered = true;
            this.AfterCheck += TreeView_AfterCheck;
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                SetNodeAndParentBold(e.Node);
                e.Node.ForeColor = Color.DarkGreen;
            }
            else
            {
                SetNodeAndParentNormal(e.Node);
                e.Node.ForeColor = Color.Black;
            }
        }

        private void SetNodeAndParentBold(TreeNode node)
        {
            node.ForeColor = Color.Blue;

            if (node.Parent != null)
            {
                SetNodeAndParentBold(node.Parent);
            }
        }

        private void SetNodeAndParentNormal(TreeNode node)
        {
            node.ForeColor = Color.Black;
            bool anyChildChecked;
            if (node.Parent != null)
            {
                anyChildChecked = node.Parent.Nodes.Cast<TreeNode>().Any(n => n.Checked);
            }
            else
            {
                anyChildChecked = node.Nodes.Cast<TreeNode>().Any(n => n.Checked);
            }
            if (!anyChildChecked && node.Parent != null)
            {
                SetNodeAndParentNormal(node.Parent);
            }
        }
    }
}


using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public class TreeDrawer : Drawer
    {
        public TreeNode node;
        public TreeDrawer(TreeView treeView1, ParserLibrary.AbstrParser.UniEl newEl, ParserLibrary.AbstrParser.UniEl ancestor)
        {
            newEl.treeNode = this;// new TreeNode(newEl.Name);
            node = new TreeNode(newEl.Name);
            if (ancestor == null)
            {
                treeView1.Nodes[0].Nodes.Add(node);
                treeView1.Nodes[0].Nodes[^1].Tag = newEl;
            }
            else
            {
                (ancestor.treeNode as TreeDrawer).node.Nodes.Add(node);
                (ancestor.treeNode as TreeDrawer).node.Nodes[^1].Tag = newEl;

            }

        }
        public void Update(ParserLibrary.AbstrParser.UniEl newEl)
        {
            if (node.Nodes.Count == 0)
            {
                node.Nodes.Add(newEl.Value.ToString());
                node.Nodes[^1].Tag = this;
            }
            else
            {
                node.Nodes[0] = new TreeNode(newEl.Value.ToString());
                node.Nodes[0].Tag = this;
            }

        }
    }

    public class TreeDrawerFactory : DrawerFactory
    {
        TreeView tree;
        public TreeDrawerFactory(TreeView tree1)
        {
            AbstrParser.drawerFactory = this;
            tree = tree1;

        }
        public Drawer Create(ParserLibrary.AbstrParser.UniEl newEl, ParserLibrary.AbstrParser.UniEl ancestor)
        {
            return new TreeDrawer(tree, newEl, ancestor);
        }
    }

}

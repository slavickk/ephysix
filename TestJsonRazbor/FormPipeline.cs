using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLibrary;

namespace TestJsonRazbor
{
    public partial class FormPipeline : Form
    {
        public FormPipeline()
        {
            InitializeComponent();
        }

        private void AddNode(object sender, EventArgs e)
        {
            selectedNode.Nodes.Add(new TreeNode("_Step") { ContextMenuStrip = this.contextMenuStrip1,Tag=new Step() });
            treeView1.ExpandAll();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        TreeNode selectedNode;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
        }

        private void FormPipeline_Load(object sender, EventArgs e)
        {
            selectedNode = treeView1.Nodes[0];
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;
            ((TreeView)sender).SelectedNode = e.Node;
        }
    }
}

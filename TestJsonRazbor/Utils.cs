﻿using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniElLib;

namespace TestJsonRazbor
{
    public class TreeDrawer : Drawer
    {
        public TreeNode node;
        public TreeDrawer(TreeView treeView1, AbstrParser.UniEl newEl, AbstrParser.UniEl ancestor,bool delRoot=false,bool isPale=false)
        {
            newEl.treeNode = this;// new TreeNode(newEl.Name);
            node = new TreeNode(newEl.Name);
            if(isPale)
                node.ForeColor = System.Drawing.Color.Gray;
            else
            node.ForeColor = System.Drawing.Color.Black;
            if (ancestor == null)
            {
                if (delRoot)
                {
                    treeView1.Nodes.Add(node);
                    treeView1.Nodes[^1].Tag = newEl;

                }
                else
                {
                    treeView1.Nodes[0].Nodes.Add(node);
                    treeView1.Nodes[0].Nodes[^1].Tag = newEl;
                }
            }
            else
            {
                if (ancestor.treeNode != null)
                {
                    (ancestor.treeNode as TreeDrawer).node.Nodes.Add(node);
                    (ancestor.treeNode as TreeDrawer).node.Nodes[^1].Tag = newEl;
                    if (delRoot && newEl.childs.Count == 0)
                        Update(newEl);
                }
            }

        }
        public void Update(AbstrParser.UniEl newEl)
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
        bool delRoot;

        public bool isPale = false;

        public TreeDrawerFactory(TreeView tree1,bool delRoot1=false)
        {
            delRoot = delRoot1;
            if(!delRoot)
             AbstrParser.drawerFactory = this;
            tree = tree1;

        }
        public Drawer Create(AbstrParser.UniEl newEl, AbstrParser.UniEl ancestor)
        {
            return new TreeDrawer(tree, newEl, ancestor,delRoot,isPale);
        }
    }

}

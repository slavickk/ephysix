using Microsoft.AspNetCore.Mvc.Diagnostics;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class UserControlViewInputTree : UserControl
    {
        Step currentStep1;

        public AbstrParser.UniEl rootEl
        {
            get
            {
                return treeView1.Nodes[0]?.Nodes[0]?.Tag as AbstrParser.UniEl;
            }
        }
        public UserControlViewInputTree()
        {
       //     InitializeComponent();
        }
        public UserControlViewInputTree(Step currentStep)
        {
            Init(currentStep);
        }

        public  void Init(Step currentStep)
        {
            currentStep1 = currentStep;
            InitializeComponent();
        }

        string ReadFile(string file_name)
        {
            if (!File.Exists(file_name))
                return "";
            using (StreamReader sr = new StreamReader(file_name))
            {
                return sr.ReadToEnd();

            }
        }
        string[] getPaths(Step step, string AddPath = "")
        {
            List<string> paths = new List<string>();
            Step nextStep = step;
            do
            {
                paths.Insert(0, nextStep.IDStep);
                nextStep = step.owner.steps.FirstOrDefault(ii => ii.IDStep == nextStep.IDPreviousStep);
            } while (nextStep != null);
            if (AddPath != "")
                paths.Add(AddPath);
            return paths.ToArray();
        }
        private void UserControlViewInputTree_Load(object sender, EventArgs e)
        {
            drawFactory = new TreeDrawerFactory(treeView1);

            if (currentStep1.owner.tempMocData != "")
            {
                ParseInput(currentStep1.owner.tempMocData, new string[] { currentStep1.owner.steps.First(ii => (ii.IDPreviousStep == null || ii.IDPreviousStep == "")).IDStep }/* getPaths(currentStep1,"")*//*, new string[] { currentStep1.owner.steps.First(ii=>(ii.IDPreviousStep == null || ii.IDPreviousStep =="")).IDStep }*/);
                return;
            }

            if (currentStep1.receiver != null && (currentStep1.receiver.MocFile != null || (currentStep1.receiver.MocBody ?? "") != ""))
            {

                ParseInput(((currentStep1.receiver.MocBody ?? "") != "") ? currentStep1.receiver.MocBody : ReadFile(currentStep1.receiver.MocFile), getPaths(currentStep1, "Rec")/* new string[] { currentStep1.IDStep, "Rec" }*/);
            }

        }


        private void ParseInput(string line, string[] paths)
        {
            int ind = 0;
            //            using (StreamReader sr = new StreamReader(file_name))
            {
                AbstrParser.UniEl ancestor = null;
                AbstrParser.UniEl rootEl = null;
                if (list.Count > 0)
                    ancestor = list.FirstOrDefault(ii => ii.Name == paths[0]);
                foreach (var path in paths)
                {
                    if (ancestor == null || ancestor.Name != path)
                    {
                        if (ancestor != null)
                            rootEl = ancestor.childs.FirstOrDefault(ii => ii.Name == path);
                        if (rootEl == null)
                        {

                            // ancestor = null;
                            AbstrParser.UniEl lastRoot = null;
                            /* if (list.Count > 0)
                                 lastRoot = list.FirstOrDefault(ii => ii.ancestor == null);*/
                            rootEl = AbstrParser.CreateNode(ancestor, list, path);
                            if (lastRoot != null)
                                lastRoot.ancestor = rootEl;
                        }
                        ancestor = rootEl;
                    }
                }
                //              var line = sr.ReadToEnd();
                if (line != "")
                {
                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(line, rootEl, list))
                            break;
                }

            }
        }
        List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        TreeDrawerFactory drawFactory;


        public delegate void AfterSelect(string Text);
        public AfterSelect afterSelect;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var el = e.Node.Tag as AbstrParser.UniEl;
            if (el != null)
            {
                string text = el.path;
                /*                if (e.Node.TreeView.Name == "treeView2" && el.path.Contains("Root/"))
                                    textBox1.Text = el.path.Substring(5);
                                else
                                    textBox1.Text = el.path;*/
                Clipboard.SetText(el.path);
                if (afterSelect != null)
                    afterSelect(Text);

            }
        }
    }
}

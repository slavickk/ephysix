using CSScripting;
using Microsoft.AspNetCore.Mvc;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestJsonRazbor
{
    public partial class FormFormCounter : Form
    {
        Pipeline pip;
        public FormFormCounter(Pipeline pip)
        {
            this.pip = pip; 
            InitializeComponent();
            if(pip.steps?.Length > 0) 
                userControlViewInputTree1.Init(pip.steps.First());
            userControlFormOutputField1.Init();
        }

        private void FormFormCounter_Load(object sender, EventArgs e)
        {   
            RefreshBuildMetrics();
 
            
        }

        private void RefreshBuildMetrics()
        {
            listView1.Items.Clear();
            foreach (var build in pip.metricsBuilder)
            {
                listView1.Items.Add(new ListViewItem(new String[] { build.Name, build.Description, String.Join(',', build.labels.Select(ii => ii.Name)) }) );
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLabelsList();
        }

        private void RefreshLabelsList()
        {
            listBox1.Items.Clear();
            if (listView1.Items.Count > 0 && listView1.SelectedIndices.Count>0)
            {
                int index = listView1.SelectedIndices[0];
                MetricBuilder metricBuilder = pip.metricsBuilder[index];
                listBox1.Items.AddRange(metricBuilder.labels.ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pip.metricsBuilder.Add(new MetricBuilder() { Name = textBoxNameMetric.Text, Description = textBoxDetailMetric.Text });
            RefreshBuildMetrics();
        }
        int getCurrentMetricIndex()
        {
            if (listView1.Items.Count > 0 && listView1.SelectedIndices.Count>0)
                return listView1.SelectedIndices[0];
            else
                return -1;

            }
            private void buttonAddLabel_Click(object sender, EventArgs e)
        {
            int index = getCurrentMetricIndex();
            if(index>=0)
            {
                pip.metricsBuilder[index].labels.Add(new MetricBuilder.Label() { Name = textBoxNameLabel.Text, Value = userControlFormOutputField1.outValue });
                RefreshLabelsList();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndices.Count > 0)
            {
                int index = getCurrentMetricIndex();
                if (index >= 0)
                {

                    int index1 = listBox1.SelectedIndices[0];
                    if(index1>=0)
                    {
                        var label = pip.metricsBuilder[index].labels[index1];
                        textBoxNameLabel.Text=label.Name;
                        userControlFormOutputField1.outValue = label.Value;
                    }
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear(); 
            foreach(var mm in pip.metricsBuilder)
            {
                mm.Init();
                await mm.Fill(userControlViewInputTree1.rootEl);
                listBox2.Items.Add(mm.PrometheusString);
            }
        }

        private void buttonEditMetric_Click(object sender, EventArgs e)
        {

        }

        private void buttonEditLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

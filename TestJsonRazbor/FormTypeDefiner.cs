using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLibrary;

namespace TestJsonRazbor
{
    public partial class FormTypeDefiner : Form
    {

        public Type tDefine;
        public object tObject;
        public FormTypeDefiner()
        {
            InitializeComponent();
        }

        private void FormTypeDefiner_Load(object sender, EventArgs e)
        {
            foreach(var t in Assembly.GetAssembly(typeof(Pipeline)).GetTypes().Where(ii => ii.IsAssignableTo(tDefine) && !ii.IsAbstract))
            {
                comboBox1.Items.Add(t);
            }
            this.Text = "Configure " + tDefine.Name;
            if(tObject != null)
            {
                comboBox1.SelectedIndex=comboBox1.Items.IndexOf(tObject.GetType());
                if(comboBox1.SelectedIndex >= 0)
                    Refresh_tObjectProp();
            }
        }


        List<System.Windows.Forms.Label> labels = new List<Label>();
        List<System.Windows.Forms.TextBox> textBoxes = new List<TextBox>();
       // List<Button> buttons = new List<Button>();
        List<FieldInfo> fields = new List<FieldInfo>();

        void AddControlGroup(ref int lastTop,FieldInfo fld)
        {

            int iCount = 1;
            if (fld.FieldType.IsArray)
                iCount = (fld.GetValue(tObject) as Array).Length;
            for (int i = 0; i < iCount; i++)
            {
                Label label = new Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(19,lastTop);
                label.Name = "label" + fld.Name;
                label.Size = new System.Drawing.Size(78, 32);
                label.TabIndex = 0;
                label.Text = fld.Name;
                this.Controls.Add(label);
                labels.Add(label);
                TextBox textBox = new TextBox();
                textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                textBox.Location = new System.Drawing.Point(379, lastTop);
                textBox.Name = "textBox" + fld.Name;
                textBox.Size = new System.Drawing.Size(597, 39);
                textBox.TabIndex = 1;
                if(fld.FieldType.IsArray)
                    textBox.Text = (fld.GetValue(tObject) as Array).GetValue(i) .ToString();
                else
                    textBox.Text = fld.GetValue(tObject)?.ToString();
                textBoxes.Add(textBox);
                this.Controls.Add(textBox);
                fields.Add(fld);
                lastTop += (60);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             if(comboBox1.SelectedIndex>=0)
            {
                tObject = Activator.CreateInstance(comboBox1.SelectedItem as Type);
                Refresh_tObjectProp();
            }
        }

        private void Refresh_tObjectProp()
        {
            foreach (var label in labels)
                this.Controls.Remove(label);
            foreach (var box in textBoxes)
                this.Controls.Remove(box);
            textBoxes.Clear();
            labels.Clear();
            fields.Clear();
            int lastTop = 87;
            Type t = (Type)comboBox1.Items[comboBox1.SelectedIndex];
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var flds = t.GetFields();
            foreach (var fld in flds.Where(ii => !ii.FieldType.IsClass || ii.FieldType.IsArray || ii.FieldType ==typeof(string)))
            {
                if (fld.FieldType != typeof(bool))
                {
                    AddControlGroup(ref lastTop, fld);
                }
            }
            if (lastTop == 19)
                lastTop = 150;
            this.Size = new Size(this.Size.Width, lastTop + 70);
        }

        object conv(string text,FieldInfo fld)
        {

            if (fld.FieldType == typeof(double) || fld.FieldType == typeof(double[]))
                return Convert.ToDouble(text);
            if (fld.FieldType == typeof(int) || fld.FieldType == typeof(int[]))
                return Convert.ToInt32(text);
            return text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0; i < textBoxes.Count;i++)
            {
                if (fields[i].FieldType.IsArray)
                {
                    // Stub
                    (fields[i].GetValue(tObject) as Array).SetValue(conv(textBoxes[i].Text, fields[i]), 0) ;
//                    fields[i].SetValue(tObject, conv(textBoxes[i].Text, fields[i]));
                } else
                    fields[i].SetValue(tObject, conv(textBoxes[i].Text, fields[i]));
            }
        }
    }
}

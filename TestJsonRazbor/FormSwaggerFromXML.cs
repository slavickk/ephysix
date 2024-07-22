using ParserLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static ParserLibrary.SwaggerDef;
using static ParserLibrary.SwaggerDef.Components.Schemas.Item;
using static ParserLibrary.SwaggerDef.GET.Responses;
using static ParserLibrary.SwaggerDef.GET.Responses.CodeRet.Content.Schema;

namespace TestJsonRazbor
{
    public partial class FormSwaggerFromXML : Form
    {
        public FormSwaggerFromXML()
        {
            InitializeComponent();
        }

        string calcPath(XmlNode node, XmlNode parentNode)
        {
            var nodeName = node.Name;
            if (node.GetType() == typeof(XmlAttribute))
                nodeName = "@" + ((XmlAttribute)node).Name;
            var retValue = (node.GetType() != typeof(XmlAttribute) && node.ParentNode.Name == "#document") ? nodeName : ("/" + nodeName);
            if (node.GetType() == typeof(XmlAttribute))
            {
                retValue = calcPath(parentNode, parentNode.ParentNode) + retValue;

            }
            else
            {
                if (node.ParentNode.Name != "#document")
                    retValue = calcPath(node.ParentNode, node.ParentNode.ParentNode) + retValue;
            }
            return retValue;
        }
        void fromXmlToList(XmlNode node, XmlNode parentNode, List<(string key, string comment)> list, Dictionary<string, string> defs)
        {
            var path = calcPath(node, parentNode);
            var ff = defs.FirstOrDefault(ii => path.Length - ii.Key.Length > 0 && path.Substring(path.Length - ii.Key.Length) == ii.Key);
            var comment = "";
            if (!string.IsNullOrEmpty(ff.Value))
                comment = ff.Value;
            list.Add((path, comment));
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.GetType() != typeof(XmlText))

                    fromXmlToList(node2, node, list, defs);
            }
            if (node.Attributes != null)
            {
                foreach (XmlNode node2 in node.Attributes/*.ChildNodes*/)
                {
                    fromXmlToList(node2, node, list, defs);
                }
            }

        }
        private void FormSwaggerFromXML_Load(object sender, EventArgs e)
        {
            //    var nname = "RPUPAY";
            FillListView("R" + textBoxXMLName.Text, listViewInput, true);
            FillListView("A" + textBoxXMLName.Text, listViewOutput, false);

        }

        private void FillListView(string nname, ListView listViewInput, bool withPars)
        {
            string path = $"C:\\БГС\\{nname}.json";

            XmlDocument xml = new XmlDocument();
            xml.Load($"C:\\БГС\\{nname}.xml");
            List<(string, string)> list = new List<(string, string)>();
            using (StreamReader sr = new StreamReader(path))
            {
                var def = JsonSerializer.Deserialize<Dictionary<string, string>>(sr.ReadToEnd());
                fromXmlToList(xml.DocumentElement, null, list, def);

            }
            listViewInput.Items.Clear();
            foreach (var item in list)
            {
                if (withPars)
                    listViewInput.Items.Add(new ListViewItem(new string[] { item.Item1, item.Item2, item.Item1.Split("/").Last().Replace("@", ""), "Y" }));
                else
                    listViewInput.Items.Add(new ListViewItem(new string[] { item.Item1, item.Item2, item.Item1.Split("/").Last().Replace("@", "") }));

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewInput.SelectedItems)
            {
                if (item.ForeColor != Color.Red)
                    item.ForeColor = Color.Red;
                else
                    item.ForeColor = Color.Black;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewOutput.SelectedItems)
            {
                if (item.ForeColor != Color.Red)
                    item.ForeColor = Color.Red;
                else
                    item.ForeColor = Color.Black;
            }


        }

        private void listViewInput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       private void buttonFormJson_Click(object sender, EventArgs e)
        {
            var reqProperties = new List<OpenApiDef.EntryPoint.InputParameter>();
            var respProperties = new List<OpenApiDef.EntryPoint.Parameter>();

            OpenApiDef def = new OpenApiDef()
            {
                 info = new Info() { title="Test", version="0.1.1", description="Descr"}
                 , tags = new List<Tag>()
                 {
                     new Tag() { name="INFO", description="infos"}
                 }
                 , paths = new List<OpenApiDef.EntryPoint>()
                 {
                      new OpenApiDef.EntryPoint()
                      {
                           tags= new List<string>() {"INFO"}
                           , description="Meth1"
                           , method= OpenApiDef.EntryPoint.Method.post
                           , path="/Api1"
                           , summary="Summary"
                           , inputs= reqProperties,
                           outputs=respProperties
                      },
                                  new OpenApiDef.EntryPoint()
                      {
                           tags= new List<string>() {"INFO"}
                           , description="Meth2"
                           , method= OpenApiDef.EntryPoint.Method.get
                           , path="/api/Method2"
                           , summary="Summary"
                           , inputs= reqProperties,
                           outputs=respProperties
                      }

                 }
            };

            foreach (ListViewItem item in listViewInput.Items)
            {
                if (item.ForeColor == Color.Red)
                    reqProperties.Add(new OpenApiDef.EntryPoint.InputParameter() {path= item.SubItems[0].Text, name= item.SubItems[2].Text,
                     description = item.SubItems[1].Text, type = "string" });
            }
            foreach (ListViewItem item in listViewOutput.Items)
            {
                if (item.ForeColor == Color.Red)
                    respProperties.Add(new OpenApiDef.EntryPoint.Parameter()
                    {
                        
                        path= item.SubItems[0].Text,
                        name = item.SubItems[2].Text,
                        description = item.SubItems[1].Text,
                        type = "string"
                    });
            }
            OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(((IEnumerable<OpenApiDef.EntryPoint.Parameter>)reqProperties).ToList() );

            OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(respProperties);
            using (StreamWriter sw = new StreamWriter($"C:\\БГС\\swagger.json"))
            {
                sw.Write(def.jsonBody);
            }
        }
    }
}

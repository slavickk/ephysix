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
using PlantUml.Net;
using static System.Net.Mime.MediaTypeNames;
using Plugins;
using UniElLib;
using CSScripting;
using static System.Windows.Forms.DataFormats;
using ParserLibrary.PlantUmlGen;
using System.Net.WebSockets;
using System.Text.RegularExpressions;

namespace TestJsonRazbor
{
    public partial class FormSwaggerFromXML : Form
    {
        string plantBody = @"@startuml
partition ""Запрос справочников ""{
#lightblue :Запрос справочника провайдеров ANYWAY(AWRPROVIDER)  <color:red><i> Готово</i>|
detach
note right
  Вызов этих справочников кэшируется
  и занимает миллисекунды  
  Это следует учитывать
  при проектировании вызовов
end note
#lightgreen :Запрос идентификатора провайдера<
#lightblue :Запрос справочника вендоров ANYWAY(AWREXPORT)|
}

detach
group Выполнение платежа
partition ""Заполнение параметров платежа ""{
#lightgreen :Запрос идентификатора провайдера<

if(AWAEXPORT.IsSupportRequestRSTEP = true) then (многоходовый)
  :Выбор  конкретного получателя платежа;
  #lightgreen :Заполнение секции Search<
#lightblue :Запрос ANYWAY AWRSTEP  Step=""0"" ,секция Search|
:Назначение currentStep=0;
repeat
#lightgreen :Заполнение реквизитов платежа\n, секция INPUTS предыдущего ответа AWASTEP<
#lightblue :Запрос ANYWAY AWRSTEP  Step=currentStep |
:currentStep=currentStep+1;
repeat while(FinalStep !=true) 

else (одноходовый)
repeat
#lightgreen :Заполнение реквизитов платежа\n, секция ADDITIONAL ( по справочнику вендоров)<
#lightblue :Запрос ANYWAY AWRPUPAY  Check=true|
repeat while(Status!=""Executed"") 
endif

}
partition ""Финализация платежа ""{
}
:Выбор  конкретного получателя платежа;
end group
detach
:СОСТОЯНИЕ;
:ВЫЗОВ ПРОЦЕДУРЫ|
:ВВОД<
note right
  
  //несколько строчек//
  и может содержать
  в себе <b>HTML</b> теги
  и creole синтаксис
  ====
  * Вызов метода """"foo()"""" запрещен
end note
:ВЫВОД>
:СОХРАНЕНИЕ/
:ЗАДАЧА]
:РЕШЕНИЕ}
detach
:Некоторое действие;
(A)
detach
(A)
:Следующее действие;
switch ( число потоков )
case ( равно 1 )
  :1 поток;
case ( равно 2 ) 
  :2 потока;
case ( равно 3 )
  :3 потока;
case ( равно 4 )
  :4 потока
  ----
  срочное
  завершение
  kill;
  kill
case ( равно 5 )
  :5 потоков;
endswitch
:BBB;
end
@enduml";
        async Task GeneratePlant()
        {
            var factory = new RendererFactory();


            var renderer = factory.CreateRenderer(new PlantUmlSettings());

            var bytes = await renderer.RenderAsync(plantBody, OutputFormat.Png);
            File.WriteAllBytes("C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\out.png", bytes);
        }
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
        void fromXmlToList(XmlNode node, XmlNode parentNode, List<OpenApiDef.SenderPartDefinition.DefinitionItem> list, List<OpenApiDef.SenderPartDefinition.DefinitionItem> definitionItems)
        {
            if(node.Name=="ADDITIONAL")
            {
                int yy = 0;
            }
            var path = calcPath(node, parentNode);
            var clearPath= string.Join('/', path.Split('/').Select(ii => ii.Substring(ii.IndexOf(':') + 1)));
            var ff = definitionItems.FirstOrDefault(ii => clearPath.Length - ii.clearPath.Length >= 0 && clearPath.Substring(clearPath.Length - ii.clearPath.Length) == ii.clearPath);
            //    var comment = "";
            if (ff != null)
            {
                ff.key = ff.path;
                ff.path = path;
                //          comment = ff.description;
                list.Add(ff);
                AddNamespaces(node, ff,ref path);
                ff.path = path;
                var currNode = parentNode;
                while(currNode!= null)
                {
                    AddNamespaces(currNode, null,ref path);
                    ff.path = path;

                    currNode = currNode.ParentNode;
                }

                var ff1 = list.FirstOrDefault(ii => ii.path == ff.path);
                if (ff1 != null)
                {
                    if(ff1.path.Contains("@Direction"))
                    {
                        int yy = 0;
                    }
                    ff1.description = ff.description;
                }
            }
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.GetType() != typeof(XmlText))

                    fromXmlToList(node2, node, list, definitionItems);
            }
            if (node.Attributes != null)
            {
                foreach (XmlNode node2 in node.Attributes/*.ChildNodes*/)
                {
                    fromXmlToList(node2, node, list, definitionItems);
                }
            }

        }

        static string RepString(string input, string oldString, string newString)
        {
            string pattern = $@"(^|/)({Regex.Escape(oldString)})($|/)";

            /*foreach (var input in inputs)
            {
                // Use Regex to replace occurrences of the variable with "pref:variable"
                string result = Regex.Replace(input, pattern, m => m.Value.Replace(toReplace, "pref:" + toReplace));

                string pattern = Regex.Escape(oldString); // Escape special characters
              */                                        // Use Regex to replace occurrences of the variable with "pref:variable"
            return Regex.Replace(input, pattern, m => m.Value.Replace(oldString, newString));
        }
        private static void AddNamespaces(XmlNode node, OpenApiDef.SenderPartDefinition.DefinitionItem ff,ref string path)
        {
            if (!string.IsNullOrEmpty(node.NamespaceURI))
            {
                string prefix;
                if (XmlParser.namespaces.TryGetValue(node.NamespaceURI, out prefix))
                {
                    if (string.IsNullOrEmpty(prefix))
                    {
                        prefix = node.NamespaceURI.Substring(node.NamespaceURI.LastIndexOf('/') + 1);
                        XmlParser.namespaces[node.NamespaceURI] = prefix;
                        path = RepString(path, node.Name, prefix + ":" + node.Name);
                        //path = path.Replace("/" + node.Name + "/", "/" + prefix + ":" + node.Name + "/");
                    }
                    else
                    {
                        if (node.Prefix != prefix)
                        {
                            if (string.IsNullOrEmpty(node.Prefix))
                                path = RepString(path, node.Name, prefix + ":" + node.Name);
//                            path = path.Replace("/" + node.Name, "/"+prefix + ":" + node.Name+"/");
                            else
                                path = RepString(path, node.Prefix + ":" + node.Name, prefix + ":" + node.Name);
//                            path = path.Replace("/" + node.Prefix + ":" + node.Name + "/", "/" + prefix + ":" + node.Name + "/");

                        }
                    }
                    /*if (node.Prefix != prefix && !string.IsNullOrEmpty(node.Prefix))
                    {
                        ff.path = ff.key = ff.path.Replace(node.Prefix + ":", (!string.IsNullOrEmpty(prefix) ? prefix + ":", ""));
                    }*/
                }
                else
                {
                    XmlParser.namespaces.TryAdd(node.NamespaceURI, node.Prefix);
                    if(string.IsNullOrEmpty(node.Prefix))
                    {
                        prefix = node.NamespaceURI.Substring(node.NamespaceURI.LastIndexOf('/') + 1);
                        XmlParser.namespaces[node.NamespaceURI] = prefix;
                                                path = RepString(path,  node.Name, prefix + ":" + node.Name);
//                           path = path.Replace("/" + node.Name + "/", "/" + prefix + ":" + node.Name + "/");
                        node.Prefix = prefix;
                    }
                }
                if (ff != null)
                {
                    ff.xml_prefix = node.Prefix;
                    ff.xml_namespace = node.NamespaceURI;
                }
            }
        }

        string summaryJsonFilePath = Path.Combine(Environment.GetEnvironmentVariable("DATA_ROOT_DIR"),"swaggerSummary.json");
        private void FormSwaggerFromXML_Load(object sender, EventArgs e)
        {
            //  await GeneratePlant();
            if (File.Exists(summaryJsonFilePath))
            {
                using (StreamReader sr = new StreamReader(summaryJsonFilePath))
                {
                    def = System.Text.Json.JsonSerializer.Deserialize<OpenApiDef>(sr.ReadToEnd());
                }
                foreach (var path in def.paths)
                {
                    OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(((IEnumerable<OpenApiDef.EntryPoint.Parameter>)path.inputs).ToList());

                    OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(path.outputs);

                }
            }
            else
                def = new OpenApiDef()
                {
                    info = new Info() { title = "Test", version = "0.1.1", description = "Descr" }
                      ,
                    tags = new List<Tag>()
                 {
                     new Tag() { name="INFO", description="infos"}
                 }
                      ,
                    paths = new List<OpenApiDef.EntryPoint>()

                };
            comboBoxAllPaths.Items.Clear();
            foreach (var path in def.paths)
            {
                comboBoxAllPaths.Items.Add(path.path);
            }
            List<OpenApiDef.EntryPoint.ExternalItem> allProviders = null;
            using (StreamReader sr = new StreamReader("C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\AllProviders.json"))
            {
                allProviders = JsonSerializer.Deserialize<List<OpenApiDef.EntryPoint.ExternalItem>>(sr.ReadToEnd());
            }
            comboBoxCommand.Items.Clear();
            /*  List<string> list = new List<string>();

              foreach (var filePath in Directory.GetFiles("C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\", "*.xml"))
              {
                  var path = Path.GetFileNameWithoutExtension(filePath).Substring(1);
                  if (!list.Contains(path))
                      list.Add(path);
              }*/
            comboBoxCommand.Items.AddRange(allProviders.ToArray());
            //    var nname = "RPUPAY";
            if (def != null)
                listBoxTags.Items.AddRange(def.tags.Select(t => t.name).ToArray());
            comboBoxMethod.Items.AddRange(Enum.GetNames<OpenApiDef.EntryPoint.Method>());
        }


       
        private void FillListView(string nname, ListView listViewInput, bool withPars, bool add, string sourceName,List<OpenApiDef.EntryPoint.Parameter> inputs=null)
        {
            List<OpenApiDef.SenderPartDefinition.DefinitionItem> list = new List<OpenApiDef.SenderPartDefinition.DefinitionItem>();
            if (inputs?.Count > 0)
            {
                list.AddRange(inputs.Select(ii => new OpenApiDef.SenderPartDefinition.DefinitionItem() { newName = ii.name, description = ii.description, example = ii.example, format = ii.format, key = ii.name, path = ii.path, old_path=ii.path, repeatable = ii.repeateable, required = ii.required, type = ii.type }));

            }

            //        if (inputs == null || inputs.Count == 0)
            {
                List<OpenApiDef.SenderPartDefinition.DefinitionItem> list1 = new List<OpenApiDef.SenderPartDefinition.DefinitionItem>();
                string path = $"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.json";
               // List<OpenApiDef.SenderPartDefinition.DefinitionItem> list = new List<OpenApiDef.SenderPartDefinition.DefinitionItem>();
                OpenApiDef.SenderPartDefinition def;
                using (StreamReader sr = new StreamReader(path))
                {
                    def = JsonSerializer.Deserialize<OpenApiDef.SenderPartDefinition>(sr.ReadToEnd());

                }
                foreach (var itemList in list)
                {
                    var found = def.definitionItems.FirstOrDefault(ii => ii.path == itemList.path);
                    if (itemList.path.Contains("@Direction"))
                    {
                        int yy = 0;
                    }
                    if (found != null)
                        itemList.description = found.description;
                }

                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.Load($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\{nname}.xml");
                    fromXmlToList(xml.DocumentElement, null, list1, list);
                    foreach(var inp in inputs)
                    {
                        inp.path=list.First(ii=>ii.old_path==inp.path).path;
                    }
                    fromXmlToList(xml.DocumentElement, null, list1, def.definitionItems);

                }
                catch (Exception ex)
                {
                    //  var ff = def.definitionItems.FirstOrDefault(ii => path.Length - ii.path.Length > 0 && path.Substring(path.Length - ii.path.Length) == ii.path);

                    list1 = def.definitionItems.ToList();
                }
    
                list.AddRange(list1.Where(ii => list.Count(i1 => i1.path == ii.path) == 0));

            }
            /*  if (inputs?.Count > 0)
              {
                  list.AddRange(inputs.Where(ii=>list.Count(i1=> i1.path==ii.path) ==0).Select(ii=> new OpenApiDef.SenderPartDefinition.DefinitionItem() {newName=ii.name, description=ii.description, example=ii.example, format=ii.format, key=ii.name, path=ii.path, repeatable=ii.repeateable, required=ii.required,type=ii.type } ));

              }*/
            RedrawListInput(listViewInput, withPars, list, add, sourceName);
        }

        private static void RedrawListInput(ListView listViewInput, bool withPars, List<OpenApiDef.SenderPartDefinition.DefinitionItem> list, bool add, string sourceName)
        {
            if (!add)
                listViewInput.Items.Clear();
            foreach (var item in list)
            {
                if (withPars)
                    listViewInput.Items.Add(new ListViewItem(new string[] { item.path, item.description, string.IsNullOrEmpty(item.newName)?item.path.Split("/").Last().Replace("@", ""):item.newName, getType(item), "", item.example, "N", item.required ? "Y" : "N", item.repeatable? "Y" : "N", sourceName }));
                else
                    listViewInput.Items.Add(new ListViewItem(new string[] { item.path, item.description, string.IsNullOrEmpty(item.newName) ? item.path.Split("/").Last().Replace("@", "") : item.newName, getType(item), "", item.example, item.repeatable.ToString(), sourceName }));

            }
        }

        private static string getType(OpenApiDef.SenderPartDefinition.DefinitionItem item)
        {
            return (item.type != null) ? item.type.Replace("xsd:", "").ToLower() : "string";
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

        ListViewItem oldInputItem = null;
        ListViewItem oldOutputItem = null;
        private void listViewInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldInputItem != null)
            {
                oldInputItem.SubItems[1].Text = textBoxDescrReq.Text;
                oldInputItem.SubItems[2].Text = textBoxNewNameInput.Text;
                oldInputItem.SubItems[3].Text = textBoxInputType.Text;
                oldInputItem.SubItems[4].Text = textBoxInputFormat.Text;
                oldInputItem.SubItems[5].Text = textBoxInputExample.Text;
                oldInputItem.SubItems[7].Text = checkBoxInputRequired.Checked ? "Y" : "N";
                oldInputItem.SubItems[8].Text =checkBoxRepeatable.Checked ? "Y" : "N";
            }
            if (listViewInput.SelectedIndices.Count > 0)
            {

                oldInputItem = listViewInput.SelectedItems[listViewInput.SelectedIndices.Count - 1];
                textBoxDescrReq.Text = oldInputItem.SubItems[1].Text;
                textBoxNewNameInput.Text = oldInputItem.SubItems[2].Text;

                textBoxInputType.Text = oldInputItem.SubItems[3].Text;
                textBoxInputFormat.Text = oldInputItem.SubItems[4].Text;
                textBoxInputExample.Text = oldInputItem.SubItems[5].Text;
                checkBoxInputRequired.Checked = oldInputItem.SubItems[7].Text == "Y";
                checkBoxRepeatable.Checked = oldInputItem.SubItems[8].Text == "Y";
            }
        }
        OpenApiDef def = null;
        /*static string calcPath(XmlNode node, XmlNode parentNode)
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
        }*/

        List<string> getAllPaths(XmlNode node, XmlNode parentNode, List<string> list)
        {

            var path = calcPath(node, parentNode);
            if (!list.Contains(path))
                list.Add(path);
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.GetType() != typeof(XmlText))

                    getAllPaths(node2, node, list);
            }
            if (node.Attributes != null)
            {
                foreach (XmlNode node2 in node.Attributes/*.ChildNodes*/)
                {
                    getAllPaths(node2, node, list);
                }
            }
            return list;
        }
        async Task<string> AddFooterInDescription(OpenApiDef.EntryPoint entry)
        {
            var retValue = "\r\n\r\n\r\n<h><i>[Диаграмма переходов](/Files/" + entry.path.Substring(entry.path.LastIndexOf('/') + 1) + ".html)             \r\n";
            if (!string.IsNullOrEmpty(entry.exampleDoc))
            {
                await GenExample.generateExample(entry.exampleDoc);
                retValue += "        [Пример использования](/Files/" + entry.exampleDoc.Replace(".json", ".html")+")       \r\n";
            }
            return retValue;
       //     retValue += "        [Документация AnyWay](/Files/doc.pdf#" + entry.path.Substring(entry.path.LastIndexOf('/') + 1) + "_bookmark)";
        }

        string clarifyTypeField(string inputType, out string outputFormat)
        {
            outputFormat = "";
            switch (inputType)
            {
                case "integer":
                case "boolean":
                case "string":
                    return inputType;
                case "decimal":
                    return "number";
                case "datetime":
                    outputFormat = "date-time";
                    return "string";
                default:
                    return "string";
                    //           break;
            }
        }
        private async void buttonFormJson_Click(object sender, EventArgs e)
        {
            XmlParser.isCorrectedNamespace = true;
            try
            {
                var reqProperties = new List<OpenApiDef.EntryPoint.InputParameter>();
                var respProperties = new List<OpenApiDef.EntryPoint.Parameter>();
                if (def == null)
                    def = new OpenApiDef()
                    {
                        info = new Info() { title = "Test", version = "0.1.1", description = "Descr" }
                         ,
                        tags = new List<Tag>()
                 {
                     new Tag() { name="INFO", description="infos"}
                 }
                         ,
                        paths = new List<OpenApiDef.EntryPoint>()

                    };
                if (currentEntryPoint == null)
                {
                    currentEntryPoint = new OpenApiDef.EntryPoint();
                }
                if (currentEntryPoint.externalCallItems == null)
                    currentEntryPoint.externalCallItems = new List<OpenApiDef.EntryPoint.ExternalItem>();
                currentEntryPoint.inputs = reqProperties;
                currentEntryPoint.outputs = respProperties;
                currentEntryPoint.path = textBoxAPIPath.Text;
                currentEntryPoint.description = textBoxDescription.Text;
                int currIndex = currentEntryPoint.description.IndexOf("<h><i>[Диаграмма переходов]");
                if (currIndex >= 0)
                    currentEntryPoint.description = currentEntryPoint.description.Substring(0, currIndex);
                //   if (!currentEntryPoint.description.Contains("[Документация AnyWay]"))
                currentEntryPoint.description += await AddFooterInDescription(currentEntryPoint);
                currentEntryPoint.summary = textBoxSummary.Text;
                currentEntryPoint.tags = new List<string>();
                foreach (var item in listBoxTags.Items)
                    currentEntryPoint.tags.Add(item.ToString());
                currentEntryPoint.method = Enum.Parse<OpenApiDef.EntryPoint.Method>(comboBoxMethod.SelectedItem.ToString());
                currentEntryPoint.operationId = Enum.GetName<OpenApiDef.EntryPoint.Method>(currentEntryPoint.method) + currentEntryPoint.path.Split("/").Last();          // comboBoxMethod.SelectedItem = Enum.GetName<OpenApiDef.EntryPoint.Method>(currentEntryPoint.method);
                if (comboBoxCommand.SelectedItem != null && (currentEntryPoint.externalCallItems == null || currentEntryPoint.externalCallItems.FirstOrDefault(ii => ii.Name == ((comboBoxCommand.SelectedItem as OpenApiDef.EntryPoint.ExternalItem)?.Name)) == null))
                {
                    if (currentEntryPoint.externalCallItems == null)
                        currentEntryPoint.externalCallItems = new List<OpenApiDef.EntryPoint.ExternalItem>();
                    currentEntryPoint.externalCallItems.Add((comboBoxCommand.SelectedItem as OpenApiDef.EntryPoint.ExternalItem));
                }
                foreach (ListViewItem item in listViewInput.Items)
                {
                    string format = "";
                    if (item.ForeColor == Color.Red)
                        reqProperties.Add(new OpenApiDef.EntryPoint.InputParameter()
                        {
                            path = item.SubItems[0].Text,
                            name = item.SubItems[2].Text,

                            description = item.SubItems[1].Text,
                            type = clarifyTypeField(string.IsNullOrEmpty(item.SubItems[3].Text) ? "string" : item.SubItems[3].Text, out format),
                            format = string.IsNullOrEmpty(format) ? item.SubItems[4].Text : format,
                            example = item.SubItems[5].Text,
                            required = item.SubItems[7].Text == "Y",
                            repeateable = item.SubItems[8].Text == "Y"
                            ,
                            externalItemName = item.SubItems[9].Text

                        });
                }
                FormList(respProperties); 
                foreach (var itemName in currentEntryPoint.externalCallItems)
                {
                    string fileNameInput = itemName.examplePathInput;// @"C:\Users\jurag\Downloads\Telegram Desktop\AWRProvider.xml";
                    string fileNameOutput = itemName.examplePathOutput;// @"C:\Users\jurag\Downloads\Telegram Desktop\RespProviders.xml";
                    foreach (var item in def.getExamplesFromXML(fileNameInput, reqProperties.Where(ii => ii.externalItemName == itemName.Name).Select(ii => ii.path).ToList()))
                    {
                        reqProperties.First(ii => ii.path == item.Key).example = item.Value;
                    }

                    foreach (var item in def.getExamplesFromXML(fileNameOutput, respProperties.Where(ii => ii.externalItemName == itemName.Name).Select(ii => ii.path).ToList()))
                    {
                        respProperties.First(ii => ii.path == item.Key).example = item.Value;
                    }
                }

                OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(((IEnumerable<OpenApiDef.EntryPoint.Parameter>)reqProperties).ToList());

                OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(respProperties);
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                if (def.paths.Count(ii => ii.path == currentEntryPoint.path) > 0)
                    def.paths.RemoveAll(ii => ii.path == currentEntryPoint.path);
                def.paths.Add(currentEntryPoint);
                // return System.Text.Json.JsonSerializer.Serialize<SwaggerDef>(def, options);
                if (File.Exists(summaryJsonFilePath) && new FileInfo(summaryJsonFilePath).Length > 0)
                    File.Copy(summaryJsonFilePath, summaryJsonFilePath.Replace(".json", ".bak"), true);
                string swaggerJsonPathShab = "{#DATA_ROOT_DIR#}wwwroot\\swagger\\v1\\swagger.json";
                string swaggerJsonPath = Environment.GetEnvironmentVariable("DATA_ROOT_DIR") + "wwwroot\\swagger\\v1\\swagger.json";
                try
                {
                    using (StreamWriter sw = new StreamWriter(summaryJsonFilePath))
                    {
                        var line = System.Text.Json.JsonSerializer.Serialize<OpenApiDef>(def, options);
                        sw.Write(line);
                    }

                    using (StreamWriter sw = new StreamWriter(swaggerJsonPath, false, Encoding.UTF8))
                    {
                        sw.Write(def.jsonBody);
                    }
                }
                catch (Exception c)
                {
                    if (File.Exists(summaryJsonFilePath.Replace(".json", ".bak")))
                        File.Copy(summaryJsonFilePath.Replace(".json", ".bak"), summaryJsonFilePath, true);
                    MessageBox.Show(c.ToString());
                }

                if (checkBoxSavePipeline.Checked)
                {
                    def.SavePipeline(Path.Combine(Environment.GetEnvironmentVariable("DATA_ROOT_DIR"),@"Pipeline\patt1.yml"), @"C:\Users\jurag\source\repos\ms-payment-service\RawDocs \patt.yml",reqProperties, respProperties, swaggerJsonPathShab,getOutputCollect(),checkBoxOnlyCurrent.Checked?currentEntryPoint:null);
                    await PlantUMLUrl.GenerateHtml(Path.Combine(Environment.GetEnvironmentVariable("DATA_ROOT_DIR"), "PlantUML"), Path.Combine(Environment.GetEnvironmentVariable("DATA_ROOT_DIR"), "wwwroot/files"), def);
                }

            }
            catch (Exception e77)
            {
                MessageBox.Show(e77.ToString());
            }

        }

        private List<OpenApiDef.EntryPoint.Parameter> FormList(List<OpenApiDef.EntryPoint.Parameter> respProperties)
        {
           // List<OpenApiDef.EntryPoint.Parameter> respProperties = new List<OpenApiDef.EntryPoint.Parameter>();
            foreach (ListViewItem item in listViewOutput.Items)
            {
                string format = "";
                if (item.ForeColor == Color.Red)
                    respProperties.Add(new OpenApiDef.EntryPoint.Parameter()
                    {

                        path = item.SubItems[0].Text,
                        name = item.SubItems[2].Text,
                        description = item.SubItems[1].Text,

                        type = clarifyTypeField(string.IsNullOrEmpty(item.SubItems[3].Text) ? "string" : item.SubItems[3].Text, out format),
                        format = string.IsNullOrEmpty(format) ? item.SubItems[4].Text : format,
                        example = item.SubItems[5].Text,
                        repeateable = Convert.ToBoolean(item.SubItems[6].Text)
                        ,
                        externalItemName = item.SubItems[7].Text
                    });
            }
            return respProperties;
        }

  
        private List<OutputValue> getOutputCollect(/*Step step0,*/ )
        {
            List<OutputValue> outputCollect0 = new List<OutputValue>();
            foreach (ListViewItem item in listViewInput.Items)
            {
                if (item.ForeColor != Color.Red)
                {
                    bool repeated = item.SubItems[8].Text == "Y";
                    if (!string.IsNullOrEmpty(item.SubItems[5].Text))
                        outputCollect0.Add(new ConstantValue()
                        {
                            alwaysInArray = repeated,
                            isExported = true,
                            typeConvert = ConstantValue.TypeObject.String,
                            outputPath = item.SubItems[0].Text                            ,
                            isUniqOutputPath = !repeated,
                            returnOnlyFirstRow = !repeated,
                            Value = item.SubItems[5].Text
                        });
                }
            }
            return outputCollect0;
        }

        OpenApiDef.EntryPoint currentEntryPoint = null;
        private void comboBoxCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text = (comboBoxCommand.SelectedItem as OpenApiDef.EntryPoint.ExternalItem).Name.ToString();
            FillListView("R" + text, listViewInput, true, checkBoxAddToExist.Checked, text);
            FillListView("A" + text, listViewOutput, false, checkBoxAddToExist.Checked, text);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void listViewOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldOutputItem != null)
            {
                oldOutputItem.SubItems[3].Text = textBoxOutputType.Text;
                oldOutputItem.SubItems[4].Text = textBoxOutputFormat.Text;
                oldOutputItem.SubItems[5].Text = textBoxOutputExample.Text;
            }
            if (listViewOutput.SelectedIndices.Count > 0)
            {
                oldOutputItem = listViewOutput.SelectedItems[listViewOutput.SelectedIndices.Count - 1];
                textBoxOutputType.Text = oldOutputItem.SubItems[3].Text;
                textBoxOutputFormat.Text = oldOutputItem.SubItems[4].Text;
                textBoxOutputExample.Text = oldOutputItem.SubItems[5].Text;
            }

        }

        private void buttonAddRequest_Click(object sender, EventArgs e)
        {
            listViewInput.Items.Add(new ListViewItem(new string[] { "", "***new***", "", "", "", "", "", "" }));
        }

        private void textBoxAPIPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAllPaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAllPaths.SelectedIndex >= 0)
                currentEntryPoint = def.paths.FirstOrDefault(ii => ii.path == comboBoxAllPaths.Items[comboBoxAllPaths.SelectedIndex].ToString());
            else
                currentEntryPoint = null;
            if (currentEntryPoint == null)
            {
                currentEntryPoint = new OpenApiDef.EntryPoint();
                currentEntryPoint.externalCallItems = new List<OpenApiDef.EntryPoint.ExternalItem>();// new List<string>(); ;
                def.paths.Add(currentEntryPoint);
            }
            bool init = false;
            foreach (var item in currentEntryPoint.externalCallItems)
            {
                FillListView("R" + item.Name, listViewInput, true, init, item.Name, ((IEnumerable<OpenApiDef.EntryPoint.Parameter>)currentEntryPoint.inputs).ToList());
                FillListView("A" + item.Name, listViewOutput, false, init, item.Name,currentEntryPoint.outputs);
                init = true;
            }
            textBoxAPIPath.Text = currentEntryPoint.path;
            textBoxDescription.Text = currentEntryPoint.description;
            textBoxSummary.Text = currentEntryPoint.summary;
            listBoxTags.Items.Clear();
            listBoxTags.Items.AddRange(currentEntryPoint.tags.ToArray());
            comboBoxMethod.SelectedItem = Enum.GetName<OpenApiDef.EntryPoint.Method>(currentEntryPoint.method);

            foreach (var it in currentEntryPoint.inputs)
            {
                foreach (ListViewItem item in this.listViewInput.Items)
                {
                    if (it.path == item.SubItems[0].Text)
                    {
                        item.ForeColor = Color.Red;
                        item.SubItems[3].Text = it.type;
                        item.SubItems[4].Text = it.format;
                        item.SubItems[5].Text = it.example;
                        item.SubItems[6].Text = it.required ? "Y" : "N";
                    }
                }
            }

            foreach (var it in currentEntryPoint.outputs)
            {
                foreach (ListViewItem item in this.listViewOutput.Items)
                {
                    if (it.path == item.SubItems[0].Text)
                    {
                        item.SubItems[3].Text = it.type;
                        item.SubItems[4].Text = it.format;
                        item.SubItems[5].Text = it.example;

                        item.ForeColor = Color.Red;
                    }
                }
            }   
            //textBoxAPIPath.Text = text;


        }

        private void buttonCheckContent_Click(object sender, EventArgs e)
        {
            var current = (comboBoxCommand.SelectedItem as OpenApiDef.EntryPoint.ExternalItem);
            if (current != null && !string.IsNullOrEmpty(current.examplePathInput) && !string.IsNullOrEmpty(current.examplePathOutput))
            {
                MyExtensions.compare2XML($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\R{current.Name}.xml", current.examplePathInput);
                MyExtensions.compare2XML($"C:\\Users\\jurag\\source\\repos\\ms-payment-service\\RawDocs\\A{current.Name}.xml", current.examplePathOutput);
            }
        }

        private void listViewInput_KeyDown(object sender, KeyEventArgs e)
        {
            HandleLastViewKeys(sender, e);

        }

        private static void HandleLastViewKeys(object sender, KeyEventArgs e)
        {
            if (!(sender is ListView)) return;
            if (e.KeyCode == Keys.Delete)
            {
                List<int> indexes = new List<int>();
                foreach (int item in (sender as ListView).SelectedIndices)
                    indexes.Add(item);
                indexes = indexes.OrderByDescending(i => i).ToList();
                foreach (var ind in indexes)
                    (sender as ListView).Items.RemoveAt(ind);

            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                var builder = new StringBuilder();
                foreach (ListViewItem item in (sender as ListView).SelectedItems)
                {
                    foreach (ListViewItem.ListViewSubItem itemSub in item.SubItems)
                        builder.AppendLine((string.IsNullOrEmpty(itemSub.Text) ? " " : itemSub.Text) + "###");
                    builder.AppendLine("@@@");
                }
                Clipboard.SetText(builder.ToString());
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                var builder = new StringBuilder();
                var clip = Clipboard.GetText();
                int yy = 0;
                foreach (var item in clip.Split("@@@"))
                {
                    var subItems = item.Split("###").ToList();
                    subItems = subItems.Select(ii => ii.Replace("\r\n", "")).Where(ii1 => ii1.Length > 0).ToList();
                    if (subItems.Count > 2)
                    {
                        subItems[subItems.Count - 1] = "";

                        int index = 6;
                        if ((sender as ListView).Columns.Count > subItems.Count)
                        {
                            subItems.Insert(index, "N");
                            subItems.Insert(index, "N");
                        }
                        if ((sender as ListView).Columns.Count < subItems.Count)
                        {
                            subItems.RemoveAt(index);
                            subItems.RemoveAt(index);
                        }
                            (sender as ListView).Items.Add(new ListViewItem(subItems.Select(ii => ii.Trim()).ToArray()));
                    }

                }
            }
        }

        private void listViewOutput_KeyDown(object sender, KeyEventArgs e)
        {
            HandleLastViewKeys(sender, e);

        }
    }
}

using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParserLibrary.SwaggerDef.GET.Responses;
using static ParserLibrary.SwaggerDef.GET.Responses.CodeRet.Content;
using static ParserLibrary.SwaggerDef.GET.Responses.CodeRet.Content.Schema;
using static ParserLibrary.SwaggerDef.Components.Schemas.Item;
using static ParserLibrary.SwaggerDef;
using System.Text.Encodings.Web;
using CSScripting;
using System.Net.Http.Headers;
using Plugins;
using System.Drawing;
using System.IO;
using UniElLib;
using System.Xml;
using static ParserLibrary.RecordExtractor;
using System.Text.RegularExpressions;

namespace ParserLibrary
{
    public class SwaggerDef
    {

        public class GET
        {
            public class Responses
            {
                public class CodeRet
                {
                    public class Content
                    {
                        public class Schema
                        {
                            public class Items
                            {
                                [System.Text.Json.Serialization.JsonPropertyName("$ref")]
                                public string @ref { get; set; }
                                public string type { get; set; }
                                public Items items { get; set; }
                                public string format { get; set; }
                                public string description { get; set; }

                                public string example { get; set; }

                            }

                            public string type { get; set; }
                            public object @default { get; set; }
                            public Items items { get; set; }
                            public string format { get; set; }

                            [System.Text.Json.Serialization.JsonPropertyName("$ref")]
                            public string @ref { get; set; }
                        }
                        public class It
                        {
                            public Schema schema { get; set; }
                            public JsonElement? example { get; set; }    
                        }

                        public Dictionary<string,It> schemas { get; set; }
                        
                    }

                    public string description { get; set; }
                    public string operationId { get; set; }
                    public Dictionary<string, SwaggerDef.GET.Responses.CodeRet.Content.It> content { get; set; }
                    public bool? required { get; set; }
                }
                public Dictionary<string, CodeRet> responses { get; set; }//Deprecated????
 
            }
            public class Parameter
            {
                public string name { get; set; }
                public string @in { get; set; }
                public string description { get; set; }
                public bool required { get; set; }
                public SwaggerDef.Components.Schemas.Item schema { get; set; }
            }

            public List<string> tags { get; set; }
            public string summary { get; set; }
            public string description { get; set; }
            public string operationId { get; set; }
            public List<Parameter> parameters { get; set; }
            public CodeRet requestBody { get; set; }

            public Dictionary<string, CodeRet> responses { get; set; }
            public List<Security> security { get; set; }


            public class Security
            {
                public List<string> oauth2 { get; set; }

              //  public List<Dictionary<string, List<string>>> items { get; set; }
            }

            public bool deprecated { get; set; }
        }

        public string openapi { get; set; } = "3.0.1";

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Contact
        {
            public string email { get; set; }
        }

        public class ExternalDocs
        {
            public string description { get; set; } = "Find out more about Swagger";
            public string url { get; set; } = "http://swagger.io";
        }

        public class Info
        {
            public string title { get; set; }
            public string description { get; set; }
            public string termsOfService { get; set; }
            public Contact contact { get; set; }
            public License license { get; set; }
            public string version { get; set; }
        }

        public class License
        {
            public string name { get; set; }
            public string url { get; set; }
        }

 
        public class Server
        {
            public string url { get; set; }
        }

        public class Tag
        {
            public string name { get; set; }
            public string description { get; set; }
            public ExternalDocs externalDocs { get; set; }
        }
        public Info info { get; set; }
        public ExternalDocs externalDocs { get; set; }
        public List<Server> servers { get; set; }
        public List<Tag> tags { get; set; }



        public Dictionary<string,Dictionary<string, GET>> paths { get; set; }

        public class Components
        {
            public class Schemas
            {
                public class Item
                {
                    public class ItemProp
                    {
                        public string type { get; set; }
                        public Items items { get; set; }
                        public bool nullable { get; set; }
                    }

                  /*  public class Item1
                    {
                        public string type { get; set; }
                        public string description { get; set; }
                        public string example { get; set; }
                        public string format { get; set; }

                    }*/
                    public string type { get; set; }
                    public Item items { get; set; }
                    public List<string> required { get; set; }
                    public Dictionary<string,Item> properties  { get; set; }
                    public string description { get; set; }
                    public string example { get; set; }
                    public string format { get; set; }


                    //   public bool additionalProperties { get; set; }
                }
                public Dictionary<string,Item> items { get; set; }
            }

           // public Schemas schemas { get; set; }
            public Dictionary<string,Schemas.Item> schemas { get; set; }

        }

        public Components components { get; set; }
    }

    public class OpenApiDef
    {


        public string OpenApiVersion { get; set; } = "3.0.1";
        public Info info { get; set; } = new Info();
        public ExternalDocs externalDocs { get; set; }
        public List<Server> servers { get; set; }
        public List<Tag> tags { get; set; }

        public class EntryPoint
        {
            public class Parameter
            {
                public override string ToString()
                {
                    return $"{name}:{path}";
                }
                public string name { get; set; }
                public string xml_namespace { get; set; } 
                public string type { get; set; }
                public string format { get; set; }
                public string example { get; set; }
                public string description { get; set; }
                public bool required { get; set; }
                public bool repeateable { get; set; }=false;
                public string externalItemName { get; set; }

                public string path {get;set;}
                public string output_path { get; set; }

                public Parameter ancestor;
                public List<Parameter> childs= new List<Parameter>();
                public static void MarkChildsAndAncestor(List<Parameter> list)
                {
                    foreach(var item in list)
                    {
                       // item.ancestor = null;
                        item.childs.Clear();
                        string[] arr;
                        if(string.IsNullOrEmpty(item.output_path))
                            arr=item.path.Split("/");
                        else
                            arr = item.output_path.Split("/");
                        if (string.IsNullOrEmpty(item.output_path))
                        {
                            foreach (var item2 in list)
                            {
                                if (item2.path != item.path)
                                {
                                    var arr2 = item2.path.Split("/");
                                    if (arr.Length == arr2.Length - 1 && arr.SequenceEqual(arr2.Take(arr.Length)))
                                    {
                                        item.childs.Add(item2);
                                        item2.ancestor = item;
                                    }



                                }

                            }
                        }
                        else
                        {
                            foreach (var item2 in list)
                            {
                                if (item2.output_path != item.output_path)
                                {
                                    var arr2 = item2.output_path.Split("/");
                                    if (arr.Length == arr2.Length - 1 && arr.SequenceEqual(arr2.Take(arr.Length)))
                                    {
                                        item.childs.Add(item2);
                                        item2.ancestor = item;
                                    }



                                }

                            }
                        }
                        item.output_path = getFullName(item, "");
                    }

                }
            }
            public class InputParameter : Parameter
            {
                public bool inUrl { get; set; }
            }
            public string path { get; set; }
            public enum Method { get, post, put, delete };
            public Method method { get; set; }
            public string description { get; set; }
            public string[] exampleDoc { get; set; }
            public string operationId {  get; set; }
            public string summary { get; set; }

            public string exampleJsonInput { get; set; }
            public string exampleJsonOutput { get; set; }
            public PlantUMLItem PlantUMLTemplate { get; set; }  
            public List<string> tags { get; set; }= new List<string>();

            public List<InputParameter> inputs { get; set; }=new List<InputParameter>();
            public List<Parameter> outputs { get; set; }=new List<Parameter>(){ };


            //****XML parameters
            public class ExternalItem
            {
                public string provider { get; set; }

                public string Name { get; set; }
                public string examplePathInput { get; set; }
                public string examplePathOutput { get; set; }
                public override string ToString()
                {
                    return $"{Name}:{provider}";
                }
            }
                public List<ExternalItem> externalCallItems { get; set; }
            public string externalPath { get; set; }
            //****XML parameters

        }
        public List<EntryPoint.Parameter> errorParams { get; set; }

        public List<EntryPoint> paths { get; set; }= new List<EntryPoint>(){ };


        string form_schemas_item_name(EntryPoint item,string suffics)
        {
            return $"{item.path.Split("/").Last().Replace("/", "")}_{suffics}";
        }
        public string jsonBody
        {
            get
            {
                OpenApiDef.EntryPoint.Parameter.MarkChildsAndAncestor(this.errorParams);
                SwaggerDef def = new SwaggerDef();
                def.paths = new Dictionary<string, Dictionary<string, GET>>();
                def.info = this.info;
                def.externalDocs = this.externalDocs;
                var scemas_items = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
                FormScemasVars(scemas_items, null, errorParams, "Error_description", new Dictionary<string, object>());
                foreach (var item in paths)
                {
                    Dictionary<string,object> example = new Dictionary<string,object>();
                    example.Add("SwaggerMethod", item.path.Substring(item.path.LastIndexOf('/')));
                    List<GET.Parameter> parameters = item.inputs.Where(ii => ii.inUrl || item.method == EntryPoint.Method.get).Select(ii => new GET.Parameter() { @in = "query", schema = FormScemasItem(null,ii,example)/* new SwaggerDef.Components.Schemas.Item() { type = ii.type }*/, description = ii.description, name = ii.name, required = ii.required }).ToList();
                    var example1 = example;
                    if(item.inputs.Count(ii => !ii.inUrl && item.method != EntryPoint.Method.get)>0)
                    {
                        example1 = new Dictionary<string, object>();
                        example.Add("body", example1);
                    }
                    Dictionary<string, CodeRet.Content.It> dict = new Dictionary<string, CodeRet.Content.It>();
                    foreach (var item1 in item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get))
                    {
                        dict.Add(item1.name, new CodeRet.Content.It());
                    }
                    var reqBodyCond = item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get);
                    FormScemasVars(scemas_items, item, reqBodyCond, "Req",example1);
                    //777
                    //item.exampleJsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(example);
                    example.Clear();
                    FormScemasVars(scemas_items, item, item.outputs, "Resp", example);
                    item.exampleJsonOutput = Newtonsoft.Json.JsonConvert.SerializeObject(example);

                    CodeRet requestBody = null;
                    if(item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get).Count() != 0)
                    requestBody = new CodeRet() { description = "Request body", required = true, content = new Dictionary<string, SwaggerDef.GET.Responses.CodeRet.Content.It>()
                              {
                                  {"application/json" , new SwaggerDef.GET.Responses.CodeRet.Content.It() { example=System.Text.Json.JsonSerializer.Deserialize<JsonElement>(item.exampleJsonInput).GetProperty("body"), schema= new SwaggerDef.GET.Responses.CodeRet.Content.Schema(){ @ref=$"#/components/schemas/{form_schemas_item_name(item, "Req")}"} }}
                              } 
                    };
                    
                    GET Get = new GET()
                    {
                        requestBody = requestBody,
                        operationId=item.operationId,
                        description = item.description,
                        summary = item.summary,
                        tags = item.tags,
                        parameters = parameters,
                        responses = new Dictionary<string, SwaggerDef.GET.Responses.CodeRet>()
                 {
                     {
                         "200", new SwaggerDef.GET.Responses.CodeRet()
                         {
                              description="Успешное исполнение"
                              , content=new Dictionary<string, SwaggerDef.GET.Responses.CodeRet.Content.It>()
                              {
                                  {"application/json" , new SwaggerDef.GET.Responses.CodeRet.Content.It() { schema= new SwaggerDef.GET.Responses.CodeRet.Content.Schema(){ @ref=$"#/components/schemas/{form_schemas_item_name(item, "Resp")}"} }}
                              }

                         }

                     },
                     {
                         "400", new SwaggerDef.GET.Responses.CodeRet()
                         {
                              description="Invalid input"

                         }

                     },
                            {
                         "422", new SwaggerDef.GET.Responses.CodeRet()
                         {
                              description="Validation exception"
                               , content=new Dictionary<string, SwaggerDef.GET.Responses.CodeRet.Content.It>()
                              {
                                  {"application/json" , new SwaggerDef.GET.Responses.CodeRet.Content.It() { schema= new SwaggerDef.GET.Responses.CodeRet.Content.Schema(){ @ref=$"#/components/schemas/Error_description"} }}
                              }


                         }

                     }
                        }
                    };
                    var defmeth = new Dictionary<string, GET>();
                    
                    def.paths.Add(item.path, defmeth);
                    defmeth.Add(Enum.GetName<EntryPoint.Method>(item.method), Get);
                }
                def.components = new SwaggerDef.Components()
                {
                    schemas = scemas_items
                };

                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
,
                    IgnoreNullValues = true
                };
                return System.Text.Json.JsonSerializer.Serialize<SwaggerDef>(def, options);

            }
        }

        private void FormScemasVars(Dictionary<string, Components.Schemas.Item> scemas_items, EntryPoint item, IEnumerable<EntryPoint.Parameter> cond, string suff, Dictionary<string, object> example)
        {
            var respProperties = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
            foreach (var item1 in cond.Where(ii => ii.ancestor == null))
            {
                FormScemasItem(respProperties, item1, example);
            }

            if(respProperties.Count>0)
                scemas_items.Add((item==null)?suff:form_schemas_item_name(item, suff), new SwaggerDef.Components.Schemas.Item() { type = "object", properties = respProperties });

          /*  foreach (var itemHead in cond.Where(ii => ii.childs.Count > 0))
            {
                var respT = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
                foreach (var item1 in itemHead.childs)
                {
                    respT.Add(item1.name, new SwaggerDef.Components.Schemas.Item()
                    {

                        description = item1.description,
                        type = (item1.childs.Count > 0) ? "object" : item1.type,
                        example = item1.example,
                        format = item1.format
                                           ,
//                        @ref = (item1.childs.Count > 0) ? form_schemas_item_name(item, item1.name) : ""

                    });
                }
                scemas_items.Add(form_schemas_item_name(item, itemHead.name), new SwaggerDef.Components.Schemas.Item() { type = "object", properties = respT });

            }
          */
        }

        private static SwaggerDef.Components.Schemas.Item FormScemasItem(Dictionary<string, Components.Schemas.Item> respProperties, EntryPoint.Parameter item1,Dictionary<string,object> exampleDict)
        {
            SwaggerDef.Components.Schemas.Item retValue;
            if (item1.childs.Count == 0)
            {
                try
                {
                    exampleDict.Add(item1.name, item1.example);
                }
                catch (Exception ex) 
                {
                    throw;
                }
                retValue = new SwaggerDef.Components.Schemas.Item()
                {

                    description = item1.description,
                   type =(item1.repeateable?"array":( (item1.childs.Count > 0) ? "object" : item1.type)),
                    example = item1.example,
                    format = item1.format
                    //                   , @ref= (item1.childs.Count > 0) ? form_schemas_item_name(item, item1.name):""
                };
                if(respProperties!= null)
                {
                    SwaggerDef.Components.Schemas.Item existingItem = null;
                    if (respProperties.TryGetValue(item1.name,out existingItem))
                    {
                        int i = 0;
                        item1.path.Where(b => (b == '/') && (i++ == 2));
                        //item1.name = item1.path.Substring(i).Replace("@", "").Replace("/", "_");
                     //   var newName=existingItem.Where(b => (b == '/') && (i++ == 2));
                    }
                }
                respProperties?.Add(item1.name,retValue);
            }
            else
            {
                Dictionary<string, Components.Schemas.Item> props = new Dictionary<string, Components.Schemas.Item>();
                Dictionary<string,object> childExampleDict = new Dictionary<string,object>();
                try
                {
                    if(!exampleDict.TryGetValue(item1.name,out object value))
                    exampleDict.Add(item1.name, childExampleDict);
                }
                catch (Exception ex)
                {
                    throw;
                }

                foreach (var item2 in item1.childs)
                    FormScemasItem(props, item2,childExampleDict);
                if(item1.repeateable)
                {
                    int y = 0;
                }
                retValue = new SwaggerDef.Components.Schemas.Item()
                {
                    properties = item1.repeateable ? null:props,
                    description = item1.description,
                    type = (item1.repeateable ? "array" : ((item1.childs.Count > 0) ? "object" : item1.type)),
                     items= item1.repeateable ?(new Components.Schemas.Item() { type="object", properties=props}):null
                    //                   , @ref= (item1.childs.Count > 0) ? form_schemas_item_name(item, item1.name):""
                };

                respProperties?.Add(item1.name, retValue);

            }
            return retValue;
        }

        public class SenderPartDefinition
        {
            public class DefinitionItem
            {
                public string key { get; set; }
                public string newName { get; set; }
                public string path { get; set; }
                public string out_path { get; set; }
                public string old_path { get; set; }

                public string xml_prefix { get; set; }
                public string xml_namespace { get; set; }
                public string clearPath
                {
                    get
                    {
                        return string.Join('/', path.Split('/').Select(ii => ii.Substring(ii.IndexOf(':') + 1)));
                    }
                }
                public string description { get; set; }
                public string type { get; set; }
                public string format { get; set; }
                public string example { get; set; }
                public List<string> @enum { get; set; }

                public bool repeatable { get; set; }

                public bool required { get; set; }
                public override string ToString()
                {
                    return $"path:{path} descr:{description} type:{this.type} rep:{repeatable} req:{required}";
                }

            }

            public enum TypePart { request, responce };
            public TypePart typePart { get; set; }
            public List<string> exampleParts { get; set; }
            public List<DefinitionItem> definitionItems { get; set; }= new List<DefinitionItem>();
        }

        string getFullStepPath(Step step, string init = "")
        {
            string retValue ="STEPS/"+ step.IDStep + "/" + init;
          /*  if (!string.IsNullOrEmpty(step.IDPreviousStep))
                return getFullStepPath(step.owner.steps.First(ii => ii.IDStep == step.IDPreviousStep), retValue);*/
            return retValue;
        }
        string convert(string path, string ResponseType)
        {
            if (ResponseType == "text/xml")
                return path.Replace("@", "-") + "/#text";
            return path.Replace("@", "-");
        }
        static string getFullName(OpenApiDef.EntryPoint.Parameter parameter, string init = "")
        {
            string retValue = parameter.name + ((init == "") ? "" : "/") + init;
            if (parameter.ancestor != null)
                retValue = getFullName(parameter.ancestor, retValue);
            return retValue;
        }
        public static List<AbstrParser.UniEl> parseContent(string line)
        {
            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            AbstrParser.UniEl root = new AbstrParser.UniEl();
            list.Add(root);
            root.Name = "Root";
            if (AbstrParser.getApropriateParser("aa", line, root, list, false))
                return list;
            return null;

        }

        public void SavePipeline(string newPath,string templatePath,List<OpenApiDef.EntryPoint.InputParameter> reqProperties, List<OpenApiDef.EntryPoint.Parameter> respProperties, string swaggerJsonPath, List<OutputValue> addCollect, OpenApiDef.EntryPoint currentEntryPoint = null)
        {
            //var doc = await NSwag.OpenApiDocument.FromFileAsync(swaggerJsonPath);
            /* NSwag.OpenApiParameter
             NSwag.OpenApiExample openApi= new NSwag.OpenApiExample() { }*/
            string initialPath = newPath;
            bool isNew = false;
            if (!File.Exists(initialPath))
            {
                initialPath = templatePath;
                isNew = true;
            }
            Pipeline pip = Pipeline.load(initialPath, null);
            var receiver = pip.steps.First().ireceiver as HTTPReceiverSwagger;
            receiver.swaggerSpecPath = swaggerJsonPath;
            if (receiver.paths == null)
                receiver.paths = new List<HTTPReceiver.PathItem>();
            pip.xmlNameSpaces=XmlParser.namespaces;
            List<OpenApiDef.EntryPoint> founded = new List<OpenApiDef.EntryPoint>();
            if (currentEntryPoint != null)
                founded.Add(currentEntryPoint);
            else
                founded.AddRange(this.paths);
            string prevPath = "";
            pip.allMocks = new List<string>();
            foreach (var path in founded)
            {
                var firstStep = pip.steps.First();
                // firstStep.ireceiver.
                foreach (var entry in path.externalCallItems)
                {
                    var IDStep0 = $"Step_{path.path.Substring(path.path.LastIndexOf("/") + 1)}_0_{entry.Name}";
                    var step0 = pip.steps.FirstOrDefault(ii => ii.IDStep == IDStep0);
                    bool isNewStep0=false,isNewStep1=false;
                    if (step0 == null)
                    {
                        isNewStep0 = true;
                        step0 = new Step() {  description="Прием входящего запроса",IDStep = IDStep0, IDPreviousStep = (entry == path.externalCallItems.First()) ? "Step_0" : prevPath, filterCollection = new List<Step.ItemFilter>() { new Step.ItemFilter() { Name = "filt1", condition = new ConditionFilter() { conditionPath = "STEPS/Step_0/Rec", conditionCalcer = new ComparerAlwaysTrue() } } } };
                        receiver.paths.Add(new HTTPReceiver.PathItem() { Path = path.path.Substring(path.path.LastIndexOf("/")), Step = step0.IDStep });
                        List<Step> llist = new List<Step>();
                        llist.AddRange(pip.steps);
                        llist.Add(step0);
                        pip.steps = llist.ToArray();
                        step0.owner = pip;
                        if (!string.IsNullOrEmpty(entry.examplePathInput))
                        {
                            var newSender = new HTTPSender();
                            step0.sender = newSender;
                            if (Path.GetExtension(entry.examplePathInput) == ".xml")
                                newSender.ResponseType = "text/xml";
                            else
                                newSender.ResponseType = "text/json";
                            newSender.MocFile = entry.examplePathOutput;
                            newSender.description = entry.provider;
                            pip.steps.First().ireceiver.MocBody = path.exampleJsonInput;
                            //var it = doc.Paths[path.path];
                            //  it.Values.First()..Parameters

                        }
                    }
                    if (!string.IsNullOrEmpty(entry.examplePathInput))
                    {
                        pip.steps.First().ireceiver.MocBody = path.exampleJsonInput;
                        pip.allMocks.Add(path.exampleJsonInput);
                    }
                    var IDStep1 = $"Step_{path.path.Substring(path.path.LastIndexOf("/") + 1)}_1_{entry.Name}";

                    var step1 = pip.steps.FirstOrDefault(ii => ii.IDStep == IDStep1);
                    if (step1 == null)
                    {
                        isNewStep1 = true;
                        step1 = new Step() {description="Отправка ответа на запрос", IDStep = IDStep1, IDResponsedReceiverStep = "Step_0", IDPreviousStep = IDStep0, filterCollection = new List<Step.ItemFilter>() { new Step.ItemFilter() { Name = "filt1", condition = new ConditionFilter() { conditionPath = "STEPS/Step_0/Rec", conditionCalcer = new ComparerAlwaysTrue() } } } };
                        List<Step> llist = new List<Step>();
                        llist.AddRange(pip.steps);
                        llist.Add(step1);
                        pip.steps = llist.ToArray();
                        step1.owner = pip;
                    }
                    if (isNewStep0)
                    {
                        var outputCollect0 = step0.filterCollection.First().outputFields = new List<OutputValue>();
                        Dictionary<string, string> examples = new Dictionary<string, string>();
                        foreach (var item in getExamplesFromXML(entry.examplePathInput, null))
                        {
                            examples.Add(item.Key, item.Value);
                            // listViewInput.Items.First(ii => ii.path == item.Key).example = item.Value;
                        }


                        foreach (var out1 in addCollect)
                        {
                            string example;
                            if ((out1 is ConstantValue) && examples.TryGetValue(out1.outputPath, out example))
                                (out1 as ConstantValue).Value = example;
                            out1.outputPath = convert(out1.outputPath, (step0.sender as HTTPSender).ResponseType);
                        }

                        outputCollect0.AddRange(addCollect);

                        foreach (var item in /*reqProperties*/path.inputs.Where(ii => ii.externalItemName == entry.Name))
                        {
                            outputCollect0.Add(new ExtractFromInputValue() { alwaysInArray = item.repeateable, isExported = true, isUniqOutputPath = !item.repeateable, returnOnlyFirstRow = !item.repeateable, copyChildsOnly = item.repeateable, conditionPath = "STEPS/Step_0/Rec/"+(!item.inUrl?"body/":"") + getFullName(item), outputPath = convert(item.path, (step0.sender as HTTPSender).ResponseType) });
                        }

                        using (StreamReader sr = new StreamReader(entry.examplePathInput/* @"C:\Users\jurag\Downloads\Telegram Desktop\RespProviders.xml"*/))
                        {
                            var paths = parseContent(sr.ReadToEnd()).getOutputPaths1();
                            var existingPath = outputCollect0.Where(ii => ii != null).Select(ii => ii.outputPath).ToList();
                            var dict = existingPath.Where(ii => ii.IndexOf(':') >= 0).ToDictionary(x => x.Substring(x.IndexOf(':')), x => x.Substring(0, x.IndexOf(':') - 1));
                            for(int i=0; i <outputCollect0.Count;i++)
                            {
                                var it1 = outputCollect0[i];
                                foreach( var it3 in it1.outputPath.Split('/'))
                                {
                                    int index=it3.IndexOf(":");
                                    if (index >= 0)
                                    {
                                        string value;
                                        if(dict.TryGetValue(it3.Substring(index), out value ))
                                        {
              //                              outputCollect0[i].outputPath= outputCollect0[i].outputPath.Replace()
                                        }
                                    }
                                }
                            }
                            foreach (var it in paths)
                                if (existingPath.Count(ii=>ii==it.Path) ==0 )
                                    outputCollect0.Insert(0,new ConstantValue() { outputPath = it.Path, Value = it.Value, isUniqOutputPath = false });
                            // listBox1.Items.Add(it);
                        }

                        step0.filterCollection.First().outputFields = outputCollect0.OrderBy(ii => ii.outputPath).ToList();
                    }
                    if (isNewStep1)
                    {
                        var outputCollect1 = step1.filterCollection.First().outputFields = new List<OutputValue>();
                        foreach (var item in /*respProperties*/path.outputs.Where(ii => ii.externalItemName == entry.Name))
                        {
                            if (item.name == "Operator")
                            {
                                int yy = 0;
                            }
                            outputCollect1.Add(new ExtractFromInputValue() { alwaysInArray = item.repeateable, isExported = true, isUniqOutputPath = !item.repeateable, returnOnlyFirstRow = !item.repeateable, copyChildsOnly = item.repeateable, conditionPath = getFullStepPath(step1) + "Rec/" + convert(item.path, item.repeateable ? "text/json" : "text/xml"), outputPath =/*"Step_0/Ans/"+*/ getFullName(item) });
                        }
                        step1.filterCollection.First().outputFields = outputCollect1.OrderBy(ii => ii.outputPath).ToList();
                        step1.filterCollection.First().transformOutputField();
                    }
                    prevPath = IDStep0;
                    string serviceNameInUml = "Сервис платежей";
                    //Generate PlantUMLScema
                    var last = path.PlantUMLTemplate?.getLastChild(serviceNameInUml);
                    List<PlantUMLItem.Link> list = new List<PlantUMLItem.Link>();
                    if (last != null)
                    {
                        if (step0.sender.cacheTimeInMilliseconds != 0)
                            list.Add(new PlantUMLItem.Link() { children = new PlantUMLItem() { Name = "REDIS" }, NameRq = "Запрос в кэш" + entry.Name, NameRp = "Данные из кэша" });
                        list.Add(new PlantUMLItem.Link() { children = new PlantUMLItem() { Name = step0.sender.description }, NameRq = ((step0.sender.cacheTimeInMilliseconds != 0) ? "Кеш пустой." : "") + "Запрос " + entry.Name, NameRp = "Возврат результата" });
                        var thisUml = new PlantUMLItem() { Name = serviceNameInUml, color = "#00FF00", links = list };
                        last.links = new List<PlantUMLItem.Link>() { new PlantUMLItem.Link() { children = thisUml } };
                    }
                    string pathFile = Path.Combine(Pipeline.configuration["DATA_ROOT_DIR"], "PlantUML\\");
                    using (StreamWriter sw = new StreamWriter(pathFile+$"{ path.path.Substring(path.path.LastIndexOf('/') + 1)}.puml"))
                    {
                        sw.WriteLine(PlantUMLItem.getUML(path.summary, new PlantUMLItem[] { path.PlantUMLTemplate }));
                    }
                }


            }
            pip.Save(newPath, null);
        }

        string getClearPath(string path)
        {
          //  string input = "/aa:bb/cc:dd/ff:ee";

            // Regular Expression
            string pattern = @"\/[^:]+:([^\/]+)";

            // Replace the matched groups
            string result = Regex.Replace(path, pattern, "/$1");

            pattern = @"[^:]+:([^\/]+)(?:\/[^:]+:)?([^\/]+)(?:\/[^:]+:)?([^\/]+)?";

            // Replace the matched groups
            result = Regex.Replace(path, pattern, "$1/$2/$3").Trim('/').Replace("//","/");

            return result;
         }
        public  Dictionary<string, string> getExamplesFromXML(string path, List<string> paths)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                enumerableNodes(xml.DocumentElement, null, paths, keyValuePairs, "");
            }
            catch
            {

            }
            return keyValuePairs;
        }
        void enumerableNodes(XmlNode node, XmlNode parentNode, List<string> paths, Dictionary<string, string> keyValuePairs, string prevPath)
        {
            var nodeName = node.Name;
            if (node.GetType() == typeof(XmlAttribute))
                nodeName = "@" + ((XmlAttribute)node).Name;
            var retValue = prevPath + ((node.GetType() != typeof(XmlAttribute) && node.ParentNode.Name == "#document") ? nodeName : ("/" + nodeName));
            if ((paths == null || paths.Contains(retValue)) && !keyValuePairs.TryGetValue(retValue, out string value) /*&& !node.HasChildNodes*/)
            {
                if (node.GetType() == typeof(XmlAttribute))
                {
                    int yy = 0;
                }
                keyValuePairs.Add(retValue, node.InnerText);
            }
            if (node.GetType() == typeof(XmlAttribute))
            {
                //   enumerableNodes(parentNode, parentNode.ParentNode, paths, keyValuePairs, retValue);

            }
            else
            {
                /*  if (node.ParentNode.Name != "#document")
                      enumerableNodes(node.ParentNode, node.ParentNode.ParentNode, paths, keyValuePairs, retValue);*/
            }
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.GetType() != typeof(XmlText))
                    enumerableNodes(node2, node, paths, keyValuePairs, retValue);
                //getAllPaths(node2, node, list);
            }
            if (node.Attributes != null)
            {
                foreach (XmlNode node2 in node.Attributes/*.ChildNodes*/)
                {
                    if (node2.Name == "OperatorCode")
                    {
                        int y = 0;
                    }
                    enumerableNodes(node2, node, paths, keyValuePairs, retValue);
                }
            }
        }


    }
    public static class AnyHelper
    {
        public static List<(string Path, string Value)> getOutputPaths1(this List<AbstrParser.UniEl> list)
        {
            List<(string, string)> retValue = new List<(string, string)>();
            foreach (AbstrParser.UniEl ele in list)
            {
                var path = ele.path.Replace("Root/", "");
                if (ele.childs?.Count == 0 && retValue.Count(ii => ii.Item1 == path) == 0)
                    retValue.Add((path, ele.Value.ToString()));
            }
            return retValue;
        }
    }

}

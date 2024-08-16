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
                public string type { get; set; }
                public string format { get; set; }
                public string example { get; set; }
                public string description { get; set; }
                public bool required { get; set; }
                public bool repeateable { get; set; }=false;
                public string externalItemName { get; set; }

                public string path {get;set;}
                public Parameter ancestor;
                public List<Parameter> childs= new List<Parameter>();
                public static void MarkChildsAndAncestor(List<Parameter> list)
                {
                    foreach(var item in list)
                    {
                        var arr=item.path.Split("/");
                        foreach(var item2 in list)
                        {
                            if(item2.path != item.path)
                            {
                                var arr2=item2.path.Split("/");
                                if(arr.Length==arr2.Length-1 && arr.SequenceEqual(arr2.Take(arr.Length)))
                                {
                                    item.childs.Add(item2);
                                    item2.ancestor = item;
                                }
                                    

                                
                            }

                        }
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
            }
                public List<ExternalItem> externalCallItems { get; set; }
            public string externalPath { get; set; }
            //****XML parameters

        }
        public List<EntryPoint> paths { get; set; }= new List<EntryPoint>(){ };

        string form_schemas_item_name(EntryPoint item,string suffics)
        {
            return $"{item.path.Split("/").Last().Replace("/", "")}_{suffics}";
        }
        public string jsonBody
        {
            get
            {
                SwaggerDef def = new SwaggerDef();
                def.paths = new Dictionary<string, Dictionary<string, GET>>();
                def.info = this.info;
                def.externalDocs = this.externalDocs;
                var scemas_items = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
                foreach (var item in paths)
                {
                    Dictionary<string,object> example = new Dictionary<string,object>();
                    example.Add("Path", item.path.Substring(item.path.LastIndexOf('/')));
                    List<GET.Parameter> parameters = item.inputs.Where(ii => ii.inUrl || item.method == EntryPoint.Method.get).Select(ii => new GET.Parameter() { @in = "query", schema = FormScemasItem(null,ii,example)/* new SwaggerDef.Components.Schemas.Item() { type = ii.type }*/, description = ii.description, name = ii.name, required = ii.required }).ToList();
                    Dictionary<string, CodeRet.Content.It> dict = new Dictionary<string, CodeRet.Content.It>();
                    foreach (var item1 in item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get))
                    {
                        dict.Add(item1.name, new CodeRet.Content.It());
                    }
                    var reqBodyCond = item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get);
                    FormScemasVars(scemas_items, item, reqBodyCond, "Req",example);
                    item.exampleJsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(example);
                    example.Clear();
                    FormScemasVars(scemas_items, item, item.outputs, "Resp", example);
                    item.exampleJsonOutput = Newtonsoft.Json.JsonConvert.SerializeObject(example);

                    CodeRet requestBody = null;
                    if(item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get).Count() != 0)
                    requestBody = new CodeRet() { description = "Request body", required = true, content = new Dictionary<string, SwaggerDef.GET.Responses.CodeRet.Content.It>()
                              {
                                  {"application/json" , new SwaggerDef.GET.Responses.CodeRet.Content.It() { schema= new SwaggerDef.GET.Responses.CodeRet.Content.Schema(){ @ref=$"#/components/schemas/{form_schemas_item_name(item, "Req")}"} }}
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
                scemas_items.Add(form_schemas_item_name(item, suff), new SwaggerDef.Components.Schemas.Item() { type = "object", properties = respProperties });

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
                public string path { get; set; }
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


   

    }


}

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
                public string name { get; set; }
                public string type { get; set; }
                public string format { get; set; }
                public string example { get; set; }
                public string description { get; set; }
                public bool required { get; set; }


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
            public string summary { get; set; }
            public List<string> tags { get; set; }= new List<string>();

            public List<InputParameter> inputs { get; set; }
            public List<Parameter> outputs { get; set; }


            //****XML parameters
            public string externalName { get; set; }
            public string externalPath { get; set; }
            //****XML parameters

        }
        public List<EntryPoint> paths { get; set; }

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
                var scemas_items = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
                foreach (var item in paths)
                {
                    List<GET.Parameter> parameters = item.inputs.Where(ii => ii.inUrl || item.method == EntryPoint.Method.get).Select(ii => new GET.Parameter() { @in = "query", schema = FormScemasItem(null,ii)/* new SwaggerDef.Components.Schemas.Item() { type = ii.type }*/, description = ii.description, name = ii.name, required = ii.required }).ToList();
                    Dictionary<string, CodeRet.Content.It> dict = new Dictionary<string, CodeRet.Content.It>();

                    foreach (var item1 in item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get))
                    {
                        dict.Add(item1.name, new CodeRet.Content.It());
                    }
                    var reqBodyCond = item.inputs.Where(ii => !ii.inUrl && item.method != EntryPoint.Method.get);
                    FormScemasVars(scemas_items, item, reqBodyCond, "Req");

                    FormScemasVars(scemas_items, item, item.outputs, "Resp");
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

        private void FormScemasVars(Dictionary<string, Components.Schemas.Item> scemas_items, EntryPoint item, IEnumerable<EntryPoint.Parameter> cond, string suff)
        {
            var respProperties = new Dictionary<string, SwaggerDef.Components.Schemas.Item>();
            foreach (var item1 in cond.Where(ii => ii.ancestor == null))
            {
                FormScemasItem(respProperties, item1);
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

        private static SwaggerDef.Components.Schemas.Item FormScemasItem(Dictionary<string, Components.Schemas.Item> respProperties, EntryPoint.Parameter item1)
        {
            SwaggerDef.Components.Schemas.Item retValue;
            if (item1.childs.Count == 0)
            {
                retValue = new SwaggerDef.Components.Schemas.Item()
                {

                    description = item1.description,
                    type = (item1.childs.Count > 0) ? "object" : item1.type,
                    example = item1.example,
                    format = item1.format
                    //                   , @ref= (item1.childs.Count > 0) ? form_schemas_item_name(item, item1.name):""
                };
                respProperties?.Add(item1.name,retValue);
            }
            else
            {
                Dictionary<string, Components.Schemas.Item> props = new Dictionary<string, Components.Schemas.Item>();
                foreach (var item2 in item1.childs)
                    FormScemasItem(props, item2);
                retValue = new SwaggerDef.Components.Schemas.Item()
                {
                    properties = props,
                    description = item1.description,
                    type = (item1.childs.Count > 0) ? "object" : item1.type,
                    //                   , @ref= (item1.childs.Count > 0) ? form_schemas_item_name(item, item1.name):""
                };

                respProperties?.Add(item1.name, retValue);

            }
            return retValue;
        }
    }



   }

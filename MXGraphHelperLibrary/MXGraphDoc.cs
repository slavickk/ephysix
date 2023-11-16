﻿//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MXGraphHelperLibrary
{
  
 
 
   


   
 

  

 
    public class MXGraphDoc
    {
        public void Save(string path=@"C:\d\ex.json")
        {
            using(StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(JsonSerializer.Serialize<MXGraphDoc>(this
                    , new JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = true }));
            }
        }
        public class Box
        {
            public class Body
            {
                public class Row
                {
                    public class Column
                    {
                        public class Item
                        {

                            public void AddBoxLink(Box box,string id,int typeLink=2)
                            {
                                if (box_links == null)
                                    box_links = new List<BoxLink>();
                                box_links.Add(new BoxLink() { link = new BoxLink.Link() { typelink=typeLink, box_id = box.id + ":" + id } });
                            }
                            public void AddBoxLink(string box_id, string id, int typeLink = 2)
                            {
                                if (box_links == null)
                                    box_links = new List<BoxLink>();
                                box_links.Add(new BoxLink() { link = new BoxLink.Link() { typelink = typeLink, box_id = box_id + ":" + id } });
                            }
                            public class BoxLink
                            {
                                public class Link
                                {

                                    public int typelink { get; set; } = 1;
                                    public string box_id { get; set; }
                                }
                                public Link link { get; set; }
                            }


                            public string caption { get; set; }
                            public string box_id { get; set; }
                            public List<BoxLink> box_links { get; set; }
                        }

                        public JsonElement? json { get; set; } = null;
                        public Item item { get; set; }
                    }
                    public List<Column> columns { get; set; }
                }
                public List<Header> header { get; set; }
                public List<Row> rows { get; set; }
                public JsonElement? json { get; set; } = null;
            }
            public class Header
            {
                public class Size
                {
                    public int width { get; set; }
                    public int height { get; set; }
                }
                public class Position
                {
                    public int left { get; set; }
                    public int top { get; set; }
                }

                public Position position { get; set; }
                public Size size { get; set; }
                public string caption { get; set; }
                public string value { get; set; }
            }

            public string id { get; set; }
            public Header header { get; set; }

            public JsonElement? AppData { get; set; } = null;
            public Body body { get; set; }
        }

        public List<Box> boxes { get; set; }
    }


    public class JsonHelper
    {
        public class ItemLink
        {
            public string jsonPath;
            public string tablePath;
        }

        string Name;

        Dictionary<string,object> root= new Dictionary<string, object>();
        public JsonHelper(string name) 
        {
            root.Add(name , new Dictionary<string, object>());
            Name = name;
        }
        public void AddVal(string val)
        {
            var first = root[Name] as Dictionary<string,object>;
//            var firstName = Name;
            foreach(var el in val.Split("/"))
            {
                object valw;
                if (!first.TryGetValue(el, out valw))
                {
                    first.Add(el, new Dictionary<string, object>());
                    first = first[el] as Dictionary<string, object>;
                }
                else
                    first = valw as Dictionary<string, object>;
            }
        }
        //        public List<string> allIds= new List<string>();

        private Dictionary<string, object> deserializeToDictionary(string jo)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
                        var values2 = new Dictionary<string, object>();
                        foreach (KeyValuePair<string, object> d in values)
                        {
                            // if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
                            if (d.Value is JObject)
                            {
                                values2.Add(d.Key, deserializeToDictionary(d.Value.ToString()));
                            }
                            else
                            {
                                values2.Add(d.Key, d.Value);
                            }
                        }
                        return  values2 ;
            return values;
        }
        void An(string id,Dictionary<string, object> dict, List<ItemLink> allIds)
        {
            if(dict.Count==0)
            {

                dict.Add("box_id", id);
                var ids = allIds?.FirstOrDefault(ii => ii.jsonPath== id);
                if(ids != null)
                {
                    dict.Add("box_links", new Dictionary<string, object>[]{ deserializeToDictionary(@"
                    {
                      ""link"": {
                        ""typelink"": 2,
                        ""box_id"": """ + ids.tablePath + @"""
                      }
                    }
                  
")});
                    
                }
            } else
            {
                foreach(var el in dict.Keys)
                {
                    An(id+"/"+el, dict[el] as Dictionary<string,object>,allIds);
                }
            }
        }

        public string getJsonBody(List<ItemLink> allIds=null)
        {
           // allIds?.Clear();
            An(Name,root[Name] as Dictionary<string, object>,allIds);



            return JsonSerializer.Serialize<Dictionary<string,object>>(root);
        }

    }

    

}
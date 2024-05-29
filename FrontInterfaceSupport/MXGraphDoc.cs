using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MXGraphHelperLibrary
{












    public class MXGraphDoc
    {
        public void Save(string path = @"C:\d\ex.json")
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(System.Text.Json.JsonSerializer.Serialize<MXGraphDoc>(this
                    , new JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = true }));
            }
        }
        public class Box
        {
            [System.Text.Json.Serialization.JsonIgnore]
            public int xCurrent = 0;  // for arrow position control
            public string type { get; set; }
            public string category { get; set; }
            public class Body
            {
                public class Row
                {
                    public class Column
                    {
                        public string style { get; set; }
                        public List<Header> header { get; set; }
                        public List<Row> rows { get; set; }


                        public class Item
                        {
                            public bool? is_output { get;set; }
                            public bool? is_need_redraw { get; set; }
                            public void AddBoxLink(Box boxDest, string id, int typeLink = 2)
                            {
                                if (box_links == null)
                                    box_links = new List<BoxLink>();
                                box_links.Add(new BoxLink() { link = new BoxLink.Link() { typelink = typeLink, box_id = boxDest.id + ":" + id } });
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
                                    public class Points
                                    {
                                        public double x { get; set; }
                                        public double y { get; set; }
                                        public Points()
                                        {
                                            this.x = x;
                                            //    this.y = y;
                                        }
                                    }
                                    public int typelink { get; set; } = 1;
                                    public string box_id { get; set; }
                                    public Points points { get; set; } = null;
                                }
                                public Link link { get; set; }
                            }
                            public class Points
                            {
                                public int x { get; set; }
                            }


                            public string caption { get; set; }
                            public string box_id { get; set; }
                            public int? colspan { get; set; }

                            public List<int> valid_link_type { get; set; }
                            public string? style { get; set; }
                            public List<BoxLink> box_links { get; set; }
                            public Points points { get; set; }
                        }

                        public JsonElement? json { get; set; } = null;
                        public Item item { get; set; }
                    }
                    public List<Column> columns { get; set; }
                    public Dictionary<string, string>? tooltip_info { get; set; }

                }
                public List<Header> header { get; set; }
                public List<Row> rows { get; set; }
                public JsonElement? json { get; set; } = null;
                public JsonElement? appdata { get; set; } = null;

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
                public string description { get; set; }
                public string zone_name { get; set; }
                public string zone_type { get; set; }
                public string value { get; set; }
                public string style { get; set; }
            }

            public string id { get; set; }
            public Header header { get; set; }

            public JsonElement? AppData { get; set; } = null;
            public Body body { get; set; }

            public class box_link_item
            {
                public string box_id;
                public MXGraphDoc.Box.Body.Row.Column.Item.BoxLink[] boxLinks;
            }

            public List<box_link_item> enumLinks()
            {
                List<box_link_item> retValue = new List<box_link_item>();
                var rows = this.body.rows;
                enumRows(retValue, rows);
                return retValue;
            }

            private void enumRows(List<box_link_item> retValue, List<MXGraphDoc.Box.Body.Row> rows)
            {
                foreach (var row in rows)
                {
                    foreach (var col in row.columns)
                    {
                        if (col.json != null)
                            EnumerateElements((JsonElement)col.json, retValue);
                        if (col.item != null)
                        {
                            if (col.item.box_links != null)
                            {
                                retValue.Add(new box_link_item() { box_id = col.item.box_id, boxLinks = col.item.box_links.ToArray() });
                            }
                        }
                        if (col.rows != null)
                        {
                            enumRows(retValue, col.rows);

                        }
                    }
                }
            }

            private void EnumerateElements(JsonElement doc, List<box_link_item> list)
            {
                string box_id = "";
                if (doc.ValueKind == JsonValueKind.Object)
                    foreach (var property in doc.EnumerateObject())
                    {
                        var type1 = property.GetType();
                        var name = property.Name;
                        if (name == "box_id")
                        {
                            box_id = property.Value.ToString();
                            int yy = 0;
                        }
                        if (name == "box_links")
                        {
                            list.Add(new box_link_item() { box_id = box_id, boxLinks = System.Text.Json.JsonSerializer.Deserialize<MXGraphDoc.Box.Body.Row.Column.Item.BoxLink[]>(property.Value) });
                            int yy = 0;
                        }
                        var value = property.Value;
                        if (value.ValueKind == JsonValueKind.Object)
                        {
                            EnumerateElements(value, list);

                        }

                    }
                if (doc.ValueKind == JsonValueKind.Array)
                    foreach (var el in doc.EnumerateArray())
                    {
                        EnumerateElements((JsonElement)el, list);
                        //                if(value.ValueKind == JsonValueKind.Object)

                    }
            }


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

        Dictionary<string, object> root = new Dictionary<string, object>();
        public JsonHelper(string name)
        {
            root.Add(name, new Dictionary<string, object>());
            Name = name;
        }
        public void AddVal(string val)
        {
            var first = root[Name] as Dictionary<string, object>;
            //            var firstName = Name;
            foreach (var el in val.Split("/"))
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
            return values2;
            return values;
        }
        void An(string id, Dictionary<string, object> dict, List<ItemLink> allIds)
        {
            if (dict.Count == 0)
            {

                dict.Add("box_id", id);
                var ids = allIds?.FirstOrDefault(ii => ii.jsonPath == id);
                if (ids != null)
                {
                    dict.Add("box_links", new Dictionary<string, object>[] { deserializeToDictionary(@"
                    {
                      ""link"": {
                        ""typelink"": 2,
                        ""box_id"": """ + ids.tablePath + @"""
                      }
                    }
                  
") });

                }
            }
            else
            {
                foreach (var el in dict.Keys)
                {
                    An(id + "/" + el, dict[el] as Dictionary<string, object>, allIds);
                }
            }
        }

        public string getJsonBody(List<ItemLink> allIds = null)
        {
            // allIds?.Clear();
            An(Name, root[Name] as Dictionary<string, object>, allIds);



            return System.Text.Json.JsonSerializer.Serialize<Dictionary<string, object>>(root);
        }

    }



}
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
            public Body body { get; set; }
        }

        public List<Box> boxes { get; set; }
    }


    public class JsonHelper
    {
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

        void An(string id,Dictionary<string, object> dict)
        {
            if(dict.Count==0)
            {

                dict.Add("box_id", id);
            } else
            {
                foreach(var el in dict.Keys)
                {
                    An(el, dict[el] as Dictionary<string,object>);
                }
            }
        }

        public string getJsonBody()
        {
            An(Name,root[Name] as Dictionary<string, object>);



            return JsonSerializer.Serialize<Dictionary<string,object>>(root);
        }

    }

    

}
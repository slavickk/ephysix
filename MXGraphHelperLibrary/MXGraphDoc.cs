using System.Text.Json;

namespace MXGraphHelperLibrary
{
  
 
 
   


   
 

  

 
    public class MXGraphDoc
    {
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
                            public class BoxLink
                            {
                                public class Link
                                {
                                    public string box_id { get; set; }
                                }
                                public Link link { get; set; }
                            }


                            public string caption { get; set; }
                            public string box_id { get; set; }
                            public List<BoxLink> box_links { get; set; }
                        }

                        public JsonElement json { get; set; }
                        public Item item { get; set; }
                    }
                    public List<Column> columns { get; set; }
                }
                public List<Header> header { get; set; }
                public List<Row> rows { get; set; }
                public JsonElement json { get; set; }
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


    

}
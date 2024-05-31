/******************************************************************
 * File: Shablon.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GraphShablons
{
    public class Shablon
    {
        public static string getExample()
        {
            var json= new JsonObject
            {
                ["Id"] = 3,
                ["Name"] = "Bob",
                ["DOB"] = new DateTime(2001, 02, 03),
                ["Friends"] = new JsonArray
    {
        new JsonObject
        {
            ["Id"] = 2,
            ["Name"] = "Smith"
        },
        new JsonObject
        {
            ["Id"] = 4,
            ["Name"] = "Jones"
        }
    }
            };

                        var shab=new Shablon()
            {
                boxes = new List<Box> {
                new Box() {
                    id = "id1",
                    type = 1,
                    header= new Box.Header()
                    {
                        caption="a1", position= new Box.Header.Position() { left=29, top=243}, size=new Box.Header.Size() { height=300, width=200}
                    }
                    , body=new Box.Body()
                    {
                         header= new List<Box.Body.BodyHeader> { new Box.Body.BodyHeader() { value="capt" } }
                         , rows= new List<Box.Body.Row> { new Box.Body.Row()
                         {
                              columns= new List<Box.Body.Row.Column> { new Box.Body.Row.Column() {  item= new Box.Body.Row.Item(){ caption="a1", box_id="a1_link"} }
                         }
                         }

                    }
                }
            },

                                new Box() {
                    id = "id2",
                    type = 2,
                    header= new Box.Header()
                    {
                        caption="Filter example", position= new Box.Header.Position() { left=329, top=243}, size=new Box.Header.Size() { height=300, width=200}
                    }
                    , body=new Box.Body()
                    {
                         header= new List<Box.Body.BodyHeader> { new Box.Body.BodyHeader() { value="caption table" } }
                         , rows= new List<Box.Body.Row> { new Box.Body.Row()
                         {
                              columns= new List<Box.Body.Row.Column> 
                              { 
                                  new Box.Body.Row.Column() { 
                                      rows=new List<Box.Body.Row>
                                      { 
                                          new Box.Body.Row(){ columns=new List<Box.Body.Row.Column>{ new Box.Body.Row.Column() { item = new Box.Body.Row.Item() { caption = "b1", box_id="b1_link" } },new Box.Body.Row.Column() { item= new Box.Body.Row.Item() { caption="b2"   } } } }
                                          ,new Box.Body.Row(){ columns=new List<Box.Body.Row.Column>{ new Box.Body.Row.Column() { item = new Box.Body.Row.Item() { caption = "b3", box_id = "b3_link" } },new Box.Body.Row.Column() { item= new Box.Body.Row.Item() { caption="b4"   } } } }
                                      }
                                  }
                                  ,
                                  new Box.Body.Row.Column() { item=new Box.Body.Row.Item() {  caption="d1"} }
,
                                                                    new Box.Body.Row.Column() {
                                      rows=new List<Box.Body.Row>
                                      {
                                          new Box.Body.Row(){ columns=new List<Box.Body.Row.Column>{ new Box.Body.Row.Column() { item = new Box.Body.Row.Item() { caption = "b1" } },new Box.Body.Row.Column() { item= new Box.Body.Row.Item() { caption="b6", box_id="b6_link"   } } } }
                                          ,new Box.Body.Row(){ columns=new List<Box.Body.Row.Column>{ new Box.Body.Row.Column() { item = new Box.Body.Row.Item() { caption = "b3" } },new Box.Body.Row.Column() { item= new Box.Body.Row.Item() { caption="b7", box_id = "b7_link"   } } } }
                                          ,new Box.Body.Row(){ columns=new List<Box.Body.Row.Column>{ new Box.Body.Row.Column() { item = new Box.Body.Row.Item() { caption = "b0" } },new Box.Body.Row.Column() { item= new Box.Body.Row.Item() { caption="b8", box_id = "b8_link"   } } } }
                                      }
                                  }

                         }
                         }

                    }
                }
            }
                                ,

                new Box() {
                    id = "id4",
                    type = 1,
                    header= new Box.Header()
                    {
                        caption="a7", position= new Box.Header.Position() { left=629, top=243}, size=new Box.Header.Size() { height=300, width=200}
                    }
                    , body=new Box.Body()
                    {
                         header= new List<Box.Body.BodyHeader> { new Box.Body.BodyHeader() { value="capt" } }
                         , rows= new List<Box.Body.Row> { new Box.Body.Row()
                         {
                              columns= new List<Box.Body.Row.Column> { new Box.Body.Row.Column() {  item= new Box.Body.Row.Item(){ caption="a1", box_id="a1_link"} }
                         }
                         }

                    }
                }
            }



                                , new Box() {
                    id = "id3",
                    type = 1,
                    header= new Box.Header()
                    {
                        caption="json1", position= new Box.Header.Position() { left=1029, top=243}, size=new Box.Header.Size() {height = 300, width = 200}
                    }
                    , body=new Box.Body()
                    {
                         header= new List<Box.Body.BodyHeader> { new Box.Body.BodyHeader() { value="js" } }
                         , json=json
                        
                    }
                }
            }


                        };
            var options = new JsonSerializerOptions { /*WriteIndented = true,*/ DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            var retValue = JsonSerializer.Serialize<Shablon>(shab,options);
            return retValue;
        }
        public class Box
        {
            public class Link
            {
                public int typelink { get; set; }
                public string box_id { get; set; }
            }
            public class BoxLink
            {
                public Link link { get; set; }
            }
            public class Header
            {
                public class Position
                {
                    public int top { get; set; }
                    public int left { get; set; }
                }
                public class Size
                {
                    public int width { get; set; }
                    public int height { get; set; }
                }
                public string caption { get; set; }
                public Position position { get; set; }
                public Size size { get; set; }
                public string icon { get; set; }
            }
            public class Body
            {
                public class BodyHeader
                {
                    public string value { get; set; }
                }

                public class Row
                {
                    public class Item
                    {
                        public string box_id { get; set; }
                        public string caption { get; set; }
                        public List<BoxLink> box_links { get; set; }
                    }

                    public class Column
                    {
                        public Item item { get; set; }
                        //Optional
                        public List<Row> rows { get; set; }
                        //Optional
                       // public JsonObject json { get; set; }

                    }
                    public List<Column> columns { get; set; }
                }

                public List<BodyHeader> header { get; set; }
                public List<Row> rows { get; set; }
                public JsonObject json { get; set; }
            }

            public string id { get; set; }
            public int type { get; set; }
            public Header header { get; set; }
            public Body body { get; set; }
        }

        public List<Box> boxes { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

   
 
   
    

}
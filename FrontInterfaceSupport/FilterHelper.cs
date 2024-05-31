/******************************************************************
 * File: FilterHelper.cs
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

using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FrontInterfaceSupport.DBTable;

namespace FrontInterfaceSupport
{
    public class FilterHelper
    {


        public static async Task AddNewCondition(MXGraphHelperLibrary.MXGraphDoc.Box box,string caption,string boxId)
        {
            box.body.rows.First().columns.First().rows.Add(new MXGraphDoc.Box.Body.Row() {
                columns = new List<MXGraphDoc.Box.Body.Row.Column>()
                {
                    new MXGraphDoc.Box.Body.Row.Column()
                    {
                        item= new MXGraphDoc.Box.Body.Row.Column.Item()
                        {
                             caption =caption,
                             box_id=boxId
                        }
                    }
                }
            });
        }

        public static async Task AddNewField(MXGraphHelperLibrary.MXGraphDoc.Box box, string caption, string boxId)
        {
            box.body.rows[2].columns.First().rows.Add(new MXGraphDoc.Box.Body.Row()
            {
                columns = new List<MXGraphDoc.Box.Body.Row.Column>()
                {
                    new MXGraphDoc.Box.Body.Row.Column()
                    {
                        item= new MXGraphDoc.Box.Body.Row.Column.Item()
                        {
                             caption =caption,
                             box_id=boxId
                        }
                    }
                }
            });
        }

        public static  MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item getRedrawItem(string caption, string box_id,List<int> valid_link_type,int colspan=1)
        {
            return new MXGraphDoc.Box.Body.Row.Column.Item() { colspan=colspan, box_id = box_id, style= "position: relative;box-sizing: border-box;width: calc(100% - 20px);height: 30px;margin: 0px auto;border-radius: 8px;border: 1px dashed var(--grey-10);background: var(--grey-1);", caption=caption, valid_link_type=valid_link_type ,is_output=false,is_need_redraw=true  };
        }
        public class AppData
        {
            public string FilterName { get; set; }
        }


        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createFilterBox(IConfiguration conf, MXGraphDoc retDoc, string streamDefJson, MXGraphDoc.Box oldbox)
        {
            return createFilterBox(streamDefJson, retDoc);
        }


        public static async Task<string> CreateFilter(string jsonMXGrapth = "", string filterDefJson = "{\r\n  \"tableId\":550119,\r\n  \"tableExistedId\":550079,\r\n  \"conditions\":[\"OriginalTime>@timeBegin\",\"OriginalTime<@timeEnd\",\"OriginalTime is not null\"],\r\n  \"relation\":[[\"Table\",\"550119\",\"account\",\"\"],[\"Column\",\"550130\",\"branchid\",\"account\"],[\"ForeignKey\",\"2163920\",\"branchid\",\"2163595\",\"branchid\",\"550130\"],[\"Column\",\"2163595\",\"branchid\",\"branch\"],[\"ForeignKey\",\"2163944\",\"branchid\",\"2163595\",\"branchid\",\"550095\"],[\"Column\",\"550095\",\"branchid\",\"card\"],[\"Table\",\"550079\",\"card\",\"\"]]\r\n\r\n\r\n\r\n,\r\n  \"depth\":6\r\n}", IConfiguration conf = null, bool isNew = true)
        {

            if (!isNew)
                return jsonMXGrapth;
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();

            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            createFilterBox(filterDefJson, retDoc);
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
        }

        private static MXGraphDoc.Box createFilterBox(string filterDefJson, MXGraphDoc retDoc)
        {
            AppData dbTableConfig = JsonSerializer.Deserialize<AppData>(filterDefJson);

            var num = retDoc.boxes.Count + 1;
            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            retBox.id = "filter" + num;
            retBox.category = "data transformer";
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<AppData>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();
            if (retDoc.boxes.Count == 0)
            {
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
            }
            else
            {
                int delta = 15;
                int left = retDoc.boxes.Max(ii => ii.header.position.left + ii.header.size.width) + delta;
                int top = retDoc.boxes.Min(ii => ii.header.position.top + ii.header.size.height);
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
            }
            retBox.header.caption = "FilterName";
            retBox.header.description = "filter descr";

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = 300 };
            //            retBox.id = mxGraphID;
            retBox.type = "filter";
            retBox.body = new MXGraphHelperLibrary.MXGraphDoc.Box.Body();
            //            retBox.body.header = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Header>() { new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Type" } };
            retBox.body.rows = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row>();
            {
                //header of conditions
                retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row()
                {
                    columns = new List<MXGraphDoc.Box.Body.Row.Column>() {
                    new MXGraphDoc.Box.Body.Row.Column() {
                        rows= new List<MXGraphDoc.Box.Body.Row>()
                        {
                            new MXGraphDoc.Box.Body.Row()
                            {
                                columns= new List<MXGraphDoc.Box.Body.Row.Column>()
                                {
                                    new MXGraphDoc.Box.Body.Row.Column()
                                    {
                                        item= new MXGraphDoc.Box.Body.Row.Column.Item()
                                        {
                                             caption="Conditions",
                                             style="font: var(--font-h3-semibold-14);"
                                        }
                                    }

                                }
                            }
                        }

                    },
                    new MXGraphDoc.Box.Body.Row.Column() { style="width:0; border:none;"}
                }
                });
                //new conditions
                retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row()
                {
                    columns = new List<MXGraphDoc.Box.Body.Row.Column>
                    {
                        new MXGraphDoc.Box.Body.Row.Column()
                        {
                            item =getRedrawItem("New condition", "new-cond-connector", new List < int > { 3 })
                        },
                        new MXGraphDoc.Box.Body.Row.Column() { style="width:0; border:none;"}
                    }
                }
                    );
                //header of fields
                retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row()
                {
                    columns = new List<MXGraphDoc.Box.Body.Row.Column>() {
                    new MXGraphDoc.Box.Body.Row.Column() {
                        rows= new List<MXGraphDoc.Box.Body.Row>()
                        {
                            new MXGraphDoc.Box.Body.Row()
                            {
                                columns= new List<MXGraphDoc.Box.Body.Row.Column>()
                                {
                                    new MXGraphDoc.Box.Body.Row.Column()
                                    {
                                        item= new MXGraphDoc.Box.Body.Row.Column.Item()
                                        {
                                             caption="Fields",
                                             style="font: var(--font-h3-semibold-14)"
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
                });
                //new conditions
                retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row()
                {
                    columns = new List<MXGraphDoc.Box.Body.Row.Column>
                    {
                        new MXGraphDoc.Box.Body.Row.Column()
                        {
                            item =  getRedrawItem("New field", "new-field-connector", new List<int> {  3})
                        },
                        new MXGraphDoc.Box.Body.Row.Column() { style="width:0; border:none;"}
                    }
                }
                    );
            }

            /*   await AddNewCondition(retBox, "cond1", "_a_cond1");
               await AddNewField(retBox, "field1", "_a_ff1");
            */
            retDoc.boxes.Add(retBox);
            return retBox;
        }
    }
}

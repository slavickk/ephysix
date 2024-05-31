/******************************************************************
 * File: EventsHandler.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FrontInterfaceSupport.DBTable;
using static FrontInterfaceSupport.StreamHelper;

namespace FrontInterfaceSupport
{
    public class EventsHandler
    {

        public async static Task<string> handleAddLink(IConfiguration conf1, string jsonMXGrapth, string SourceBoxId, string SourceBoxLink, string DestBoxId, string DestBoxLink)
        {
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            var connectionString = StreamHelper.buildConnString(conf1);

            // string connectionString = "User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;";
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();

            var firstBox = retDoc.boxes.First(II => II.id == SourceBoxId);
            var secondBox = retDoc.boxes.First(II => II.id == DestBoxId);
            if (firstBox.type == "table" && secondBox.type == "stream")
            {
                await TableToStreamLink(conf1, SourceBoxLink, options, retDoc, firstBox, secondBox);
            }
            if (/*firstBox.type == "table" &&*/ secondBox.type == "filter")
            {
                await AllToFilterLink(conf1, SourceBoxLink, DestBoxLink, options, retDoc, firstBox, secondBox);
            }


            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }
        private static async Task AllToFilterLink(IConfiguration conf1, string SourceBoxLink, string DestBoxLink, JsonSerializerOptions options, MXGraphDoc retDoc, MXGraphDoc.Box firstBox, MXGraphDoc.Box secondBox)
        {
            (MXGraphDoc.Box.Body.Row.Column.Item item, string colname) ret;
            if (firstBox.type == "table")
                ret = getCaptionOnTable(SourceBoxLink, firstBox);
            else
                ret = getCaptionOnOther(SourceBoxLink, firstBox);
            if (DestBoxLink == "new-cond-connector")
                await FilterHelper.AddNewCondition(secondBox, ret.colname, ret.colname);
            if (DestBoxLink == "new-field-connector")
                await FilterHelper.AddNewField(secondBox, ret.colname, ret.colname);
            ret.item.AddBoxLink(secondBox, ret.colname, 3);
            secondBox.header.size.height += heigthRow;
            // return JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);

        }

        private static (MXGraphDoc.Box.Body.Row.Column.Item item, string colname) getCaptionOnTable(string SourceBoxLink, MXGraphDoc.Box firstBox)
        {
            foreach (var row in firstBox.body.rows)
            {
                var col = row.columns.FirstOrDefault(ii => ii.item?.box_id == SourceBoxLink);
                if (col != null)
                {
                    var coltype = col.item.caption;

                    return (col.item, row.columns[row.columns.IndexOf(col) - 1].item.caption);
                }
            }
            return (null, null);
        }
        private static (MXGraphDoc.Box.Body.Row.Column.Item item, string colname) getCaptionOnOther(string SourceBoxLink, MXGraphDoc.Box firstBox)
        {
            foreach (var row in firstBox.body.rows)
            {
                var col = row.columns.FirstOrDefault(ii => ii.item?.box_id == SourceBoxLink);
                if (col != null)
                {
                    var coltype = col.item.caption;

                    return (col.item, "Item");
                }
            }
            return (null, null);
        }

        private static async Task TableToStreamLink(IConfiguration conf1, string SourceBoxLink, JsonSerializerOptions options, MXGraphDoc retDoc, MXGraphDoc.Box firstBox, MXGraphDoc.Box secondBox)
        {
            foreach (var row in firstBox.body.rows)
            {
                var col = row.columns.FirstOrDefault(ii => ii.item?.box_id == SourceBoxLink);
                if (col != null)
                {
                    var coltype = col.item.caption;

                    var colname = row.columns[row.columns.IndexOf(col) - 1].item.caption;
                    var conf = JsonSerializer.Deserialize<StreamConfig>((JsonElement)secondBox.AppData);
                    var stream = await StreamHelper.GetStreamDescription(conf1, conf.StreamName);
                    if (stream != null)
                    {
                        stream.fields.Add(new StreamDescr.Item()
                        {
                            Name = colname,
                            Type = coltype switch
                            {
                                "text" => "string",
                                _ => coltype
                            }
                        });
                    }
                    await StreamHelper.saveStream(conf1, stream);
                    StreamDescr descr = await GetStreamDescription(conf1, conf.StreamName);

                    secondBox.body = StreamHelper.GetBody(descr);
                    col.item.AddBoxLink(secondBox, colname, 3);

                    // return JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);

                }
            }
        }
    }
}


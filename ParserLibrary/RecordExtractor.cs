/******************************************************************
 * File: RecordExtractor.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UniElLib;

namespace ParserLibrary
{

    [Serializable]
    public class RecordExtractor
    {
        public class OutputField
        {
            public string outputPath { get; set; }
            public OutputValue Value { get; set; }

        }

        public string nameRecord { get; set; } = "CheckRegistration";
        public string condPath { get; set; } = "Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name";
        //                                  Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
        //Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
//       [JsonInclude]
        public ComparerV condCalcer { get; set; } = new ComparerForValue("CheckRegistration");
        public FieldExtractorCond[] fields { get; set; } = new FieldExtractorCond[] {
                new FieldExtractorCond() { nameField = "Model", condPath = "Item/everything!/request/body/content/Envelope/Body/Invoke/ActionRq/Action/Params/Param/-Name", condCalcer = new ComparerForValue("DeviceInfo"), valuePath = "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/Params/Param/String/Model" }
                ,new FieldExtractorCond() { nameField = "Latitude", condPath = "Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/-Name", condCalcer = new ComparerForValue("Latitude"), valuePath = "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/#text" }
                ,new FieldExtractorCond() { nameField = "Longitude", condPath = "Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/-Name", condCalcer = new ComparerForValue("Longitude"), valuePath = "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/#text" }
                ,new FieldExtractorCond() { nameField = "Login", condPath = "Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/-Name", condCalcer = new ComparerForValue("Login"), valuePath = "item/request/body/raw/http/request/body/content/Envelope/Body/Invoke/ActionRq/Context/Pid/Field/#text" }
                ,new FieldExtractorCond() { nameField = "OriginalTime", condPath = "Item/everything!/event/start", condCalcer = null}
                ,new FieldExtractorCond() { nameField = "IPCountryCode", condPath = "Item/client_ip/IsoCountryCode", condCalcer = null}
                
//                "item/request/body/raw/event/start"
            };
        public List<ExtratedRecord> selectAllCond(List<AbstrParser.UniEl> list)
        {
            List<ExtratedRecord> retValue = new List<ExtratedRecord>();
            foreach (var item in list.Where(ii => ii.path == condPath && condCalcer.Compare(ii)))
            {
                retValue.Add(new ExtratedRecord() { nameRecord = this.nameRecord, fields = fields.Select(ii => new ExtratedRecord.FieldItem() { name = ii.nameField, value = ii.getFieldValue(item) }).ToList() });
            }
            return retValue;

        }
        [Serializable]
        public class ExtratedRecord
        {
            public string nameRecord { get; set; }
            public class FieldItem
            {
                public string name { get; set; }
                public string value { get; set; }
                public override string ToString()
                {
                    return name+":"+value;
                }
            }
            public List<FieldItem> fields { get; set; } = new List<FieldItem>();
        }
    }


    [Serializable]
    public class FieldExtractorCond
    {
        public string nameField { get; set; }
        public string condPath { get; set; }
        public ComparerV condCalcer { get; set; }
        public string valuePath { get; set; } = "";
        public override string ToString()
        {
            return nameField+";"+condPath;
        }
        public string getFieldValue(AbstrParser.UniEl rootEl)
        {
            if(nameField == "Status" || nameField == "status")
            {
                int yy = 0;
            }

            var pathOwn = rootEl.path;
            var patts1 = AbstrParser.PathBuilder(new string[] { pathOwn, condPath });


            var patts = AbstrParser.PathBuilder(new string[] { condPath, valuePath });

            var rootEl1 = getLocalRoot(patts1, 0, rootEl);


            foreach (var item in rootEl1.getAllDescentants().Where(ii => ii.path == condPath && ((condCalcer == null) ? true : condCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    item1 = getLocalRoot(patts, 0, item1);
                    foreach (var item2 in item1.getAllDescentants().Where(ii => ii.path == valuePath))
                        return item2.Value?.ToString();
                }
                else
                    return item.Value.ToString();

            }
            return "";
        }

        private AbstrParser.UniEl getLocalRoot(string[] patts, int indexF, AbstrParser.UniEl item1)
        {
            var nodes = patts[indexF].Split("/");
            var index = nodes.Length - 1;
            while (index >= 0 &&AbstrParser. isEqual(item1.Name ,nodes[index]))
            {
                item1 = item1.ancestor;
                index--;

            }

            return item1;
        }

    }
}

/******************************************************************
 * File: APIExecutor.cs
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

using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace CamundaInterface
{
        public interface _ApiExecutor
        {
            public class ItemCommand
            {

            public ExecContextItem toExecItem()
            {
                ExecContextItem retValue= new ExecContextItem();
                retValue.Command = this.Name;
                retValue.Params=this.parameters.Select(ii=>new ExecContextItem.ItemParam() {  Key=ii.name}).ToList();
                return retValue;
            }

                public string environment;
                public override string ToString()
                {
                    return Name;
                }
                public string Name;
                public class Parameter
                {
                    public string name;
                    public string fullPath;
                    public bool isDemand = false;
                    public List<string> alternatives = new List<string>();
                    
                }
                public string outputPath;

                public List<Parameter> parameters = new List<Parameter>();
                public class OutputItem
                {
                    public string path;
                    public string Name
                    {
                        get
                        {
                            int index = path.LastIndexOf('/');
                            if (index < 0)
                                return path;
                            return path.Substring( index+1);
                        }
                    }
                    public OutputItem parent = null;
                    public List<OutputItem> children = new List<OutputItem>();
                    public override string ToString()
                    {
                        return path;
                    }

                }
                public List<OutputItem> outputItems = new List<OutputItem>();
            }

            Task beginSessionAsync();
            Task<_ApiFilter> ExecAsync(ExecContextItem[] commands);
            public class ErrorItem
            {
                public string content;
                public string error;
            }
            ErrorItem getError();
            Task endSessionAsync();
            List<ItemCommand> getDefine();


        }
        public interface _ApiFilter
        {
            string[] filter(string path);

            (string name,string value)[] filterWithNames(string path);

        }

    public class ExecContextItem
    {
        public class ItemParam
        {
            public string Key { get; set; }
            public string FullAddr { get; set; }
            public object? Value { get; set; }
            public string? Variable { get; set; }

            public ItemParam()
            {

            }
            public ItemParam(string key, object value)
            {
                Key = key;
                Value = value;
            }

        }
        public ExecContextItem()
        {

        }
        public ExecContextItem(_ApiExecutor.ItemCommand command)
        {
            CommandItem = command;
        }
        [JsonIgnore]
        public _ApiExecutor.ItemCommand CommandItem;
        public string Command { get; set; }

        public List<ItemParam> Params { get; set; }
    }



}

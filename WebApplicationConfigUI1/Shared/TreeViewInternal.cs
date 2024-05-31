/******************************************************************
 * File: TreeViewInternal.cs
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

using ParserLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationConfigUI1.Shared
{
    public class TreeViewItemInternal
    {
        public TreeViewItemInternal()
        {

        }
        public TreeViewItemInternal Parent=null;
        public TreeViewItemInternal(AbstrParser.UniEl el)
        {
            this.Title = el.Name;
            if (el.Value != null)
                this.Value = el.Value.ToString();
            this.childNodes = el.childs.Select(ii => new TreeViewItemInternal(ii) {  Parent=this}).ToList();
        }
        public bool IsExpanded { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public string Title { get; set; }
        public string Value { get; set; } = null;
        public List<TreeViewItemInternal> childNodes { get; set; }


        public static TreeViewItemInternal ParseInput(string file_name, string[] paths)
        {
            {
                List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                //                var line = await resp.Content.ReadAsStream().ReadAsStringAsync();
                using (StreamReader sr = new StreamReader(file_name))
                {
                    AbstrParser.UniEl ancestor = null;
                    AbstrParser.UniEl rootEl = null;
                    if (list.Count > 0)
                        ancestor = list[0];
                    foreach (var path in paths)
                    {
                        if (ancestor == null || ancestor.Name != path)
                        {
                            if (ancestor != null)
                                rootEl = ancestor.childs.FirstOrDefault(ii => ii.Name == path);
                            if (rootEl == null)
                                rootEl = AbstrParser.CreateNode(ancestor, list, path);
                            ancestor = rootEl;
                        }
                    }
                    var line = sr.ReadToEnd();
                    if (line != "")
                    {
                        foreach (var pars in AbstrParser.availParser)
                            if (pars.canRazbor(line, rootEl, list))
                                break;
                    }
                    return new TreeViewItemInternal(list[0]);
                }
            }
        }

    }
}

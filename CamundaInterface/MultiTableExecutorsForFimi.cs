/******************************************************************
 * File: MultiTableExecutorsForFimi.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterface
{


    public class TableDefine
    {
        public class ExtID
        {
            public string Column { get; set; }
            public string Table { get; set; }
        }
        public class Column
        {
            public bool uid { get; set; } = false; 
            public long col_id { get; set; }
            public int ind { get; set; }
            public string? path { get; set; }
            public string? variable { get; set; }
            public string? constant { get; set; }

            public string Name { get; set; }
            public string Type { get; set; }
            public string TableName;
        }

        public string[] KeyColumns { get; set; }

        public bool UpdateIfExists { get; set; }
        public long table_id { get; set; }
        public string Table { get; set; }
        public List<Column> Columns { get; set; }
        public List<ExtID> ExtIDs { get; set; }
    }


    public class MultiTableExecutorsForFimi
    {

    }
}

﻿using System;
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

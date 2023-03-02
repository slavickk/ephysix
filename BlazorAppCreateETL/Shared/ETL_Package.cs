using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppCreateETL.Shared
{
    public class ETL_Package
    {
        public string ETLName { get; set; } = "";
        public string ETLDescription { get; set; }
        public int ETL_dest_id { get; set; }
        public List<string>  TableOutputName { get; set; }
        public long idPackage { get; set; } = -1;
        public string ETL_add_par { get; set; } = "";
        public class VariableItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string VariableType { get; set; }
            public string VariableDefaultValue { get; set; }

        }

        public List<VariableItem> variables { get; set; } = new List<VariableItem>();
        public class ItemTable
        {
            public List<ItemColumn>? columns { get; set; }
            public string table_name { get; set; }
            public int? src_id { get; set; }
            public bool? pci_dss_zone { get; set; }
            public string scema { get; set; }
            public long table_id { get; set; }
            public string alias { get; set; } = "";
            public long etl_id { get; set; }
            public override string ToString()
            {
                if (alias != "")
                    return $"{table_name}({alias}) -{scema}";
                else
                    return $"{table_name}-{scema}";
            }
        }
        public class ItemColumn
        {
            public string col_name { get; set; }
            public string alias { get; set; }
            public long col_id { get; set; }
            public ItemTable table { get; set; }

            public override string ToString()
            {
                return $"{col_name}:{table}";

            }
        }

        public List<ItemTable> allTables { get; set; } = new List<ItemTable>();
        public class ItemRelation
        {
            public List<string>? column1Name { get; set; }
            public List<string>? column2Name { get; set; }
            public ItemTable table1 { get; set; }
            public ItemTable table2 { get; set; }
            public long relationID { get; set; }
            public string relationName { get; set; }
            public override string ToString()
            {
                return $"{relationName}-{table1.table_name}:{table2.table_name}";
            }
        }
        public class ItemAddCondition
        {
            public ItemTable table { get; set; }
            public string condition { get; set; }
        }
        public List<ItemAddCondition> conditions { get; set; } = new List<ItemAddCondition>();

        public List<ItemRelation> relations { get; set; } = new List<ItemRelation>();

        public class ItemSelectedList
        {
            public ItemColumn sourceColumn;
            public string outputTable;
//            public string outpu
        }
        public List<ItemSelectedList> selectedFields { get; set; } = new List<ItemSelectedList>();

    }
    public class ItemPackage
    {
        public string Name { get; set; }
        public long id { get; set; }
        public override string ToString()
        {
            return $"{id}:{Name}";
        }
    }
   // Task runner;
}

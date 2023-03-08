using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_DB_Interface
{
    public class GraphvizImpl
    {
        public static string drawError()
        {
            return @"
digraph {
    subgraph cluster_2 {
    color = red;
    Error
    }
   
}";
        }
        public static string drawContent(GenerateStatement.ETL_Package package)
        {
            string addCont = "";
            string content = "digraph {\r\n    graph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\r\n    node [shape=plain]\r\n    rankdir=LR;\r\n";
            
            var src = package.tables.Select(ii => ii.src_id).ToList();
            src.Add(package.dest_id);
            List<GenerateStatement.ItemTable> addList = new List<GenerateStatement.ItemTable>();
            foreach(var tableName in package.outputTable.Split(','))
            {
                var table=new GenerateStatement.ItemTable() { Name = tableName, src_id = package.dest_id, src_name="output", columns = new List<GenerateStatement.ItemTable.ColumnItem>() };                
                foreach(var tab in package.allTables)
                    table.columns.AddRange(tab.SelectList.Where(ii=>ii.outputTable==tableName).Select(ii=>new GenerateStatement.ItemTable.ColumnItem() {  Name=ii.alias}));
                addList.Add(table);
                
            }
            package.allTables.AddRange(addList);
            foreach (var item in src.Distinct().OrderBy(ii => ii))
            {
                var tab = package.allTables.FirstOrDefault(ii => ii.src_id == item);
                string zoneName = "unknown";
                var color = "lightgray";
                if (tab != null)
                {
                    if (tab.pci_dss_zone)
                        color = "lightgreen";
                    zoneName = tab.src_name.Replace(" ", "_");
                }
                content += $"subgraph cluster_{zoneName} {{\r\n    label={zoneName};\r\n    style=filled;\r\n    color={color};\r\n";
                foreach (var table in package.tables.Where(ii => ii.src_id == item))
                {
                    content += $"{item}{table.Name} [label=<\r\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\r\n  <tr><td bgcolor=\"white\"><b><i>{table.Name}</i></b></td></tr>";
                    foreach (var col in table.columns)
                        content += $"<tr><td port=\"{col.Name}\">{col.Name}</td></tr>";
                    content += "</table>>];";
                    foreach (var opt in table.optionalRelItems)
                    {
                        addCont += $"{item}{opt.srcTable.Name}:{opt.colSrc.Name} -> {package.dest_id}{opt.colDst.OutputTable}:{opt.colDst.Name} ;\r\n";
                    }
                }
                content += "}";
            }
            foreach (var rel in package.list)
            {
                var cols1 = rel.NameColumns1.Split(',');
                var cols2 = rel.NameColumns2.Split(',');
                if (string.IsNullOrEmpty(rel.Name1Table))
                {
                    int yy = 0;
                }
                else
                {
                    for (int i = 0; i < cols1.Length; i++)
                    {
                        var st = (package.allTables.First(ii => ii.Name == rel.Name1Table).src_id == package.allTables.First(ii => ii.Name == rel.Name2Table).src_id) ? ("[constraint=false,style=\"dotted\"]") : ("");
                        addCont += $"{package.allTables.First(ii => ii.Name == rel.Name1Table).src_id}{rel.Name1Table}:{cols1[i]} -> {package.allTables.First(ii => ii.Name == rel.Name2Table).src_id}{rel.Name2Table}:{cols2[i]} {st} ;\r\n";
                    }
                }

            }
            content += addCont;

            content += "}";

            return content;
        }


        public static async Task<string> drawContent(NpgsqlConnection conn, BlazorAppCreateETL.Shared.ETL_Package package)
        {
            string addCont = "";
            string content = "digraph {\r\n    graph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\r\n    node [shape=plain]\r\n    rankdir=LR;\r\n";
            List<GenerateStatement.ItemTable> tables = new List<GenerateStatement.ItemTable>();
            foreach(var table in package.allTables)
            {
                await DBInterface.enrichTable(conn, table);
            }
            var src = package.allTables.Select(ii => ii.src_id).ToList();
            src.Add(package.ETL_dest_id);

            foreach (var item in src.Distinct().OrderBy(ii => ii))
            {
                var tab = package.allTables.FirstOrDefault(ii => ii.src_id == item);
                string zoneName = "unknown";
                var color = "lightgray";
                if (tab != null)
                {
                    if ((bool)tab.pci_dss_zone)
                        color = "lightgreen";
                    zoneName = tab.scema.Replace(" ", "_");
                }
                content += $"subgraph cluster_{zoneName} {{\r\n    label={zoneName};\r\n    style=filled;\r\n    color={color};\r\n";
                foreach (var table in package.allTables.Where(ii => ii.src_id == item))
                {
                    content += $"{table.table_name} [label=<\r\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\r\n  <tr><td bgcolor=\"white\"><b><i>{table.table_name}</i></b></td></tr>";
                    foreach (var col in table.columns)
                        content += $"<tr><td port=\"{col.col_name}\">{col.col_name}</td></tr>";
                    content += "</table>>];";
                    foreach (var it in package.selectedFields.Where(ii => ii.sourceColumn.table.table_name == table.table_name))
                    {
                        addCont += $"{table.table_name}:{it.sourceColumn.col_name} -> {it.outputTable}:{it.sourceColumn.alias} ;\r\n";

                    }

                }
                content += "}";
            }
            foreach (var rel in package.relations)
            {
                await DBInterface.enrichRelation(conn,rel);
                var cols1 = rel.column1Name;
                var cols2 = rel.column2Name;
                /*                if (string.IsNullOrEmpty(rel.Name1Table))
                                {
                                    int yy = 0;
                                }
                                else*/
                {
                    for (int i = 0; i < cols1.Count; i++)
                    {
                        var st = (package.allTables.First(ii => ii.table_name == rel.table1.table_name).src_id == package.allTables.First(ii => ii.table_name== rel.table2.table_name).src_id) ? ("[constraint=false,style=\"dotted\"]") : ("");
                        addCont += $"{rel.table1.table_name}:{cols1[i]} -> {rel.table2.table_name}:{cols2[i]} {st} ;\r\n";
                    }
                }

            }
            content += addCont;

            content += "}";

            return content;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using ETL_DB_Interface;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using WinFormsApp1;

namespace WinFormsETLPackagedCreator
{
    public class GraphvizTest
    {

        public static Bitmap toGraphviz(string body= "digraph{a -> b; b -> c; c -> a;}")
        {
            var getStartProcessQuery = new GetStartProcessQuery() ;
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
          
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
            wrapper.GraphvizPath = @"C:\Program Files\Graphviz\bin\";

            byte[] output = wrapper.GenerateGraph(body,Enums.GraphReturnType.Png/* Enums.GraphReturnType.Png*/);

            Bitmap bmp;
            using (MemoryStream mStream = new MemoryStream())
            {
              //  byte[] pData = blob;
                mStream.Write(output, 0, Convert.ToInt32(output.Length));
                bmp = new Bitmap(mStream, false);
            }
           /* using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(new Bitmap((@"C:\Users\Mena\Desktop\1.png"), new Point(182, 213));
            }*/
     //       pictureBox2.Image = bmp;
            return bmp;
        }
        public static string drawContent(GenerateStatement.ETL_Package package)
        {
            string addCont = "";
            string content = "digraph {\r\n    graph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\r\n    node [shape=plain]\r\n    rankdir=LR;\r\n";
            var src= package.tables.Select(ii => ii.src_id).ToList();
            src.Add(package.dest_id);

            foreach(var item in src.Distinct().OrderBy(ii=>ii))
            {
                var tab=package.allTables.FirstOrDefault(ii => ii.src_id == item);
                string zoneName = "unknown";
                var color= "lightgray";
                if(tab != null)
                {
                    if (tab.pci_dss_zone)
                        color = "lightgreen";
                    zoneName = tab.src_name.Replace(" ","_");
                }
                content += $"subgraph cluster_{zoneName} {{\r\n    label={zoneName};\r\n    style=filled;\r\n    color={color};\r\n";
                foreach(var table in package.tables.Where(ii=>ii.src_id == item))
                {
                    content += $"{table.Name} [label=<\r\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\r\n  <tr><td bgcolor=\"white\"><b><i>{table.Name}</i></b></td></tr>";
                    foreach (var col in table.columns)
                        content += $"<tr><td port=\"{col.Name}\">{col.Name}</td></tr>";
                    content += "</table>>];";
                    foreach(var opt in table.optionalRelItems) {
                        addCont += $"{opt.srcTable.Name}:{opt.colSrc.Name} -> {table.Name}:{opt.colDst.Name} ;\r\n";
                    }
                }
                content += "}";
            }
            foreach( var rel in package.relations)
            {
                var cols1=rel.NameColumns1.Split(',');
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
                        addCont += $"{rel.Name1Table}:{cols1[i]} -> {rel.Name2Table}:{cols2[i]} {st} ;\r\n";
                    }
                }

            }
            content += addCont;

            content += "}";

            return content;
        }

        public interface IGraphviz
        {
            string graphviz_body { get; }
            virtual int graphviz_order { get=>1; }
            virtual string? graphviz_ancestor { get=>null; }
        }

    }
}

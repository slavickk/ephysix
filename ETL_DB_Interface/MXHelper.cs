using CamundaInterface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ParserLibrary.ReplaySaver;
using MXGraphHelperLibrary;
using NUnit.Framework;
using static ETL_DB_Interface.GenerateStatement.ETL_Package;
using System.Text.RegularExpressions;

namespace ETL_DB_Interface
{
    public static  class MXHelper
    {
        public class SavedItem
        {
            public string sql_query { get; set; }
            public TableDefine[] def { get; set; }
            public ExecContextItem[] commands { get; set; }
        }


        public static IEnumerable<string> getQueryVariables(this string body)
        {
            string pattern = @"\:\w+";
            foreach (Match match in Regex.Matches(body, pattern,
                         RegexOptions.None,
                         TimeSpan.FromSeconds(1)))
                if(match.Success && match.Length>0)
                yield return match.Value.Substring(1, match.Value.Length - 1);
        }


        private static Dictionary<string, object> getVariables(this GenerateStatement.ETL_Package pack)
        {
            //     var trans = new FimiXmlTransport();
            return pack.variables.Select(x => new KeyValuePair<string, object>(x.Name, x.Type switch
            {
                "Long" => Int64.Parse(x.DefaultValue),
                "Integer" => Int64.Parse(x.DefaultValue),
                "JSON" => JsonDocument.Parse(x.DefaultValue),
                _ => x.DefaultValue
            }))
    .ToDictionary(x => x.Key, x => x.Value);
        }
        static MXGraphDoc.Box.Body.Row.Column.Item getItemForID(MXGraphDoc.Box box, string id)
        {
            foreach (var row in box.body.rows)
            {
                foreach (var col in row.columns)
                {
                    if (col.item?.box_id?.ToUpper() == id?.ToUpper())
                        return col.item;
                }
            }
            return null;
        }



        static string getStyle(bool topleft,bool topright,bool bottomleft,bool bottomright,bool drawdelimiter=false)
        {
            return $"padding: 5px 30px;background: var(--global-white);vertical-align: top;border-bottom: 1px solid var(--grey-10); border-top:none;{(!drawdelimiter ? "border-bottom:none;" : "")} {(!topleft ? "border-top-left-radius: 0;" : "")}{(!topright ? "border-top-right-radius: 0;" : "")}{(!bottomleft ? "border-bottom-left-radius: 0;" : "")}{(!bottomright ? "border-bottom-right-radius: 0;" : "")}";
        }

        public static MXGraphDoc.Box getConditionsRows(this MXGraphDoc.Box box, string Caption, string[] leftStrings, string[] rightStrings)
        {
            if (box.body.rows == null)
                box.body.rows = new List<MXGraphDoc.Box.Body.Row>();
//            List<MXGraphDoc.Box.Body.Row> rows = new List<MXGraphDoc.Box.Body.Row>();
            if(string.IsNullOrEmpty(Caption))
            {
                box.body.rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item=new MXGraphDoc.Box.Body.Row.Column.Item() {  caption=Caption, colspan=2, style= "\"padding: 5px 30px;border-top: 1px solid var(--grey-10); border-bottom: none; background: var(--global-white);vertical-align: top;border-bottom-right-radius: 0;border-bottom-left-radius: 0; font: var(--font-h3-semibold-14);\"" } } } });
            }
            int kol = Math.Max(leftStrings.Length, rightStrings.Length);
            int kol1 = kol - 1;
            for(int i=0;  i<kol; i++)
            {
                box.body.rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item()
                {

                    box_id = ((leftStrings.Length > i) ? (box.id + "_Inp" + i) : null), caption = ((leftStrings.Length > i) ? leftStrings[i] : ""),
                    style =getStyle(i==0,false,i==kol-1,false,!( i<kol-1))

                }
                },
                    new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item()
                    {
                    box_id = ((rightStrings.Length > i) ? (box.id + "_Out" + i) : null), caption = ((rightStrings.Length > i) ? rightStrings[i] : ""),
                    style =getStyle(false,i==0,false,i==kol-1,!( i<kol-1))

                    } }
                } });
            }
            return box;
        }
        public static MXGraphDoc.Box getNewRow(this MXGraphDoc.Box box, string id, string Caption, bool left, bool right)
        {
            if (box.body.rows == null)
                box.body.rows = new List<MXGraphDoc.Box.Body.Row>();

            //            List<MXGraphDoc.Box.Body.Row> rows = new List<MXGraphDoc.Box.Body.Row>();
            box.body.rows.Add(new MXGraphDoc.Box.Body.Row()
            {
                columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item()
                {

                    box_id = ((left) ? (box.id + id + "_left") : null), caption = Caption,
                    style = "position: relative;box-sizing: border-box;width: calc(100% - 20px);height: 30px;padding: 5px 30px;margin: 0px auto;border-radius: 8px;border: 1px dashed var(--grey-10);background: var(--grey-1);border-bottom-right-radius: 0;border-top-right-radius: 0;border-right:none;"


                }
                },
                    new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item()
                {

                    box_id = ((right) ? (box.id + id + "_right") : null), caption = "",
                    style = "position: relative;box-sizing: border-box;width: calc(100% - 20px);height: 30px;padding: 5px 30px;margin: 0px auto;border-radius: 8px;border: 1px dashed var(--grey-10);background: var(--grey-1);border-bottom-left-radius: 0;border-top-left-radius: 0;border-left:none;"


                } }
                }
            });
            return box;
        }
        public static MXGraphDoc getExample()
        {
            MXGraphDoc doc = new MXGraphDoc() ;
            
            doc.boxes = new List<MXGraphDoc.Box>();
            doc.boxes.Add(new MXGraphDoc.Box() { id = "Script", header = new MXGraphDoc.Box.Header() { caption = "Script:aaa", position = new MXGraphDoc.Box.Header.Position() { left = 100, top = 100 }, size = new MXGraphDoc.Box.Header.Size() { height = 200, width = 200 } }, body = new MXGraphDoc.Box.Body() }.getConditionsRows( "Fix",new string[] {"Input1","Input2","Input3"},new string[] {"Output1" }));
            doc.boxes.Add(new MXGraphDoc.Box() { id = "AAA", header = new MXGraphDoc.Box.Header() { caption = "Transform", position = new MXGraphDoc.Box.Header.Position() { left = 100, top = 300 }, size = new MXGraphDoc.Box.Header.Size() { height = 200, width = 200 } }, body = new MXGraphDoc.Box.Body() }.getConditionsRows("Cond", new string[] { "Condition1","Condition2" }, new string[] {  }).getNewRow("Item1", "Item1", true, true).getNewRow("Item2", "Item2", true, true));
            doc.boxes.Add(new MXGraphDoc.Box() { id = "CondConverter", header = new MXGraphDoc.Box.Header() { caption = "CondConvert", position = new MXGraphDoc.Box.Header.Position() { left = 400, top = 100 }, size = new MXGraphDoc.Box.Header.Size() { height = 200, width = 200 } }, body = new MXGraphDoc.Box.Body() }.getNewRow("Item1", "Item1", true, true).getNewRow("Item2", "Item2", true, false));
            doc.Save("C:\\d\\ex1.json");
            return doc;
        }

        public static MXGraphDoc.Box deserializeBox(string sss)
        {
            return JsonSerializer.Deserialize<MXGraphDoc>(sss).boxes[0];
        }
        public static string serializeBox(MXGraphDoc.Box box)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { IgnoreNullValues = true };
            return JsonSerializer.Serialize<MXGraphDoc.Box>(box,options);
        }



        public static async Task DrawMXGraph(this GenerateStatement.ETL_Package pack,NpgsqlConnection conn)
        {
            try
            {
                int left = 190;
                int top = 90;
                SavedItem itemSaved = null;
                if(!string.IsNullOrEmpty(pack.ETL_add_define))
                    itemSaved = JsonSerializer.Deserialize<SavedItem>(pack.ETL_add_define);
                var trans = new FimiXmlTransport();//????
                var def = trans.getDefine();// FIMIHelper.getDefine();

                ExecContextItem[] commands = itemSaved?.commands;
                if(commands != null && commands.Length > 0)
                {
                    commands[0].CommandItem = def.FirstOrDefault(ii => ii.Name == commands[0].Command);
                }
                MXGraphDoc doc = new MXGraphDoc();
                doc.boxes = new List<MXGraphDoc.Box>();
                /*
                 * 
                 *      "id": "selector_rows_example",
                      "header": {
                        "position": {
                          "left": 890,
                          "top": 90
                        },
                        "size": {
                          "width": 290,
                          "height": 840
                        },
                        "caption": "Selector1 name"
                      }

                 * 
                 */
                MXGraphDoc.Box boxVars = null;
                if (pack.variables.Count > 0)
                {
                    boxVars = new MXGraphDoc.Box();
                    boxVars.id = "Vars";

                    doc.boxes.Add(boxVars);
                    boxVars.header = new MXGraphDoc.Box.Header();
                    boxVars.header.caption = "ETL params";
                    boxVars.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                    boxVars.header.size = new MXGraphDoc.Box.Header.Size() { height = 500, width = 290 };
                    var rows = new List<MXGraphDoc.Box.Body.Row>();
                    boxVars.body = new MXGraphDoc.Box.Body() { rows = rows };
                    boxVars.body.header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphDoc.Box.Header() { value = "Type" }, new MXGraphDoc.Box.Header() { value = "DefaulValue" } };
                    foreach (var variable in pack.variables)
                    {
                        rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = variable.Name } }, new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = variable.Type } }, new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = variable.DefaultValue, box_id = variable.Name } } } });
                    }
                }
                MXGraphDoc.Box boxSql = null;
                if (!string.IsNullOrEmpty(itemSaved?.sql_query))
                {
                    left += 400;
                    boxSql = new MXGraphDoc.Box();
                    boxSql.id = "Sql";

                    doc.boxes.Add(boxSql);
                    boxSql.header = new MXGraphDoc.Box.Header();
                    boxSql.header.caption = "SQL query:"+ itemSaved?.sql_query ;
                    boxSql.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                    boxSql.header.size = new MXGraphDoc.Box.Header.Size() { height = 300, width = 290 };
                    var rows = new List<MXGraphDoc.Box.Body.Row>();
                    boxSql.body = new MXGraphDoc.Box.Body() { rows = rows };
                    boxSql.body.header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "Params" }, new MXGraphDoc.Box.Header() { value = "Results" } };

                    await using (var cmdCommand = new NpgsqlCommand(itemSaved.sql_query, conn))
                    {
                        var variables = pack.getVariables();
                        foreach (var patt in itemSaved.sql_query.getVariablesForPattern())
                        {
                            getItemForID(boxVars, patt.Replace("@", ""))?.AddBoxLink(boxSql, patt);
                            rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = patt, box_id = patt } }, new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = "" } } } });


                            cmdCommand.Parameters.AddWithValue(patt, variables[patt.Substring(1)]);
                        }

                        await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                        {
                            while (await readerCom.ReadAsync())
                            {
                                // Initialize commands variables
                                for (int i = 0; i < readerCom.FieldCount; i++)
                                {
                                    rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = "" } }, new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = readerCom.GetName(i), box_id = readerCom.GetName(i) } } } });
                                    //                                    comboBoxSqlColumn.Items.Add(readerCom.GetName(i));
                                }
                                break;
                            }
                        }
                    }
                }
                if (commands != null)
                {
                    left += 400;
                    top += 300;
                    List<JsonHelper.ItemLink> alls = new List<JsonHelper.ItemLink>();
                    foreach(var it in itemSaved.def)
                        alls.AddRange(it.Columns.Select(i1 =>new JsonHelper.ItemLink() { jsonPath = "Results/" + i1.path, tablePath = "Table_" + it.Table + ":id_" + i1.Name }));
                    foreach (var command in commands)
                    {
                        MXGraphDoc.Box box = new MXGraphDoc.Box();
                        box.id = "Fimi_" + command.Command;

                        doc.boxes.Add(box);
                        box.header = new MXGraphDoc.Box.Header();
                        box.header.caption = $"Fimi command:{command.Command}";
                        box.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                        box.header.size = new MXGraphDoc.Box.Header.Size() { height = 500, width = 290 };
                        var rows = new List<MXGraphDoc.Box.Body.Row>();
                        box.body = new MXGraphDoc.Box.Body() { rows = rows };

                        JsonHelper pars_json = new JsonHelper("Params");
                        foreach (var par in command.Params)
                        {
                            if (!string.IsNullOrEmpty(par.Variable))
                            {
                                if (pack.variables.Count(ii => ii.Name.ToUpper() == par.Variable.ToUpper()) > 0)
                                    getItemForID(boxVars, par.Variable)?.AddBoxLink(box,"Params/"+ par.Key);
                                else
                                    getItemForID(boxSql, par.Variable)?.AddBoxLink(box, "Params/" + par.Key);

                            }
                            pars_json.AddVal(par.Key);
                        }
                        var colLeft = new MXGraphDoc.Box.Body.Row.Column() { json = JsonDocument.Parse(pars_json.getJsonBody()).RootElement };

                        JsonHelper out_json = new JsonHelper("Results");
                        foreach (var outp in command.CommandItem.outputItems)
                        {
                            out_json.AddVal(outp.path);
                        }
                        var colRight = new MXGraphDoc.Box.Body.Row.Column() { json = JsonDocument.Parse(out_json.getJsonBody(alls)).RootElement };
                        
                        rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { colLeft, colRight } });



                        /*                    var it =new MXGraphDoc.Box.Body.Row.Column.Item();
                                                        MXGraphDoc.Box.Body.Row.Column colLeft= new MXGraphDoc.Box.Body.Row.Column(){ item= new MXGraphDoc.Box.Body.Row.Column.Item
                                                        rowMain.columns.Add()
                                                        box.body.rows.Add(rowMain);

                                                    command.CommandItem.parameters */
                    }
                    left += 600;
                   // foreach(var command in commands)
                    foreach (var table in itemSaved.def)
                    {
                        top += 300;
                        MXGraphDoc.Box box = new MXGraphDoc.Box();
                        box.id = "Table_" + table.Table;
                        pack.allTables.RemoveAll(ii => ii.Name == table.Table);

                        doc.boxes.Add(box);
                        box.header = new MXGraphDoc.Box.Header();
                        box.header.caption = $"Table:{table.Table}";
                        box.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                        box.header.size = new MXGraphDoc.Box.Header.Size() { height = 500, width = 290 };
                        var rows = new List<MXGraphDoc.Box.Body.Row>();
                        box.body = new MXGraphDoc.Box.Body() { rows = rows };

                        foreach (var col in table.Columns)
                        {
                            rows.Add(new MXGraphDoc.Box.Body.Row()
                            {
                                columns = new List<MXGraphDoc.Box.Body.Row.Column>()
                        { new MXGraphDoc.Box.Body.Row.Column() {
                            item= new MXGraphDoc.Box.Body.Row.Column.Item() { box_id="id_"+col.Name, caption=col.Name  },
                        }

                        ,
                                new MXGraphDoc.Box.Body.Row.Column()
                                {
                                    item = new MXGraphDoc.Box.Body.Row.Column.Item() { box_id = "out_" + col.Name, caption = col.Type },
                                }
                            }
                            });
                            if(string.IsNullOrEmpty(col.path))
                            {
                                getItemForID(doc.boxes.First(ii => ii.id == "Vars"), col.variable)?.AddBoxLink(box.id, "id_"+col.Name);

                            }

                            //    getItemForID(doc.boxes.First(ii => ii.id == "Fimi_" + commands[0].Command), "Results/" + col.path)?.AddBoxLink(box.id, "id_" + col.Name);

                        }
                    }

                }
                left += 400;
                top -= 300;
                foreach (var table in pack.allTables)
                {

                    top += 300;
                    MXGraphDoc.Box box = new MXGraphDoc.Box();
                    box.AppData = JsonDocument.Parse(JsonSerializer.Serialize<GenerateStatement.ItemTable>(table)).RootElement; 
                    box.id = "Table_" + table.Name ;


                    doc.boxes.Add(box);
                    box.header = new MXGraphDoc.Box.Header();
                    box.header.caption = $"Table:{table.Name}";
                    box.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                    box.header.size = new MXGraphDoc.Box.Header.Size() { height = 500, width = 290 };
                    var rows = new List<MXGraphDoc.Box.Body.Row>();
                    box.body = new MXGraphDoc.Box.Body() { rows = rows };

                    foreach (var col in table.columns)
                    {
                        rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() 
                        { new MXGraphDoc.Box.Body.Row.Column() {
                            item= new MXGraphDoc.Box.Body.Row.Column.Item() { box_id="id_"+col.Name, caption=col.Name  },
                        }
                        
                        ,
                                new MXGraphDoc.Box.Body.Row.Column()
                                {
                                    item = new MXGraphDoc.Box.Body.Row.Column.Item() { box_id = "out_" + col.Name, caption = col.Type },
                                }
                            }
                        });



                    }
                }
                var outputTables = pack.outputTable.Split(',');
                List<OutTableToLiquid> outTables = new List<OutTableToLiquid>();
                foreach (var tab in pack.allTables)
                {
                    var sel = tab.SelectList;
                    foreach (var col in sel)
                    {
                        var ot = outTables.FirstOrDefault(ii => (ii.Name == col.outputTable && outputTables.Contains(col.outputTable)) || (outputTables.Length == 1 && outputTables.First() == ii.Name));
                        col.outputTable = CorrectOutTab(outputTables, col);
                        if (ot == null)
                        {
                            ot = new OutTableToLiquid() {  Name = col.outputTable };
                            outTables.Add(ot);
                        }
                        if (!ot.Columns.Contains(col.alias))
                            ot.Columns.Add(col.alias);
                        getItemForID(doc.boxes.First(ii => ii.id == "Table_" + tab.Name), "out_" + col.expression)?.AddBoxLink("OutTable_" + col.outputTable, col.alias);
                    }
                    if(!string.IsNullOrEmpty(tab.Condition))
                    {
                        MXGraphDoc.Box.Body.Row.Column captCol = new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() {  caption = "Conditions", colspan=2,
                            style= "padding: 5px 30px;border-top: 1px solid var(--grey-10); border-bottom: none; background: var(--global-white);vertical-align: top;border-bottom-right-radius: 0;border-bottom-left-radius: 0; font: var(--font-h3-semibold-14);" } };
                        MXGraphDoc.Box.Body.Row newRowCapt = new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { captCol } };
                        doc.boxes.First(ii => ii.id == "Table_" + tab.Name).body.rows.Add(newRowCapt);

                        MXGraphDoc.Box.Body.Row.Column condCol = new MXGraphDoc.Box.Body.Row.Column() 
                        { item = new MXGraphDoc.Box.Body.Row.Column.Item() { box_id = $"Cond_{tab.Name}", caption = tab.Condition,colspan=2,
                            style= "position: relative;    box-sizing: border-box;    width: calc(100% - 20px);    height: 30px;    padding: 5px 30px;    margin: 0px auto;    border-radius: 8px;\n    border: 1px dashed var(--grey-10);\n    background: var(--grey-1);"
                        } };
                        MXGraphDoc.Box.Body.Row newRow = new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { condCol } };
                        doc.boxes.First(ii => ii.id == "Table_" + tab.Name).body.rows.Add(newRow);
                        foreach(var field in  tab.Condition.getQueryVariables())
                        {
                            getItemForID(doc.boxes.First(ii => ii.id == "Vars"), field)?.AddBoxLink(doc.boxes.First(ii => ii.id == "Table_" + tab.Name).id, $"Cond_{tab.Name}");

                        }
                    }
                }
                foreach (var rel in pack.relations)
                {
                    var split1 = rel.NameColumns1.Split(',');
                    var split2 = rel.NameColumns2.Split(',');
                    for(int i=0;i<split1.Length; i++)   
                        getItemForID(doc.boxes.First(ii => ii.id == "Table_" + rel.Name1Table ), "id_" + split1[i])?.AddBoxLink(doc.boxes.First(ii => ii.id == "Table_" + rel.Name2Table).id,"id_" + split2[i], 1);

                }

                    foreach (var table in outTables)
                {
                    left += 400;
                    top -= 300;
                    MXGraphDoc.Box box = new MXGraphDoc.Box();
                    box.id = "OutTable_" + table.Name;


                    doc.boxes.Add(box);
                    box.header = new MXGraphDoc.Box.Header();
                    box.header.caption = $"Table:{table.Name}";
                    box.header.position = new MXGraphDoc.Box.Header.Position() { left = left, top = top };
                    box.header.size = new MXGraphDoc.Box.Header.Size() { height = 500, width = 290 };
                    var rows = new List<MXGraphDoc.Box.Body.Row>();
                    box.body = new MXGraphDoc.Box.Body() { rows = rows };

                    foreach (var col in table.Columns)
                    {
                        rows.Add(new MXGraphDoc.Box.Body.Row()
                        {
                            columns = new List<MXGraphDoc.Box.Body.Row.Column>()
                        { new MXGraphDoc.Box.Body.Row.Column() {
                            item= new MXGraphDoc.Box.Body.Row.Column.Item() { box_id=col, caption=col  }
                        }
                            }
                        }
                            

                       );


                    }

                }

                doc.Save();
            }
            catch (Exception ex)
            {

            }
        }

    }
}

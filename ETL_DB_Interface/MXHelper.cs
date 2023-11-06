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

namespace ETL_DB_Interface
{
    public static  class MXHelper
    {
        public class SavedItem
        {
            public string sql_query { get; set; }
            public TableDefine[] def { get; set; }
            public APIExecutor.ExecContextItem[] commands { get; set; }
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
                    if (col.item.box_id?.ToUpper() == id?.ToUpper())
                        return col.item;
                }
            }
            return null;
        }


        public  static async Task DrawMXGraph(this GenerateStatement.ETL_Package pack,NpgsqlConnection conn)
        {
            try
            {
                int left = 190;
                int top = 90;
                var itemSaved = JsonSerializer.Deserialize<SavedItem>(pack.ETL_add_define);

                APIExecutor.ExecContextItem[] commands = itemSaved.commands;
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
                    boxVars.body.header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphDoc.Box.Header() { value = "Type" } };
                    foreach (var variable in pack.variables)
                    {
                        rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = variable.Name } }, new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = variable.Type, box_id = variable.Name } } } });
                    }
                }
                MXGraphDoc.Box boxSql = null;
                if (!string.IsNullOrEmpty(itemSaved.sql_query))
                {
                    left += 400;
                    boxSql = new MXGraphDoc.Box();
                    boxSql.id = "Sql";

                    doc.boxes.Add(boxSql);
                    boxSql.header = new MXGraphDoc.Box.Header();
                    boxSql.header.caption = "SQL query";
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
                    foreach (var command in commands)
                    {
                        MXGraphDoc.Box box = new MXGraphDoc.Box();
                        box.id = "Fimi_" + command.CommandItem.Name;

                        doc.boxes.Add(box);
                        box.header = new MXGraphDoc.Box.Header();
                        box.header.caption = $"Fimi command:{command.CommandItem.Name}";
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
                                    getItemForID(boxVars, par.Variable)?.AddBoxLink(box, par.Key);
                                else
                                    getItemForID(boxSql, par.Variable)?.AddBoxLink(box, par.Key);

                            }
                            pars_json.AddVal(par.Key);
                        }
                        var colLeft = new MXGraphDoc.Box.Body.Row.Column() { json = JsonDocument.Parse(pars_json.getJsonBody()).RootElement };

                        JsonHelper out_json = new JsonHelper("Results");
                        foreach (var outp in command.CommandItem.outputItems)
                        {
                            out_json.AddVal(outp.path);
                        }
                        var colRight = new MXGraphDoc.Box.Body.Row.Column() { json = JsonDocument.Parse(out_json.getJsonBody()).RootElement };

                        rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() { colLeft, colRight } });



                        /*                    var it =new MXGraphDoc.Box.Body.Row.Column.Item();
                                                        MXGraphDoc.Box.Body.Row.Column colLeft= new MXGraphDoc.Box.Body.Row.Column(){ item= new MXGraphDoc.Box.Body.Row.Column.Item
                                                        rowMain.columns.Add()
                                                        box.body.rows.Add(rowMain);

                                                    command.CommandItem.parameters */
                    }
                }
                left += 400;
                top -= 300;
                foreach (var table in pack.allTables)
                {
                    top += 300;
                    MXGraphDoc.Box box = new MXGraphDoc.Box();
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
                doc.Save();
            }
            catch (Exception ex)
            {

            }
        }

    }
}

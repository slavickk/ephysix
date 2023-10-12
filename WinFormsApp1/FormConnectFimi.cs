using CamundaInterface;
using BlazorAppCreateETL.Shared;
using ETL_DB_Interface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Packaging;
using WinFormsApp1;
using static CamundaInterface.APIExecutor;
using CamundaInterfaces;
//using static ETL_DB_Interface.GenerateStatement;
using NUnit.Framework.Internal;
using static CamundaInterface.CamundaExecutor.ExternalTaskAnswer;
//using static ETL_DB_Interface.GenerateStatement.ItemTask;
using System.Text.Json;
using static ETL_DB_Interface.GenerateStatement.ItemTask;
using static CamundaInterface.CamundaExecutor;
using System.Linq;
using YamlDotNet.Core.Tokens;

namespace WinFormsETLPackagedCreator
{
    public partial class FormConnectFimi : Form
    {
        long id_package;
        APIExecutor._ApiExecutor trans;
        
        public FormConnectFimi(long id_package)
        {
            trans = new FimiXmlTransport();
            this.id_package = id_package;
            InitializeComponent();
        }

        private void comboBoxFimiCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!ignoreComboSelect)
            {

            var comm = comboBoxFimiCommand.SelectedItem as APIExecutor._ApiExecutor.ItemCommand;
            FillFimiControls(comm);
            }
        }

        private void FillFimiControls(_ApiExecutor.ItemCommand? comm)
        {
            if (comm != null)
            {
                listViewFIMIInputParams.Items.Clear();
                foreach (var item in comm.parameters)
                {
                    listViewFIMIInputParams.Items.Add(new ListViewItem(new string[] { item.name, "", "" }));
                }
                //                listView
                //     this.listVie
                listViewFimiOutputParam.Items.Clear();
                //                listBoxFimiOutputParam.Items.Clear();
                foreach (var item in comm.outputItems)
                {
                    listViewFimiOutputParam.Items.Add(new ListViewItem(new string[] { item.path, "" }));
                }

            }
        }

        private void listBoxFimiParam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public class ItemKeyColumns
        {
            public string table;
            public List<string> keyColumns;
        }

        List<ItemKeyColumns> listKeyColumns= new List<ItemKeyColumns>();

        public class ItemRel
        {
            public string pktable;
            public string pkcolumns;
            public string fktable;
            public string fkcolumn;
        }
        List<ItemRel> relations= new List<ItemRel>();
        List<ETL_Package.ItemColumn> listAllColumns = new List<ETL_Package.ItemColumn>();
        private async void comboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var table = comboBoxTable.SelectedItem as ETL_Package.ItemTable;
            if (table != null)
            {
/*                ETL_Package.ItemColumn[] arr = new ETL_Package.ItemColumn[listBoxTableColumns.Items.Count];
                listBoxTableColumns.Items.CopyTo(arr, 0);*/
                var arr1=listAllColumns.Select(ii=>ii.table).DistinctBy(i1=>i1.table_name).Select(ii=>ii.table_id).ToArray();
                if (arr1.Length > 0)
                {
                    FormAddTable frm = new FormAddTable(arr1,arr1.First() , table.table_id, conn);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // package.allTables.Add(cols.table);
                        var rr=frm.fromLeftToRight;
                        foreach (var item in frm.returnedItems.Where(ii=>ii.itemName=="ForeignKey"))
                        {

                            await using (var cmdCommand = new NpgsqlCommand("select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(@idrel)", conn))
                            {
                                cmdCommand.Parameters.AddWithValue("@idrel", item.itemId);
                                await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                                {
                                    while (await readerCom.ReadAsync())
                                    {
                                        relations.Add(new ItemRel() { pktable = readerCom.GetString(0), pkcolumns = readerCom.GetString(1), fktable = readerCom.GetString(2), fkcolumn = readerCom.GetString(3) });


                                        //                            "select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(2163938)"
                                        /*                          if (item.itemName == "Table")
                                                      {
                                                      }*/
                                    }
                                }
                            }
                        }
                    }
                }
                FormSelectKeyColumns frm1 = new FormSelectKeyColumns(list.Where(ii => ii.table.table_name == table.table_name).Select(ii => ii.col_name).ToArray());
                if (frm1.ShowDialog() == DialogResult.OK)
                {
                    listKeyColumns.Add(new ItemKeyColumns() { table = table.table_name, keyColumns = frm1.keyColumns });
                    /*                    var keyColumns = frm1.keyColumns;
                                        listBoxTableColumns.Items.Clear();*/
                    var ll = list.Where(ii => ii.table.table_name == table.table_name).ToArray();
                    listAllColumns.AddRange(ll);
                    listViewTableColumns.Items.AddRange(ll.Select(ii => new ListViewItem(new string[] { "", "", "", ii.col_name, ii.table.table_name })).ToArray());
                    foreach(var key in frm1.keyColumns)
                    {
                        var index= listAllColumns.IndexOf(listAllColumns.First(ii=>ii.col_name== key && ii.table.table_name==table.table_name));
                        listViewTableColumns.Items[index].ForeColor= Color.Red;
                    }

//                    listBoxTableColumns.Items.AddRange(list.Where(ii => ii.table.table_name == table.table_name).ToArray());
                }

            }
        }

        private void listBoxTableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        List<ETL_Package.ItemColumn> list;

        bool busyText = false;
        private async void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
/*            await Task.Delay(300);
            if (!busyText)
            {
                busyText = true;
                if (!string.IsNullOrEmpty(textBoxPrefix.Text))
                {
                    list = await DBInterface.GetColumnsForTablePattern(conn, textBoxPrefix.Text);
                    comboBoxTable.Items.Clear();
                    comboBoxTable.Items.AddRange(list.Select(ii => ii.table).DistinctBy(i1 => i1.table_name).ToArray());



                }
            }*/
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {
            if(listViewFimiOutputParam.SelectedItems.Count > 0 && listViewTableColumns.SelectedItems.Count>0) 
            {
                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[0].Text = listViewFimiOutputParam.Items[index1].SubItems[0].Text;
                }
            }

        }

        private async void buttonTest_Click(object sender, EventArgs e)
        {
            APIExecutor.ExecContextItem[] commands = new APIExecutor.ExecContextItem[1];
            commands[0] = new APIExecutor.ExecContextItem();
            commands[0].Command = (comboBoxFimiCommand.SelectedItem as APIExecutor._ApiExecutor.ItemCommand).Name;
            commands[0].Params = new List<APIExecutor.ExecContextItem.ItemParam>();
            for (int i = 0; i < listViewFIMIInputParams.Items.Count; i++)
            {
                var subitems = listViewFIMIInputParams.Items[i].SubItems;
                if (subitems[1].Text.Length > 0 || subitems[2].Text.Length > 0)
                {
                    commands[0].Params.Add(new APIExecutor.ExecContextItem.ItemParam() { Key = subitems[0].Text, Value = subitems[2].Text, Variable = subitems[1].Text });
                }
            }
            Dictionary<string, object> dict = getVariables();
            var ans = await APIExecutor.ExecuteApiRequestOnly(trans, commands, textBoxSQL.Text, dict);

            if (ans == null)
            {
                MessageBox.Show(trans.getError().error, "Error");
                return;
            }
            try
            {
                for (int i = 0; i < listViewFimiOutputParam.Items.Count; i++)
                {
                    var subitems = listViewFimiOutputParam.Items[i].SubItems;
                    subitems[1].Text = string.Join(',', ans.filter(subitems[0].Text));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private Dictionary<string, object> getVariables()
        {
            //     var trans = new FimiXmlTransport();
            return pack.variables.Select(x => new KeyValuePair<string, object>(x.Name, x.Type switch
            {
                "Long" => Int64.Parse(x.DefaultValue),
                "Integer" => Int64.Parse(x.DefaultValue),
                "JSON"=>JsonDocument.Parse(x.DefaultValue),
                _ => x.DefaultValue
            }))
    .ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<CamundaProcess.ExternalTask> toExternalTask(GenerateStatement.ETL_Package package, NpgsqlConnection conn, string sqlQuery, APIExecutor.ExecContextItem[] commands, TableDefine[] tables)
        {
            var columnList=package.allTables.SelectMany(ii=>ii.columns).ToList();
            var variables = package.variables;
            int index = 0;
/*            var src = await GenerateStatement.getSrcInfo(source_id, conn);
            var dest = await GenerateStatement.getSrcInfo(dest_id, conn);
            foreach (var table in allTables.Where(ii => !string.IsNullOrEmpty(ii.url)))
            {
                listValue.Add(urlTask(src.connectionString, GenerateStatement.ConnectionStringAdm, table));
            }
*/
            CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
            retValue.id = $"Id{index}";
            retValue.name = $"ETL_Task_{index}";
            retValue.topic = "FimiConnector";
            retValue.url = "http://CSExternalTask.service.dc1.consul:24169/api/Api/FimiConnector";

            retValue.parameters.Clear();
            retValue.Annotation = $"Fimi transform data  ";
            retValue.author = "Yury Gasnikov";
            retValue.service_location = "unknown";

            retValue.description = "Execute Fimi requests and put it ( if it's demand ) into a several tables";

            {

                retValue.noDescribe = true;

               /* retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SName", src.name));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TName", dest.name));*/
                /*                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDSN", src.dsn));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SLogin", src.login));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SPassword", src.password));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDriver", src.driver));

                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDSN", dest.dsn));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TLogin", dest.login));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TPassword", dest.password));
                                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDriver", dest.driver));*/


             //   retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLTable", JsonSerializer.Serialize<List<JsonItem>>(await getJsonDefs(conn, columnList, dest_id) /*this.outputPath*/)));
                //                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", ""));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlQuery));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Oper", "None"));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("FIMICommands", JsonSerializer.Serialize<APIExecutor.ExecContextItem[]>(commands)));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Tables", JsonSerializer.Serialize<TableDefine[]>(tables)));
                //                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLColumns", columnsDescription(columnList)));
                //                   retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", String.Join(", ",variables.Select(ii=>ii.Name))));
            }
            retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Signature", "569074234566666", "Parameters signature( for control package integrity)"));
            package.usedExternalTasks.Add(retValue);
            return retValue;
        }


        List<_ApiExecutor.ItemCommand> def;
        private NpgsqlConnection conn;
//        GenerateStatement.ETL_Package pack = null;
            
        GenerateStatement.ETL_Package pack=null;
        bool ignoreComboSelect = false;
        private async void FormConnectFimi_Load(object sender, EventArgs e)
        {
            GenerateStatement.camundaAddr = Resolver.ResolveConsulAddr("Camunda");
            // DBInterface.SaveAndExecuteETL(conn, pack);
            textBoxSQL.Text = "select 1 dummy";
            conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            conn.Open();
            //  await DBInterface.SavePackage(conn, pack);
            if (this.id_package > 0)
                pack = await GenerateStatement.getPackage(conn, id_package);
            //            pack = CreateTmpJson();
            comboBoxSqlColumn.Items.Clear();
            foreach (var val in pack.variables)
            {
                comboBoxSqlColumn.Items.Add(val.Name);
            }

            def = trans.getDefine();// FIMIHelper.getDefine();
            foreach (var item in def)
            {
                comboBoxFimiCommand.Items.Add(item);
            }
            if (pack != null && !string.IsNullOrEmpty(pack.ETL_add_define))
            {
                var itemSaved = JsonSerializer.Deserialize<SavedItem>(pack.ETL_add_define);
                textBoxSQL.Text = itemSaved.sql_query;
                foreach (var com1 in itemSaved.commands)
                {
                    var comm = def.First(ii => ii.Name == com1.Command);

                    FillFimiControls(comm);
                    foreach (var par in com1.Params)
                    {
                        foreach (ListViewItem item in listViewFIMIInputParams.Items)
                        {
                            if (item.SubItems[0].Text == par.Key)
                            {
                                if (!string.IsNullOrEmpty(par.Value?.ToString()))
                                    item.SubItems[2].Text = par.Value.ToString();
                                if (!string.IsNullOrEmpty(par.Variable))
                                    item.SubItems[1].Text = par.Variable;
                            }
                        }
                    }
                    ignoreComboSelect = true;
                    int index = 0;
                    foreach (APIExecutor._ApiExecutor.ItemCommand item in comboBoxFimiCommand.Items)
                    {

                        if (item.Name == com1.Command)
                        {
                            comboBoxFimiCommand.SelectedIndex = index;
                            break;
                        }
                        index++;
                    }

/*                    comboBoxFimiCommand.Items.Add(new FIMIHelper.ItemCommand() { Name = com1.Command, parameters = com1.Params.Select(ii => new FIMIHelper.ItemCommand.Parameter() { name = ii.Key }).ToList() });
                    comboBoxFimiCommand.SelectedIndex = 0;*/
                    ignoreComboSelect = false;

                }
                foreach (var table in itemSaved.def)
                {
                    if (list == null)
                        list = new List<ETL_Package.ItemColumn>();
                    var ll = await DBInterface.GetColumnsForTableID(conn, table.table_id);
                    list.AddRange(ll);
                    listAllColumns.AddRange(ll);
                    listViewTableColumns.Items.AddRange(ll.Select(ii => new ListViewItem(new string[] { "", "", "", ii.col_name, ii.table.table_name })).ToArray());
                    foreach (var col in table.Columns)
                    {
                        foreach(ListViewItem item in listViewTableColumns.Items)
                        {
                            if (item.SubItems[3].Text == col.Name && item.SubItems[4].Text == table.Table)
                            {
                               if(!string.IsNullOrEmpty( col.variable))
                                    item.SubItems[2].Text = col.variable;
                                if (!string.IsNullOrEmpty(col.constant))
                                    item.SubItems[2].Text = col.constant;
                                if (!string.IsNullOrEmpty(col.path))
                                    item.SubItems[0].Text = col.path;
                            }
                        }

/*                        ETL_Package.ItemTable tab;
                        var tableF = listAllColumns.FirstOrDefault(ii => ii.table.table_name == table.Table);

                        if (tableF == null)
                            tab = new ETL_Package.ItemTable() { table_id = table.table_id, table_name = table.Table };
                        else
                            tab = tableF.table;
    
                        list.Add(new ETL_Package.ItemColumn() { col_id = col.col_id, col_name = col.Name, table = tab });
                        //                    var ll = list.Where(ii => ii.table.table_name == table.table_name).ToArray();
                        listAllColumns.Add(list.Last());
                        listViewTableColumns.Items.Add(new ListViewItem(new string[] { "", "", "", list.Last().col_name, list.Last().table.table_name }));*/
                    }
                    this.listKeyColumns.Add(new ItemKeyColumns() { table = table.Table, keyColumns = table.KeyColumns.ToList() });
                    this.relations.AddRange(table.ExtIDs.Select(ii => new ItemRel() { pkcolumns = ii.Column, pktable = ii.Table }));
                }
            }
        }

        private GenerateStatement.ETL_Package CreateTmpJson()
        {

            using (StreamReader sr = new StreamReader(@"c:\d\packageTemp.json"))
            {
                var content = sr.ReadToEnd();
                GenerateStatement.ETL_Package pack = JsonSerializer.Deserialize<GenerateStatement.ETL_Package>(content);
                foreach (var rel in pack.relations)
                {
                    rel.table1 = pack.allTables.First(ii => ii.etl_id == rel.table1.etl_id);
                    rel.table2 = pack.allTables.First(ii => ii.etl_id == rel.table2.etl_id);
                }
                return pack;

            }
        }

        private async void textBoxSQL_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxSQL.Text))
            {
                comboBoxSqlColumn.Items.Clear();
                foreach(var val in pack.variables)
                {
                    comboBoxSqlColumn.Items.Add(val.Name);
                }
                await using (var cmdCommand = new NpgsqlCommand(textBoxSQL.Text, conn))
                {
                    var variables = getVariables();
                    foreach (var patt in textBoxSQL.Text.getVariablesForPattern())
                    {
                        cmdCommand.Parameters.AddWithValue(patt, variables[patt.Substring(1)]);
                    }

                    await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                    {
                        while (await readerCom.ReadAsync())
                        {
                            // Initialize commands variables
                            for (int i = 0; i < readerCom.FieldCount; i++)
                            {
                                comboBoxSqlColumn.Items.Add(readerCom.GetName(i));
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void textBoxCommandPrefix_Leave(object sender, EventArgs e)
        {
            comboBoxFimiCommand.Items.Clear();
            foreach (var item in def.Where(ii=>ii.Name.Contains(textBoxCommandPrefix.Text)))
            {
                comboBoxFimiCommand.Items.Add(item);
            }

        }

        private void buttonAddConst_Click(object sender, EventArgs e)
        {
            if(/*textBoxConstant.Text.Length > 0 &&*/ listViewFIMIInputParams.SelectedIndices.Count>0)
            {
                int index= listViewFIMIInputParams.SelectedIndices[0];
                listViewFIMIInputParams.Items[index].SubItems[2].Text=textBoxConstant.Text;
            }
        }

        private void buttonAddSQLColumn_Click(object sender, EventArgs e)
        {
            var val=comboBoxSqlColumn.SelectedItem?.ToString(); 
            if(val!= null && val.Length>0)
            {
                int index = listViewFIMIInputParams.SelectedIndices[0];
                listViewFIMIInputParams.Items[index].SubItems[1].Text = val;

            }
        }

        private async void textBoxPrefix_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxPrefix.Text))
            {
                list = await DBInterface.GetColumnsForTablePattern(conn, textBoxPrefix.Text,new int[] { 1 });
                comboBoxTable.Items.Clear();
                comboBoxTable.Items.AddRange(list.Select(ii => ii.table).DistinctBy(i1 => i1.table_name).ToArray());



            }

        }

        async Task saveToCamunda(TableDefine[] tables, APIExecutor.ExecContextItem[] commands,string SQLQuery,string connectionString)
        {
   /*         CamundaProcess process = new CamundaProcess();
            string CamundaID = $"ETL_Process{id}";



        
            int index = 999;
            List<ETL_DB_Interface.GenerateStatement.ItemTask> tasks = new List<ETL_DB_Interface.GenerateStatement.ItemTask>();
            process.ProcessID = CamundaID;
            process.ProcessName = $"{"Example"}{123}";
            process.documentation = $"{"Some example"}\r\n  Not contain input variables!";
            process.tasks.Clear();
            CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
            retValue.id = $"Id{index}";
            retValue.name = $"ETL_Task_{index}";
            retValue.topic = "LoginDB";
            retValue.parameters.Clear();
            retValue.Annotation = $"FIMI executor ";
            retValue.author = "Yury Gasnikov";
            retValue.service_location = "unknown";

            retValue.description = "Collecting data from one sources(SQL) and put to another destination( SQL too)";
            {
                retValue.noDescribe = true;

                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLTable", JsonSerializer.Serialize<List<JsonItem>>(await getJsonDefs(conn, columnList, dest_id) )));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Oper", "None"));
            }
            retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Signature", "569074234566666", "Parameters signature( for control package integrity)"));
            process.tasks.Add(retValue);

            var path1 = $"{GenerateStatement.pathToSaveETL}{111}.bpmn";
            process.save(path1);
            */
        }

        private void buttonAddColumnToTable_Click(object sender, EventArgs e)
        {
            if (comboBoxSqlColumn.SelectedItem != null &&   listViewTableColumns.SelectedItems.Count > 0)
            {

//                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
//                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[2].Text = comboBoxSqlColumn.SelectedItem.ToString();
                }
            }

        }

        private void buttonAddConstantToTable_Click(object sender, EventArgs e)
        {
            if (textBoxConstant.Text.Length>0 && listViewTableColumns.SelectedItems.Count > 0)
            {

                //                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
                //                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[1].Text = textBoxConstant.Text;
                }
            }

        }

        private void listViewTableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        GenerateStatement.ItemTable formETLPackageTable(ETL_Package.ItemTable src,List<ETL_Package.ItemColumn> all_columns)
        {
            GenerateStatement.ItemTable retValue= new GenerateStatement.ItemTable()
            {
                 TableId=src.table_id, Name=src.table_name, Alias=src.alias, pci_dss_zone=src.pci_dss_zone??false
                 , etl_id=src.etl_id, src_id=src.src_id??-1, src_name=src.src_name 
            };
            retValue.columns = all_columns.Where(ii=>ii.table.table_name == src.table_name).Select(ii => new GenerateStatement.ItemTable.ColumnItem() {
                 Name=ii.col_name, Type="String"
                }).ToList();
            return retValue;
        }


        Dictionary<string, ExternalTaskAnswer.Variables> fromParametersToCamundaVars(List<GenerateStatement.ItemVar> variables)
        {
            return variables.Select(x => new KeyValuePair<string, ExternalTaskAnswer.Variables>(x.Name,new ExternalTaskAnswer.Variables() { type = x.Type, value = x.DefaultValue }))
            .ToDictionary(x => x.Key, x => x.Value);
        }
        public class SavedItem
        {
            public string sql_query { get; set; }
            public TableDefine[] def { get; set; }
            public APIExecutor.ExecContextItem[] commands { get; set; }
        }
        async Task<(string type_col,long tableid,long colid)> getColAttr(NpgsqlConnection conn,long colid)
        {
            var cmd = @"select  n1.nodeid,n2.nodeid,n2.Name,a2.key,nav3.val,n2.synonym,md.md_find_sensitive(n2.nodeid)
    from md_node n1, md_arc a1, 
md_node n2 
left join md_node_attr_val nav3 on(n2.NodeID=nav3.NodeID and nav3.AttrID=29 )
left join md_node_attr_val sens on(n2.NodeID=sens.NodeID and sens.AttrID=md_get_attr('Addon','SensData') )


, md_node_attr_val nav2,
    md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
      and a2.attrPID=1
      and n2.nodeid=@id and n1.isdeleted=false and n2.isdeleted=false  and a1.isdeleted=false";
            await using (var cmdCommand = new NpgsqlCommand(cmd, conn))
            {
                cmdCommand.Parameters.AddWithValue("@id", colid);
                await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                {
                    while (await readerCom.ReadAsync())
                    {
                        return (readerCom.GetString(3), readerCom.GetInt64(0), readerCom.GetInt64(1));
                        /*tableid = readerCom.GetInt64(0);
                        colid = readerCom.GetInt64(1);
                        type_col = readerCom.GetString(3);*/
                        break;


                        //                            "select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(2163938)"
                        /*                          if (item.itemName == "Table")
                                      {
                                      }*/
                    }
                }
            }
            return ("", -1, -1);
        }
        private async void buttonFullTest_Click(object sender, EventArgs e)
        {
            try
            {
  
                TableDefine[] tables = listAllColumns.Select(ii => ii.table.table_name).Distinct().Select(i1 => new TableDefine() { Table = i1, Columns = new List<TableDefine.Column>() }).ToArray();
                foreach (ListViewItem item in listViewTableColumns.Items)
                {
                    if (item.SubItems[1].Text.Length > 0 || item.SubItems[0].Text.Length > 0 || item.SubItems[2].Text.Length > 0 || relations.Count(ii=>ii.pktable == item.SubItems[4].Text && ii.pkcolumns==item.SubItems[3].Text)> 0 || relations.Count(ii => ii.fktable == item.SubItems[4].Text && ii.fkcolumn == item.SubItems[3].Text) > 0)
                    {
                        var col= listAllColumns.First(ii=>ii.table.table_name== item.SubItems[4].Text && ii.col_name == item.SubItems[3].Text);
                        var att=await getColAttr(conn,col.col_id);
/*                        long tableid = -1, colid = -1;
                        string type_col ="";
                        var cmd = @"select  n1.nodeid,n2.nodeid,n2.Name,a2.key,nav3.val,n2.synonym,md.md_find_sensitive(n2.nodeid)
    from md_node n1, md_arc a1, 
md_node n2 
left join md_node_attr_val nav3 on(n2.NodeID=nav3.NodeID and nav3.AttrID=29 )
left join md_node_attr_val sens on(n2.NodeID=sens.NodeID and sens.AttrID=md_get_attr('Addon','SensData') )


, md_node_attr_val nav2,
    md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
      and a2.attrPID=1
      and n2.nodeid=@id and n1.isdeleted=false and n2.isdeleted=false  and a1.isdeleted=false";
                    await using (var cmdCommand = new NpgsqlCommand(cmd, conn))
                        {
                            cmdCommand.Parameters.AddWithValue("@id", col.col_id);
                            await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                            {
                                while (await readerCom.ReadAsync())
                                {
                                    tableid = readerCom.GetInt64(0);
                                    colid = readerCom.GetInt64(1);
                                    type_col = readerCom.GetString(3);
                                    break;


                                }
                            }
                        }*/
                        tables.First(ii => ii.Table == item.SubItems[4].Text).table_id=att.tableid;
                        tables.First(ii => ii.Table == item.SubItems[4].Text).Columns.Add(new TableDefine.Column() { col_id=att.colid, Type=att.type_col, Name = item.SubItems[3].Text, constant = item.SubItems[1].Text, path = item.SubItems[0].Text, variable = item.SubItems[2].Text });
                    }
                    else
                    {
                        if (item.ForeColor == Color.Red)
                        {
                            MessageBox.Show($"Column {item.SubItems[3].Text} on table {item.SubItems[4].Text} must have source");
                            return;
                        }
                    }
                }
                foreach (var table in tables)
                {
                    var col = list.FirstOrDefault(ii => ii.col_name == "operuuid" && ii.table.table_name == table.Table);
                    if (col != null)
                    {
                        var att = await getColAttr(conn, col.col_id);
                        table.Columns.Add(new TableDefine.Column() { col_id = col.col_id, uid = true, Name = col.col_name, Type = att.type_col });
                    }
                    table.KeyColumns = this.listKeyColumns.First(ii => ii.table == table.Table).keyColumns.ToArray();

                    //var t=this.relations.Where(ii => ii.fktable == table.Table).Select(i1 => new TableDefine.ExtID() { Column = i1.pkcolumns, Table = i1.pktable }).ToArray();
                    table.ExtIDs = this.relations.Where(ii => ii.fktable == table.Table).Select(i1 => new TableDefine.ExtID() { Column = i1.pkcolumns, Table = i1.pktable }).ToList();
                }
                APIExecutor.ExecContextItem[] commands = new APIExecutor.ExecContextItem[1];
                commands[0] = new APIExecutor.ExecContextItem();
                commands[0].Command = (comboBoxFimiCommand.SelectedItem as APIExecutor._ApiExecutor.ItemCommand).Name;
                commands[0].Params = new List<APIExecutor.ExecContextItem.ItemParam>();
                for (int i = 0; i < listViewFIMIInputParams.Items.Count; i++)
                {
                    var subitems = listViewFIMIInputParams.Items[i].SubItems;
                    if (subitems[1].Text.Length > 0 || subitems[2].Text.Length > 0)
                    {
                        commands[0].Params.Add(new APIExecutor.ExecContextItem.ItemParam() { Key = subitems[0].Text, Value = subitems[2].Text, Variable = subitems[1].Text });
                    }
                }
                SavedItem itemSaved = new SavedItem() { commands = commands, def = tables,sql_query=textBoxSQL.Text };

                pack.ETL_add_define=JsonSerializer.Serialize<SavedItem>(itemSaved);
                await DBInterface.SavePackage(conn, pack);

                pack.allTables.Clear();
                pack.allTables.AddRange(listAllColumns.Select(ii => ii.table).DistinctBy(ii => ii.table_name).Select(i1 => formETLPackageTable(i1,listAllColumns)));
                await toExternalTask(pack, conn, textBoxSQL.Text, commands, tables);
                await GenerateStatement.Generate(conn, pack,true);
                await saveToCamunda(tables, commands, textBoxSQL.Text, "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;");
                //  var trans = new FimiXmlTransport();
                MessageBox.Show("All saved!!!");
           /*     var ans1=await new APIExecutor().ExecuteApiRequest(trans, commands,tables,textBoxSQL.Text, "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;", fromParametersToCamundaVars(pack.variables));
                if(ans1.Errors>0)
                {
                    MessageBox.Show(trans.getError().error, "Error");
                    return;

                }*/
                /*                var ans = await APIExecutor.ExecuteApiRequestOnly(trans, commands);
                                if (ans == null)
                                {
                                    MessageBox.Show(trans.getError().Reason.Text, "Error");
                                    return;
                                }*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void listViewFIMIInputParams_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            var comm=comboBoxFimiCommand.SelectedItem as APIExecutor._ApiExecutor.ItemCommand;
            if (comm != null && listViewFIMIInputParams.SelectedIndices.Count > 0)
            {
                int index = listViewFIMIInputParams.SelectedIndices[0];
                var par = comm.parameters.First(ii => ii.name == listViewFIMIInputParams.Items[index].SubItems[0].Text);
                if (par.alternatives?.Count > 0)
                {
                    comboBoxAlternatives.Items.Clear();
                    comboBoxAlternatives.Items.AddRange(par.alternatives.ToArray());
                }
            }
        }

        private void comboBoxAlternatives_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listViewFIMIInputParams.SelectedIndices[0];
            listViewFIMIInputParams.Items[index].SubItems[2].Text= comboBoxAlternatives.SelectedItem.ToString();

        }
    }
}

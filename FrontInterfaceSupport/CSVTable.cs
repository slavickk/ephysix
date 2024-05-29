using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using Npgsql;
using static FrontInterfaceSupport.DBTable;

namespace FrontInterfaceSupport
{
    public class CSVTable
    {

        public class CSVTableConfig
        {
            public class Header
            {
                public string Name { get; set; }
                public object Width { get; set; }
            }
            public int FileOrigin { get; set; }
            public string Separator { get; set; }
            public string QuoteSymbol { get; set; }
            public object EmptySymbol { get; set; }
            public int TrimOption { get; set; }
            public bool HasHeader { get; set; }
            public bool IgnoreEmptyRows { get; set; }
            public List<Header> Headers { get; set; }
            public List<List<string>> Rows { get; set; }
        }
        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createCSVBox(IConfiguration conf, MXGraphDoc retDoc, string tableDefJson, MXGraphDoc.Box oldbox)
        {
            CSVTableConfig dbTableConfig = JsonSerializer.Deserialize<CSVTableConfig>(tableDefJson);

            return await AddTable(retDoc, dbTableConfig);
        }


        private static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> AddTable(MXGraphDoc retDoc, CSVTableConfig dbTableConfig)
        {
            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            retBox.category = "receiving";

            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<CSVTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();
            if (retDoc.boxes.Count == 0)
            {
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
            }
            else
            {
                int delta = 15;
                int left = retDoc.boxes.Min(ii => ii.header.position.left);
                int top = retDoc.boxes.Max(ii => ii.header.position.top + ii.header.size.height) + delta;
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
            }

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = heigthHeaderBox + heigthRow };
            //            retBox.id = mxGraphID;
            retBox.type = "csv";
            retBox.category = "source";
            retBox.body = new MXGraphHelperLibrary.MXGraphDoc.Box.Body();
            retBox.body.header = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Header>() { new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Type" } };
            retBox.body.rows = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row>();

            {
                {
                    foreach (var header in dbTableConfig.Headers)
                    {

                        retBox.header.size.height += heigthRow;
//                        retBox.header.zone_name = reader.GetString(4);
  //                      bool isPciDss = reader.GetBoolean(5);
                        retBox.header.zone_type = "unknown";
                        retBox.header.caption = "CSV_Table";
                        retBox.id = "CSVTable1";
                        retBox.header.description = "unknown";
                        retBox.body.rows.Add(new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row()
                        {
                            tooltip_info = new Dictionary<string, string>() {
                                { "name", "Name" },
                                { "description", "Description" },
                                { "type", "Data type" }
                            },
                            columns = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column>() {
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()  { item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {caption= header.Name } },
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()  { item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {caption="STRING", box_id=retBox.id+"_"+header.Name } }
                            }

                        }
                            );
                        //                        list.Add(new ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1)/*, table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), src_name = reader.GetString(4) }*/ });
                    }
                }
            }

            retDoc.boxes.Add(retBox);
            return retBox;
        }

        public static async Task<string> CreateOrModifyTables(string jsonMXGrapth = "", string tableDefJson = "{\r\n  \"tableId\":550119,\r\n  \"tableExistedId\":550079,\r\n  \"conditions\":[\"OriginalTime>@timeBegin\",\"OriginalTime<@timeEnd\",\"OriginalTime is not null\"],\r\n  \"relation\":[[\"Table\",\"550119\",\"account\",\"\"],[\"Column\",\"550130\",\"branchid\",\"account\"],[\"ForeignKey\",\"2163920\",\"branchid\",\"2163595\",\"branchid\",\"550130\"],[\"Column\",\"2163595\",\"branchid\",\"branch\"],[\"ForeignKey\",\"2163944\",\"branchid\",\"2163595\",\"branchid\",\"550095\"],[\"Column\",\"550095\",\"branchid\",\"card\"],[\"Table\",\"550079\",\"card\",\"\"]]\r\n\r\n\r\n\r\n,\r\n  \"depth\":6\r\n}", IConfiguration conf = null, bool isNew = true)
        {
        
            if (!isNew)
                return jsonMXGrapth;
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();

            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            CSVTableConfig dbTableConfig = JsonSerializer.Deserialize<CSVTableConfig>(tableDefJson);
            var box1 = await AddTable(retDoc, dbTableConfig);
            JsonSerializerOptions options = new JsonSerializerOptions() { IgnoreNullValues = true };

            return JsonSerializer.Serialize<MXGraphHelperLibrary.MXGraphDoc>(retDoc, options);

        }

        public static void test()
        {
          /*  var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = " ",
                Comment = '#',
                HasHeaderRecord = false
            };*/



            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {

                DetectDelimiter = true,
  WhiteSpaceChars = new[] { ' ', '\t' }, // Note \t, otherwise it won't be trimmed.
  TrimOptions = TrimOptions.Trim//.InsideQuotes
            };
//            TrimOptions.
  //          configuration..UseNewObjectForNullReferenceMembers
            //   var conf =new CsvConfiguration()
            using (var reader = new StreamReader(@"C:\D\ardsqb.ext"))
            using (var csv = new CsvReader(reader,configuration))
            {
                while(csv.Read())
                {
                    for (int i = 0; i < csv.ColumnCount; i++)
                    {
                        var e1 = csv.GetField(i);
                    }
                }
//                var records = csv.GetRecords<Foo>();
            }
        }
    }
}

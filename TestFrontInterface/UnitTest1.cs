using ClassApiExecutor;
using ETL_DB_Interface;
using FrontInterfaceSupport;
using Microsoft.AspNetCore.WebUtilities;
using MXGraphHelperLibrary;
using System.Text.Json;

namespace TestFrontInterface
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async Task TestReceiverExecs()
        {
            var rets1 = await ReceiverHelper.getItem();

            var arr = await ReceiverHelper.getExamples("C:\\D\\OUT_DummyProtocol1", null, "TransactionAmount");
            arr = await ReceiverHelper.getExamples("C:\\D\\OUT_DummyProtocol1", arr, "TransactionAmount","2568");
            var rets = await ReceiverHelper.getItem("sa123",arr[0]);
            Assert.AreNotEqual(rets, 0);
        }


        [TestMethod]
        public async Task TestAllExecs()
        {
            var rets3 =await  TransformerHelper.addTransformer();
            var rets = await ServiceHelper.getSources();
            var rets1= await ServiceHelper.getMethodsForSource(rets[0]);
            var rets2 = await ServiceHelper.getServiceBox("{\"boxes\":[{\"type\":\"table\",\"id\":\"card\",\"header\":{\"position\":{\"left\":100,\"top\":100},\"size\":{\"width\":300,\"height\":574},\"caption\":\"card\",\"description\":\"unknown\",\"zone_name\":\"DATAMART\",\"zone_type\":\"unknown\"},\"AppData\":{\"tableId\":550079,\"tableExistedId\":550119,\"conditions\":[\"2 = 2\"],\"relation\":[],\"depth\":6},\"body\":{\"header\":[{\"value\":\"Name\"},{\"value\":\"Type\"}],\"rows\":[{\"columns\":[{\"item\":{\"caption\":\"cardid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"card_2163855\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"pan\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"card_550081\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"mbr\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"card_550087\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"expirationdate\"}},{\"item\":{\"caption\":\"DATE\",\"box_id\":\"card_550107\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"branchid\",\"box_id\":\"card_branchid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"card_550095\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"status\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"card_550099\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"statustime\"}},{\"item\":{\"caption\":\"TIMESTAMP\",\"box_id\":\"card_550103\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"firstusedate\"}},{\"item\":{\"caption\":\"DATE\",\"box_id\":\"card_550111\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"closedate\"}},{\"item\":{\"caption\":\"DATE\",\"box_id\":\"card_550115\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"scope\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"card_2163863\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"operuuid\"}},{\"item\":{\"caption\":\"TEXT\",\"box_id\":\"card_2201938\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"accountid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"card_2163859\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"Conditions\",\"colspan\":2,\"style\":\"padding: 5px 30px;border-top: 1px solid var(--grey-10); border-bottom: none; background: var(--global-white);vertical-align: top;border-bottom-right-radius: 0;border-bottom-left-radius: 0; font: var(--font-h3-semibold-14);\"}}]},{\"columns\":[{\"item\":{\"caption\":\"2 = 2\",\"box_id\":\"cond_0\",\"colspan\":2,\"style\":\"padding: 5px 30px;background: var(--global-white);vertical-align: top;border-bottom: 1px solid var(--grey-10); border-top:none; border-top-right-radius: 0;border-top-left-radius: 0;\"}}]}]}},{\"type\":\"table\",\"id\":\"account\",\"header\":{\"position\":{\"left\":100,\"top\":689},\"size\":{\"width\":300,\"height\":676},\"caption\":\"account\",\"description\":\"unknown\",\"zone_name\":\"DATAMART\",\"zone_type\":\"unknown\"},\"AppData\":{\"tableId\":550119,\"tableExistedId\":550079,\"conditions\":[\"OriginalTime\\u003E@timeBegin\",\"OriginalTime\\u003C@timeEnd\",\"OriginalTime is not null\"],\"relation\":[[\"Table\",\"550119\",\"account\",\"\"],[\"Column\",\"550130\",\"branchid\",\"account\"],[\"ForeignKey\",\"2163920\",\"branchid\",\"2163595\",\"branchid\",\"550130\"],[\"Column\",\"2163595\",\"branchid\",\"branch\"],[\"ForeignKey\",\"2163944\",\"branchid\",\"2163595\",\"branchid\",\"550095\"],[\"Column\",\"550095\",\"branchid\",\"card\"],[\"Table\",\"550079\",\"card\",\"\"]],\"depth\":6},\"body\":{\"header\":[{\"value\":\"Name\"},{\"value\":\"Type\"}],\"rows\":[{\"columns\":[{\"item\":{\"caption\":\"statustime\"}},{\"item\":{\"caption\":\"TIMESTAMP\",\"box_id\":\"account_550147\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"accountid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"account_2163749\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"externalid\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"account_550125\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"branchid\",\"box_id\":\"account_branchid\",\"box_links\":[{\"link\":{\"typelink\":2,\"box_id\":\"branch:branch_branchid\",\"points\":{\"x\":85,\"y\":0}}}]}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"account_550130\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"orignumber\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"account_550134\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"customerid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"account_550139\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"opendate\"}},{\"item\":{\"caption\":\"DATE\",\"box_id\":\"account_550151\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"closedate\"}},{\"item\":{\"caption\":\"DATE\",\"box_id\":\"account_550155\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"currency\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"account_550159\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"balance\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"account_2163753\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"scope\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"account_2163757\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"operuuid\"}},{\"item\":{\"caption\":\"TEXT\",\"box_id\":\"account_2202185\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"status\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"account_550143\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"Conditions\",\"colspan\":2,\"style\":\"padding: 5px 30px;border-top: 1px solid var(--grey-10); border-bottom: none; background: var(--global-white);vertical-align: top;border-bottom-right-radius: 0;border-bottom-left-radius: 0; font: var(--font-h3-semibold-14);\"}}]},{\"columns\":[{\"item\":{\"caption\":\"OriginalTime\\u003E@timeBegin\",\"box_id\":\"cond_0\",\"colspan\":2,\"style\":\"padding: 5px 30px;background: var(--global-white);vertical-align: top;border-radius:0; border-top: none; border-bottom: none;\"}}]},{\"columns\":[{\"item\":{\"caption\":\"OriginalTime\\u003C@timeEnd\",\"box_id\":\"cond_1\",\"colspan\":2,\"style\":\"padding: 5px 30px;background: var(--global-white);vertical-align: top;border-radius:0; border-top: none; border-bottom: none;\"}}]},{\"columns\":[{\"item\":{\"caption\":\"OriginalTime is not null\",\"box_id\":\"cond_2\",\"colspan\":2,\"style\":\"padding: 5px 30px;background: var(--global-white);vertical-align: top;border-bottom: 1px solid var(--grey-10); border-top:none; border-top-right-radius: 0;border-top-left-radius: 0;\"}}]}]}},{\"type\":\"table\",\"id\":\"branch\",\"header\":{\"position\":{\"left\":100,\"top\":1380},\"size\":{\"width\":300,\"height\":778},\"caption\":\"branch\",\"description\":\"unknown\",\"zone_name\":\"DATAMART\",\"zone_type\":\"unknown\"},\"AppData\":{\"tableId\":550542,\"tableExistedId\":0,\"conditions\":null,\"relation\":null,\"depth\":0},\"body\":{\"header\":[{\"value\":\"Name\"},{\"value\":\"Type\"}],\"rows\":[{\"columns\":[{\"item\":{\"caption\":\"branchid\",\"box_id\":\"branch_branchid\",\"box_links\":[{\"link\":{\"typelink\":2,\"box_id\":\"card:card_branchid\",\"points\":{\"x\":70,\"y\":0}}}]}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"branch_2163595\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"externalid\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550548\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"institutionid\"}},{\"item\":{\"caption\":\"BIGINT\",\"box_id\":\"branch_550553\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"title\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550557\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"region\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550566\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"city\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550571\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"address1\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550576\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"address2\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550581\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"address3\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550586\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"address4\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550591\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"phone1\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550596\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"phone2\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550601\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"phone3\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550606\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"email\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550611\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"contactname1\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550616\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"contactname2\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550621\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"contactname3\"}},{\"item\":{\"caption\":\"VARCHAR\",\"box_id\":\"branch_550626\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"scope\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"branch_2163599\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"operuuid\"}},{\"item\":{\"caption\":\"TEXT\",\"box_id\":\"branch_2201930\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}},{\"columns\":[{\"item\":{\"caption\":\"country\"}},{\"item\":{\"caption\":\"INTEGER\",\"box_id\":\"branch_550562\"}}],\"tooltip_info\":{\"name\":\"Name\",\"description\":\"Description\",\"type\":\"Data type\"}}]}}]}", "{\r\n        \"Type\": \"DummySystem2XmlTransport\",\r\n        \"Function\": \"GetAcctInfo\" }");
            using (StreamWriter sw = new StreamWriter(@"C:\d\sampleService.Json"))
            {
                sw.Write(rets2);
            }
            Assert.AreNotEqual(rets, 0);
        }
        [TestMethod]
        public async Task TestTranParser()
        {
            TranParserHelper.getDefine(null);
        }

        [TestMethod]
        public async Task TestStreams()
        {
            var rets = await StreamHelper.GetAllStreams("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;");
            var ret = await StreamHelper.GetStreamDescription("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", rets[0].Name);
            Assert.AreNotEqual(rets, 0);
        }


        [TestMethod]
        public async Task  TestFindTable()
        {
            var rets=await DBTable.FindTable("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", "account");
            Assert.AreNotEqual(rets,0);
        }
        [TestMethod]
        public async Task TestFindAllLinkTable()
        {
            var rets = await DBTable.FindLinkBetween2Tables("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", 550119, 550079, 14);
            var ans=JsonSerializer.Serialize<List<string[]>>(rets[1]);
            Assert.AreNotEqual(rets, 0);
        }
        [TestMethod]
        public void TestCsv()
        {
            CSVTable.test();
        }
        [TestMethod]
        public async Task TestCreateMxTable()
        {
            var mxText = "";
            mxText = await DBTable.CreateOrModifyTables(mxText, "{\r\n  \"tableId\":550079,\r\n  \"tableExistedId\":550119,\r\n  \"conditions\":[\"2 = 2\"],\r\n  \"relation\":[],\r\n  \"depth\":6\r\n}");
            mxText= await DBTable.CreateOrModifyTables(mxText);//.FindLinkBetween2Tables("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", 550079, 550119, 6);
            using(StreamWriter sw = new StreamWriter(@"C:\d\sampleDoc.Json"))
            {
                sw.Write(mxText);
            }
            Assert.AreNotEqual(mxText.Length, 0);
        }

        [TestMethod]
        public void TestSer()
        {
            string body;
            using(StreamReader sr = new StreamReader(@"c:\d\table_box.json"))
            {
                body = sr.ReadToEnd();
            }
            var box=MXHelper.deserializeBox(body);
            var sss1 =MXHelper.serializeBox(box);
        }
    }
}
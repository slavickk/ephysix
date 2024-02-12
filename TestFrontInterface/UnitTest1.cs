using ETL_DB_Interface;
using FrontInterfaceSupport;

namespace TestFrontInterface
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task  TestFindTable()
        {
            var rets=await DBTable.FindTable("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", "account");
            Assert.AreNotEqual(rets,0);
        }
        [TestMethod]
        public async Task TestFindAllLinkTable()
        {
            var rets = await DBTable.FindLinkBetween2Tables("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;", 550079, 550119, 6);
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
using BlazorAppCreateETL.Shared;
using ETL_DB_Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;

namespace BlazorAppCreateETL.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ETLDBController : ControllerBase
    {
        private readonly ILogger<ETLDBController> _logger;
        //    string db_connection_string = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=fp;";
        // string db_connection_string = "User ID=postgres;Password=1;Host=localhost;Port=5432;Database=postgres;";
        //  NpgsqlConnection conn;
        private readonly IConfiguration Configuration;

        public ETLDBController(IConfiguration configuration)
        {
            Configuration = configuration;
           // ConnPool.db_connection_string = this.Configuration["PGConnStr"];
            //        conn = ConnPool.GetConn();// new NpgsqlConnection(this.Configuration["PGConnStr"]);
            //        conn.Open();

        }

        async Task<NpgsqlConnection> getConn()
        {
            NpgsqlConnection conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            await conn.OpenAsync();
            return conn;
        }

        [HttpGet]
        [Route("DrawPackage")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<string> DrawPackage(int id)
        {
            string ret;
            NpgsqlConnection conn=null;
            try
            {
                conn = await getConn();
                /*            ItemPackage pack = new ItemPackage() { id = id };
                            var package1 = await DBInterface.FillPackageContent(conn, pack);*/
                var package = await GenerateStatement.getPackage(conn, id);
                ret = GraphvizImpl.drawContent(package);
            }
            catch(Exception ex)
            {
                ret = GraphvizImpl.drawError();
            }
            conn?.Close();
            return ret;
        }
        [HttpGet]
        [Route("FillETLPackage")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<ETL_Package> Fill_ETL_Package(int id)
        {
            var conn = await getConn();
            ItemPackage pack= new ItemPackage() { id = id };
            var ret = await DBInterface.FillPackageContent(conn, pack);
            conn.Close();
            return ret;
        }
        [HttpPost]
        [Route("drawFromSource")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<string> DrawPackage([FromBody] string packageString)
        {
            ETL_Package package= System.Text.Json.JsonSerializer.Deserialize<ETL_Package>(packageString);   
            var conn = await getConn();
            var ret = await GraphvizImpl.drawContent(conn, package);
            conn.Close();
            return ret;
        }


        [HttpGet]
        [Route("GetPackages")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<IEnumerable<ItemPackage>> GetPackagesItems()
        {
            var conn = await getConn();
            var ret = await DBInterface.GetPackagesItems(conn);


//            var ret = await DBInterface.FillPackageContent(conn, pack);
            conn.Close();
            return ret.ToArray();
        }

        [HttpGet]
        [Route("GetSrc")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<IEnumerable<ItemPackage>> GetSrcItems()
        {
            var conn = await getConn();
            var ret = await DBInterface.GetSrcItems(conn);


            //            var ret = await DBInterface.FillPackageContent(conn, pack);
            conn.Close();
            return ret.ToArray();
        }
        [HttpGet]
        [Route("GetTablesForPattern")]
        //public async Task<CCFA_Operator_Blazor.Shared.JJResultSetSet> Post(Dictionary<string, object> jjpost)
        public async Task<IEnumerable<ItemPackage>> GetTablesForPattern(string? pattern,int srcid)
        {
            var conn = await getConn();
            var ret = await DBInterface.GetTablesForPatternAndSrc(conn, pattern, srcid);


            //            var ret = await DBInterface.FillPackageContent(conn, pack);
            conn.Close();
            return ret.ToArray();
        }
        
        }
}

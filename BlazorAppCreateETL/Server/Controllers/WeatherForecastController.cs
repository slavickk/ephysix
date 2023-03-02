using BlazorAppCreateETL.Shared;
using ETL_DB_Interface;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BlazorAppCreateETL.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        async Task<NpgsqlConnection> getConn()
        {
            NpgsqlConnection conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            await conn.OpenAsync();
            return conn;
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
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
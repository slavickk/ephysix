using CamundaInterface;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using static CamundaInterface.APIExecutor;
using static CamundaInterface.CamundaExecutor;

namespace WebApiCamundaExecutors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [SwaggerSchemaFilter(typeof(OutputResultFilter))]
        public class OutputResult
        {
            [SwaggerSchema("All ")]
            public long All { get; set; }

            [SwaggerSchema("Errors ")]
            public long Errors { get; set; }
            [SwaggerSchema("UUID ")]
            public string OperUUID { get; set; }
        }

        public class OutputResultFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                schema.Example = new OpenApiObject
                {
                    ["All"] =
                        new OpenApiLong(12),
                    ["Errors"] =
                        new OpenApiLong(12)
                };
            }
        }
        HttpClient client = null;

        public class CommandItem
        {
            public string connSelect { get; set; }
            public string connAdm { get; set; }
            public string Table { get; set; }
            public string URL { get; set; }
            public string SQL { get; set; }
            public string UpdateTimeout { get; set; }
        }

        [HttpPost("url-crowler")]
        [SwaggerOperation(
Summary = "Запрос ",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "Request is successful")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<OutputResult> PostCrowler([FromBody, SwaggerParameter("Command item", Required = true)] Dictionary<string,string> item)
        //public async Task<int> GetHealthCheck()
        {
            try
            {
                //    return new OutputResult() { All = 12, Errors = 0 };
                metric_AllSendedByWeb?.Add(DateTime.Now);

                _logger.LogInformation("Start url crowler ");
                if (client == null)
                    client = new HttpClient();
                var itog = await url_crowler.execGet(client
                                     , item["ConnSelect"], item["ConnAdm"]
                                     , item["Table"], item["URL"]
                                     , item["SQL"]
                                     , Convert.ToInt32(item["UpdateTimeout"]));

                /*              var itog = await url_crowler.execGet(client
                                                      , connSelect, connAdm, Table, URL
                                                      , SQL
                                                      , UpdateTimeout);*/
                _logger.LogInformation("End url crowler ");
                return new OutputResult() { All = itog.all, Errors = itog.errors };
            } catch
            {
                metric_ErrorSendedByWeb?.Add(DateTime.Now);
                throw;
            }
        }
        [HttpPost("FimiConnector")]
        [SwaggerOperation(
Summary = "Запрос ",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "Request is successful")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<OutputResult> PostFimiConnector([FromBody, SwaggerParameter("Command item", Required = true)] Dictionary<string, string> item)
        //public async Task<int> GetHealthCheck()
        {
            try
            {
                metric_AllSendedByWeb?.Add(DateTime.Now);
                _logger.LogInformation("Start fimi connector ");
                /*   try
                   {*/
                var trans = new FimiXmlTransport();

                var ans1 = await new APIExecutor().ExecuteApiRequest(trans, System.Text.Json.JsonSerializer.Deserialize<ExecContextItem[]>(item["FIMICommands"]), System.Text.Json.JsonSerializer.Deserialize<TableDefine[]>(item["Tables"]), item["SQLText"], "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;", item.Select(x => new KeyValuePair<string, ExternalTaskAnswer.Variables>(x.Key, new ExternalTaskAnswer.Variables() { value = x.Value, type = "String" }))
                .ToDictionary(x => x.Key, x => x.Value), "xxxxxxxxx");
                _logger.LogInformation("End fimi connector ");
                return new OutputResult() { All = ans1.All, Errors = ans1.Errors, OperUUID = ans1.OperUUID };
            }
            catch
            {
                metric_ErrorSendedByWeb?.Add(DateTime.Now);
                throw;
            }
        }

        [HttpPost("to-dict-sender")]
        [SwaggerOperation(
Summary = "Запрос ",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "Request is successful")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<OutputResult> PostToDictSender([FromBody, SwaggerParameter("Command item", Required = true)] Dictionary<string, string> item)
        //public async Task<int> GetHealthCheck()
        {
            try
            {
                metric_AllSendedByWeb?.Add(DateTime.Now);
                _logger.LogInformation("Send to dict start");
                if (client == null)
                    client = new HttpClient();
                /*   try
                   {*/
                var itog = await SendToRefDataLoader.putRequestToRefDataLoader(client, "XXXXXXX:to-dict-sender"
                     , item["ConnSelect"], item["ConnAdm"], item["DictName"], "TEST", item["SQLText"]
                     , Convert.ToInt32(item["MaxRecords"]), item["DictAddr"], item["SensitiveData"], Convert.ToInt32(item["CountInKey"]
                     ), item["Fields"]);
                _logger.LogInformation("Send to dict end");
                return new OutputResult() { All = itog.all, Errors = itog.errors };
            }
            catch
            {
                metric_ErrorSendedByWeb?.Add(DateTime.Now);
                throw;
            }
        }



    }
}

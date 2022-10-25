using Microsoft.AspNetCore.Mvc;
using ParserLibrary;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiCamundaExecutors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly ILogger<MonitoringController> _logger;

        public MonitoringController(ILogger<MonitoringController> logger)
        {
            _logger = logger;
        }


        [HttpGet("ConsulHealthCheck")]
        [SwaggerOperation(
Summary = "Endpoint �������� �������� �������",
Description = "��� ������ ���������� 200 (OK)"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "���- ������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("���� ����� ������������ ��������", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("����� �������� ������� MobiCash", Required = true)] string PhoneMobicashClient)
        public async Task<int> GetHealthCheck()
        {
            _logger.LogDebug("HealthCheck OK");
            return 1;
        }


        [HttpGet("getMetrics")]
        [SwaggerOperation(
Summary = "Endpoint ������ ( � ������� Prometeus)",
Description = "���������� ������������������ ����� "
, Tags = new[] { "Monitoring", "Metrics", "Prometeus" }
)]
        [SwaggerResponse(200, "������� ������� ���������")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("���� ����� ������������ ��������", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("����� �������� ������� MobiCash", Required = true)] string PhoneMobicashClient)
        public async Task<string> GetMetrics()
        {
            _logger.LogDebug("Metrics request");
            return Pipeline.metrics.getPrometeusMetric();
            //            return 1;
        }

        /*traefik_entrypoint_request_duration_seconds_count
                { code="404",entrypoint="traefik",method="GET",protocol="http"} 44*/




        [HttpPost("/ChangeLogLevel")]
        [SwaggerOperation(
Summary = "�������� ������� ������������",
Description = "��� ������ ���������� 200 (OK)"
, Tags = new[] { "Monitoring", "LogLevel" }
)]
        [SwaggerResponse(200, "LogLevel changed")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task Post(Serilog.Events.LogEventLevel level)
        {
            ParserLibrary.Logger.levelSwitch.MinimumLevel = level;
            _logger.LogDebug("Change log level to {level} ", level);

            //            Console.WriteLine(req);
        }

    }
}
using MaxMind.GeoIP2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParserLibrary;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApiConsoleUtility.Controllers
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
        [HttpGet("SelfTest")]
        [SwaggerOperation(
Summary = "Endpoint тестирования пайплайна",
Description = "При вызове возвращает 200 (OK) Если какие то сервера недоступны -возвращает 502"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "Pipeline all right")]
        [SwaggerResponse(502, "Some servers unavailale(see body)")]
        [SwaggerResponse(501, "Pipeline not found")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("Дата время формирования операции", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("Номер телефона клиента MobiCash", Required = true)] string PhoneMobicashClient)
        public async  Task<IActionResult> SelfTest()
        {
            if(Program.pip == null)
                return StatusCode(StatusCodes.Status501NotImplemented,"pipeline not found");
            var ret =await Program.pip.SelfTest();
            if(ret.Result)
                return Ok( ret.Description);
            return StatusCode(StatusCodes.Status502BadGateway,ret.Description);

        }
/*        [HttpGet]
        public ActionResult DownloadZip()
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(fullyQualifiedFileName, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentLength = stream.Length;

            string input = $"filename=test.pdf";
            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse(input, out contentDisposition))
            {
                result.Content.Headers.ContentDisposition = contentDisposition;
            }

            return Ok(result);
        }
*/
        [HttpGet("ConsulHealthCheck")]
        [SwaggerOperation(
Summary = "Endpoint проверки здоровья сервиса",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Monitoring", "HealthCheck" }
)]
        [SwaggerResponse(200, "Жив- здоров")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("Дата время формирования операции", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("Номер телефона клиента MobiCash", Required = true)] string PhoneMobicashClient)
        public async Task<int> GetHealthCheck()
        {
            _logger.LogDebug("HealthCheck OK");
            return 1;
        }


        [HttpGet("getMetrics")]
        [SwaggerOperation(
Summary = "Endpoint метрик ( в формате Prometeus)",
Description = "Возвращает последовательность строк "
, Tags = new[] { "Monitoring", "Metrics","Prometeus" }
)]
        [SwaggerResponse(200, "Метрики успешно выгружены")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //        public string Post([FromBody, SwaggerParameter("Дата время формирования операции", Required = true)] DateTime timeCreate, [FromQuery, SwaggerParameter("Номер телефона клиента MobiCash", Required = true)] string PhoneMobicashClient)
        public async Task<string> GetMetrics()
        {
            _logger.LogDebug("Metrics request");
            return Metrics.metric.getPrometeusMetric();
//            return 1;
        }

        /*traefik_entrypoint_request_duration_seconds_count
                { code="404",entrypoint="traefik",method="GET",protocol="http"} 44*/




        [HttpPost("/ChangeLogLevel")]
        [SwaggerOperation(
Summary = "Изменяет уровень логгирования",
Description = "При вызове возвращает 200 (OK)"
, Tags = new[] { "Monitoring", "LogLevel" }
)]
        [SwaggerResponse(200, "LogLevel changed")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task Post(Serilog.Events.LogEventLevel level)
        {
            ParserLibrary.Logger.levelSwitch.MinimumLevel = level;
            _logger.LogDebug("Change log level to {level} " , level);

            //            Console.WriteLine(req);
        }

    }
}

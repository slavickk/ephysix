using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _logger.LogDebug("Change log level to " + level);

            //            Console.WriteLine(req);
        }

    }
}

/******************************************************************
 * File: WeatherForecastController.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

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
, Tags = new[] { "Monitoring", "Metrics", "Prometeus" }
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
            _logger.LogDebug("Change log level to {level} ", level);

            //            Console.WriteLine(req);
        }

    }
}
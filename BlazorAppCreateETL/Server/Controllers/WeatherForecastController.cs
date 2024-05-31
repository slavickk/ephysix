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
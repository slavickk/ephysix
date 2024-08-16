using DotLiquid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserLibrary.Plugins
{
    /*public class DummyTest
    {

    }*/
    public  class ServiceController : Controller
    {
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(ILogger<ServiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet("TestMethod")]
        public async Task<ContentResult> GetExecutionPlanDiagram()
        {
            string json_body = "";
            using (StreamReader sr = new StreamReader(@"C:\D\swagger-dummy.json"))
            {
                json_body = sr.ReadToEnd();
            }
            string template;
            using (StreamReader sr = new StreamReader(@"C:\Users\jurag\source\repos\ephysix\ParserLibrary\SwaggerTemplate.txt"))
            {
                template = sr.ReadToEnd();
            }
            this.Response.ContentType = "text/html";

            //return template.Replace("{0}", json_body);
            return new ContentResult
            {
                Content = template.Replace("{0}", json_body),

                ContentType = "text/html; charset=cp1252"/*charset = UTF - 8"*/
            };

            /*      IPlantUmlRenderer renderer = new RendererFactory().CreateRenderer(new PlantUmlSettings
                  {
                      RemoteUrl = configuration.GetConnectionString("PlantUMLServer"),
                      RenderingMode = RenderingMode.Remote
                  });

               PlantUml.Net.Tools
               return await _umlService.GetAsync(compile.planExecutionDescription, OutputFormat.Svg, token);*/
           
        }
    }
}

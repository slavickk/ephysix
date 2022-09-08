using Microsoft.AspNetCore.Hosting;
using ParserLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace WebApplicationConfigUI1.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
//            var tt=Assembly.GetAssembly(typeof(Pipeline)).GetType($"ParserLibrary.JsonSender");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

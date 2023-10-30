using CamundaInterface;
using ETL_DB_Interface;
using GraphShablons;
using Namotion.Reflection;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Xml;
using WinFormsETLPackagedCreator;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* var localTime = DateTime.Now;
             DateTimeOffset localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));

             var q = localTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);*/
            GenerateStatement.camundaAddr = Environment.GetEnvironmentVariable("CAMUNDA_ADDR");
            var executor = new RTPXmlTransport();
            var tt=executor.getDefine();
            
            var retValue = executor.ExecAsync(new APIExecutor.ExecContextItem[] { new APIExecutor.ExecContextItem() { Command = tt[0], Params= new List<APIExecutor.ExecContextItem.ItemParam>() { new APIExecutor.ExecContextItem.ItemParam() { Key = tt[0].parameters[1].name, FullAddr = tt[0].parameters[1].fullPath, Value= "1234560000000009" } } } }).GetAwaiter().GetResult();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
           // var ex=Shablon.getExample();
            Application.Run(new Form1()/* new FormConnectFimi(-1)*/);
        }   }
}
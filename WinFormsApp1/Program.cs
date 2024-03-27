using CamundaInterface;
using ETL_DB_Interface;
using GraphShablons;
using MXGraphHelperLibrary;
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
          /*  string a = "ass=:b1 and fff=:b2";
           foreach(var t in  a.getQueryVariables())
            {
                Console.WriteLine(t);
            }
          */
            /* var localTime = DateTime.Now;
             DateTimeOffset localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));

             var q = localTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);*/
            /*JsonHelper hh = new JsonHelper("root");
            hh.AddVal("aa");
            hh.AddVal("aa/bb");
            hh.AddVal("aa/cc");
            hh.AddVal("bb");
            var ss = hh.getJsonBody();
            */
            GenerateStatement.camundaAddr = Environment.GetEnvironmentVariable("CAMUNDA_ADDR");
       /*     var executor = new RTPXmlTransport();
            var tt=executor.getDefine();
            
            var retValue = executor.ExecAsync(new ExecContextItem[] { new ExecContextItem() { CommandItem = tt[0], Params= new List<ExecContextItem.ItemParam>() { new ExecContextItem.ItemParam() { Key = tt[0].parameters[1].name, FullAddr = tt[0].parameters[1].fullPath, Value= "1234560000000009" } } } }).GetAwaiter().GetResult();
       */
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
           // var ex=Shablon.getExample();
            Application.Run(new Form1()/* new FormConnectFimi(-1)*/);
        }   }
}
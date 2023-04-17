using ETL_DB_Interface;
using System.Text.RegularExpressions;

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

   
    }
}
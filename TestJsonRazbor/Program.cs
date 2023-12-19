using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ETL_DB_Interface;
using ParserLibrary;

namespace TestJsonRazbor
{
    static class Program
    {

        public static string ExecutedPath = "";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
         //   MXHelper.getExample();
            if(args.Length >0)
                ExecutedPath = args[0];
           // var assm=ParserLibrary.PluginsInterface.loadPlugins(new string[] { @"C:\Users\User\source\repos\projectx\Plugins\bin\Debug\net6.0\CCFAProtocols.dll" , @"C:\Users\User\source\repos\projectx\Plugins\bin\Debug\net6.0\UAMP.dll",  @"C:\Users\User\source\repos\projectx\Plugins\bin\Debug\net6.0\Plugins.dll" });
            /*object s = "2.2";
            var t= double.Parse((s as string).Replace(".",","));
            */
//            var ti = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
   //         int a1 = 35;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new FormTypeDefiner() { tDefine = typeof(Sender) }/*FormPipeline()*/);
            //            Application.Run(new Form3());
           // TIPRecieverTests.Init();
            Application.Run(new FormPipeline());
//            Application.Run(new FormSelectField());
        }
    }
}

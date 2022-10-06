using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            if(args.Length >0)
                ExecutedPath = args[0];
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

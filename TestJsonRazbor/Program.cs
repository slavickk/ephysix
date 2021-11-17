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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new FormTypeDefiner() { tDefine = typeof(Sender) }/*FormPipeline()*/);
            Application.Run(new FormPipeline());
//            Application.Run(new FormSelectField());
        }
    }
}

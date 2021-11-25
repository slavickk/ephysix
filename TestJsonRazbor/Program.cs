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
   //         int a1 = 35;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new FormTypeDefiner() { tDefine = typeof(Sender) }/*FormPipeline()*/);
            Application.Run(new Form3());
            Application.Run(new FormPipeline());
//            Application.Run(new FormSelectField());
        }
    }
}

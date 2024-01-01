using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UniElLib;

namespace ParserLibrary.Tests
{
    public class TransformerTests
    {
        string[] ConvObject(string[] params1)
        {
            return new string[] { (Convert.ToDouble(params1[0]) / 1000).ToString() };
        }
        [Test]
        public void TestTransformer()
        {
            var tr1 = new RegexpTransformer();
            var arr = tr1.transform(new string[] { "22200*****037=2512" });
//            var tr = new CSScriptTransformer("(@Input1+@Input0)/10", true);
            var tr =new CSScriptTransformer("$\"20{@Input1}-{@Input2}-01T00:00:00\"");
            var out1=tr.transform(arr);
        }
    }
}

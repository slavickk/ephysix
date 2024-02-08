using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace FrontInterfaceSupport
{
    public class CSVTable
    {
        public static void test()
        {
          /*  var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = " ",
                Comment = '#',
                HasHeaderRecord = false
            };*/



            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {

                DetectDelimiter = true,
  WhiteSpaceChars = new[] { ' ', '\t' }, // Note \t, otherwise it won't be trimmed.
  TrimOptions = TrimOptions.Trim//.InsideQuotes
            };
//            TrimOptions.
  //          configuration..UseNewObjectForNullReferenceMembers
            //   var conf =new CsvConfiguration()
            using (var reader = new StreamReader(@"C:\D\ardsqb.ext"))
            using (var csv = new CsvReader(reader,configuration))
            {
                while(csv.Read())
                {
                    for (int i = 0; i < csv.ColumnCount; i++)
                    {
                        var e1 = csv.GetField(i);
                    }
                }
//                var records = csv.GetRecords<Foo>();
            }
        }
    }
}

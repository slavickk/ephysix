using System;
//using System.Reflection;
using ParserLibrary;


namespace ConsoleIntegrityUtility
{
    class Program
    {
        static int Main(string[] args)
        {
/*            JsonSender sender = new JsonSender() { url = "http://192.168.75.160:25080/api/Streams/LoadStream1" };
            var answer=sender.internSend("{\"stream\":\"loginEnter\",\"originalTime\":\"2020-12-19T21:06:35.2387735+05:00\",\"login\":\"+79222310645\"}").Result;*/
/*            var bytes=Convert.FromBase64String("M0RTLzNSSUluZGljYXRvcj0QM0RTL0V4cFRpbWVJbnRlcnZhbD02MBBFeHQvTmV0d29yaz0xMQ==");

            string value = System.Text.Encoding.UTF8.GetString(bytes);*/
/*            var pip2 = new Pipeline();
            pip2.Save( @"aa3.yml");*/
       /*     var tt=typeof(LongLifeRepositorySender).IsSubclassOf(typeof(Sender));
            var tt1 = typeof(LongLifeRepositorySender).IsAssignableTo(typeof(Sender));
            // typeof(ComparerForValue).GenericTypeParameters
            //           typeof(ComparerForValue).IsAssignableTo(typeof(ComparerV));*/
            Pipeline pip;
            try
            {
                pip = Pipeline.load(args[0]);

//                pip = pips[0];
            }  
            catch( Exception e77)
            {
                Console.WriteLine("Error:"+e77.Message);
                return -1;
            }
            pip.run().GetAwaiter().GetResult();
            return 0;
        }
    }
}

using ParserLibrary.TIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJsonRazbor
{
    public class TIPRecieverTests
    {
        int port = 5001;

        public static void Init()
        {
            var tipReciever = new TIPReceiver()
            {
                WorkDir = @"C:\TestTip", DelayTime = 5
            };


            tipReciever.stringReceived = (s, o) => tipReciever.sendResponseInternal(s, o);
            tipReciever.startInternal();
        }

    }
}

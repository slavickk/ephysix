using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public  class Mocker
    {
        
        public string MocBody="";
        public string MocFile;
        string MocContent;
        object syncro = new object();
        public int MocTimeoutInMilliseconds=0;
        public async Task<string> getMock ()
        {
            if(MocTimeoutInMilliseconds >0)
                await Task.Delay(MocTimeoutInMilliseconds);
            string input;
            if ((MocBody ?? "") != "")
            {
                return MocBody;
            }
            else
            if ((MocFile ?? "") != "")
            {
                if (MocContent == "")
                {
                    lock (syncro)
                    {
                        if (MocContent == "")
                        {

                            // string ans;
                            using (StreamReader sr = new StreamReader(MocFile))
                            {
                                MocContent = sr.ReadToEnd();
                                return MocContent;
                            }
                        }
                        else
                            return MocContent;

                    }
                    
                } else
                    return MocContent;
/*                using (StreamReader sr = new StreamReader(MocFile))
                {
                    return sr.ReadToEnd();
                }*/
            }
            else
                return "";
        }

    }
}

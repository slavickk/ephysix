using System.IO;
using System.Threading.Tasks;

namespace ParserLibrary;

public class TestReceiver:Receiver
{
    public string path;
    public string pattern="";



    public async override Task startInternal()
    {

        foreach (var file_name in ((pattern == "") ? new string[] { path } : Directory.GetFiles(path, pattern)))
        {
            using (StreamReader sr = new StreamReader(file_name))
            {
                var body = sr.ReadToEnd();
                await signal(body,null);

            }
        }

    }
}
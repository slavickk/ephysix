using System.IO;
using System.Threading.Tasks;

namespace ParserLibrary;

public class FileReceiver : Receiver
{
    string delim = "---------------------------RRRRR----------------------------------";

    public string file_name = @"C:\Data\scratch_1.txt";

    public async override Task startInternal()
    {
        int ind = 0;
        using (StreamReader sr = new StreamReader(file_name))
        {
            while (!sr.EndOfStream && ind < 50)
            {
                ind++;

                var line = sr.ReadLine();
                int pos = line.IndexOf(delim);
                if (pos >= 0)
                    line = line.Substring(0, pos);
                if (line != "")
                {
                    await signal(line,null);
                }
            }

        }
    }
}
using ParserLibrary;
using PluginBase;

namespace Plugins;

/// <summary>
/// A receiver that reads the given file and triggers a signal for each text chunk separated by the delimiter. 
/// </summary>
public class FileReceiver : IReceiver
{
    string delim = "---------------------------RRRRR----------------------------------";

    public string file_name = @"C:\Data\scratch_1.txt";

    public IReceiverHost host { get; set; }
    public Task sendResponse(string response, object context)
    {
        Logger.log($"FileReceiver.sendResponse called with response {response} and context {context}");
        return Task.CompletedTask;
    }

    public bool cantTryParse; // This one comes from the YAML definition of the receiver

    bool IReceiver.cantTryParse => this.cantTryParse; // This one is exposed to the host machinery

    public bool debugMode { get; set; }

    public async Task start()
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
                    await host.signal(line,null);
                }
            }

        }
    }
}
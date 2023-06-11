using System.IO;
using System.Threading.Tasks;
using PluginBase;

namespace Plugins;

/// <summary>
/// A drop-in replacement for the TestReceiver that uses the new IReceiver interface.
/// </summary>
public class TestReceiver : IReceiver
{
    public string path;
    public string pattern = "";

    public IReceiverHost host { get; set; }

    public bool cantTryParse { get; set; }

    public bool debugMode { get; set; }

    public async Task start()
    {
        foreach (var file_name in ((pattern == "") ? new string[] { path } : Directory.GetFiles(path, pattern)))
        {
            using (StreamReader sr = new StreamReader(file_name))
            {
                var body = sr.ReadToEnd();
                await host.signal(body, null);
            }
        }
    }

    public Task sendResponse(string response, object context)
    {
        throw new System.NotImplementedException("The TestReceiver is not supposed to send responses");
    }
}
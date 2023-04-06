namespace PluginBase;

public interface IReceiver
{
    Task start();
    IReceiverHost host { get; set; }
    Task sendResponse(string response, object context);
    bool cantTryParse { get; }
    bool debugMode { get; set; }
}

public interface IReceiverHost
{
    Task signal(string input, object context);
}

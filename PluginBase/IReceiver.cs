namespace PluginBase;

/// <summary>
/// A receiver is a component that receives messages from an external source and sends them
/// to the owning step through the receiver host.
/// </summary>
public interface IReceiver
{
    Task start();
    IReceiverHost host { get; set; }
    Task sendResponse(string response, object context);
    bool cantTryParse { get; }
    bool debugMode { get; set; }
}

/// <summary>
/// A receiver host is part of a step and is responsible for handling the input messages from the receiver.
/// </summary>
public interface IReceiverHost
{
    /// <summary>
    /// Handle an input message from receiver.
    /// </summary>
    /// <param name="input">Input message</param>
    /// <param name="context">Arbitrary context object</param>
    /// <returns></returns>
    Task signal(string input, object context);
    string IDStep { get; }
    int MaxConcurrentConnections { get; }
    int ConnectionTimeoutInMilliseconds { get; }


}

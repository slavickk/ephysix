using System.Collections.Generic;
using System.Threading.Tasks;
using PluginBase;

namespace Plugins;

/// <summary>
/// Dummy sender that doesn't send anything anywhere, and returns a predefined response.
/// This is an ISender-based counterpart to the legacy DummyReceiver. 
/// </summary>
public class DummySender : ISender
{
    public TypeContent typeContent => TypeContent.json;
    public void Init()
    {
        ParserLibrary.Logger.log("Plugins.DummySender.Init() called.");
    }

    /// <summary>
    /// Dictionary of dummy responses to return, indexed by string keys.
    /// </summary>
    public Dictionary<string, string> DummyResponses;

    /// <summary>
    /// The key of the dummy response to return.
    /// </summary>
    public string ResponseToReturn;
    
    public ISenderHost host { get; set; }

    public Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
    {
        return Task.FromResult(this.DummyResponses[this.ResponseToReturn]);
    }

    public Task<string> send(string JsonBody, ContextItem context)
    {
        return Task.FromResult(this.DummyResponses[this.ResponseToReturn]);
    }

    public string getTemplate(string key)
    {
        return string.Empty;
    }

    public void setTemplate(string key, string body)
    {
    }
}
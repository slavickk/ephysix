using System.Collections.Generic;
using System.Threading.Tasks;
using PluginBase;

namespace ParserLibrary;

/// <summary>
/// Dummy sender that doesn't send anything anywhere, and returns a predefined response.
/// </summary>
public class DummySender : Sender
{
    public override TypeContent typeContent => TypeContent.json;

    /// <summary>
    /// Dictionary of dummy responses to return, indexed by string keys.
    /// </summary>
    public Dictionary<string, string> DummyResponses;

    /// <summary>
    /// The key of the dummy response to return.
    /// </summary>
    public string ResponseToReturn;

    public override Task<string> send(string JsonBody, ContextItem context)
    {
        return Task.FromResult(this.DummyResponses[this.ResponseToReturn]);
    }
}
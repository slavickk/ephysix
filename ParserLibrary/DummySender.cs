using System.Threading.Tasks;

namespace ParserLibrary;

/// <summary>
/// Dummy sender that doesn't send anything anywhere, and returns a predefined response.
/// </summary>
public class DummySender : Sender
{
    public override TypeContent typeContent => TypeContent.json;

    /// <summary>
    /// The dummy response to return.
    /// </summary>
    public string DummyResponse;

    public override Task<string> send(string JsonBody, Step.ContextItem context)
    {
        return Task.FromResult(this.DummyResponse);
    }
}
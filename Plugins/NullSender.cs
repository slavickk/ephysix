using PluginBase;
using UniElLib;

namespace Plugins;

/// <summary>
/// A sender that does absolutely nothing, even less than DummySender.
/// This implementation is here for completeness only, as it was present in the original codebase.
/// </summary>
[Annotation("Пустой Sender")]
public class NullSender : ISender
{
    public ISenderHost host { get; set; }
    public async Task<string> send(AbstrParser.UniEl root, ContextItem context)
    {
        return string.Empty;
    }

    public async Task<string> send(string JsonBody, ContextItem context)
    {
        return string.Empty;
    }

    public string getTemplate(string key)
    {
        return string.Empty;
    }

    public void setTemplate(string key, string body)
    {
    }

    public TypeContent typeContent => TypeContent.internal_list;
    public void Init()
    {
        ParserLibrary.Logger.log("Plugins.NullSender.Init() called.");
    }
}
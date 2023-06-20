using System.Diagnostics;
using PluginBase;
using UniElLib;

public enum TypeContent { internal_list,json};

public interface ISender
{
    ISenderHost host { get; set; }
    
    // TODO: the real type of context below is Step.ContextItem, but we can't reference Step here; refactor.
    
    Task<string> send(AbstrParser.UniEl root, ContextItem context);
    Task<string> send(string JsonBody, ContextItem context);

    string getTemplate(string key);

    void setTemplate(string key, string body);
    
    TypeContent typeContent { get; }

    string getExample() => string.Empty;

    void Init();
}

public interface ISenderHost
{
    string IDStep { get; }
    Activity sendActivity { get; }
    
    /// <summary>
    /// Saves context in the pipeline.
    /// Can be implemented by calling the pipeline's SaveContext method.
    /// </summary>
    /// <param name="body"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    string SavePipelineContext(string body, string extension = "txt");
}


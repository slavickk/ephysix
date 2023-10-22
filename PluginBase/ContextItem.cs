using System.Diagnostics;

namespace PluginBase;

public class ContextItem
{
    public List<UniElLib.AbstrParser.UniEl> list = new List<UniElLib.AbstrParser.UniEl>();
    public object context;
    public Activity mainActivity;
    public int increment;
//    public Scenario currentScenario = null;

    public string GetPrefix(string context)
    {
        return $"a{increment}_{context}";
    }


}

using UniElLib;

namespace PluginBase;

public class TemplateSenderOutputValue : OutputValue
{
    string templ;
    AbstrParser.UniEl rootElement;

    public string templateBody
    {
        get { return ownerSender.getTemplate(key); }
        set
        {
            ownerSender.setTemplate(key, value);
            SetTemplate(value);
        }
    }

    private void SetTemplate(string value)
    {
        templ = value;
        rootElement = AbstrParser.ParseString(templ);
    }


    ISender ownerSender;
    string key;

    public TemplateSenderOutputValue(ISender sender, string key1)
    {
        key = key1;
        ownerSender = sender;
    }

    public override bool canReturnObject => false;

    public override bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot, ContextItem context)
    {
        foreach (var el in rootElement.childs)
            el.copy(outputRoot);
        return true;
        //            return base.addToOutput(inputRoot, ref outputRoot);
    }

    public override object getValue(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
    {
        return null;
    }

    public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl,ContextItem context)
    {
        return new AbstrParser.UniEl[] { null };
    }
}

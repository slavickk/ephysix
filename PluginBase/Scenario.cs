namespace PluginBase;

/// <summary>
///  Scenario item for Pipeline
/// </summary>
public  class Scenario
{
    public string Description { get; set; }
    public List<Item> mocs { get; set; }=new List<Item>();
    public override string ToString()
    {
        return Description;
    }
    public Item getStepItem(string idStep)
    {
        var stepItem = mocs.FirstOrDefault(ii => ii.IDStep == idStep);
        if (stepItem == null) 
        {
            stepItem = new Item() { IDStep = idStep };
            mocs.Add(stepItem);
        }
        return stepItem;
    }
    public class Item
    { 
        public string IDStep { get; set; }
//            public bool isMocReceiverEnabled { get; set; }
        public string MocFileReceiver { get; set; }
//            public bool isMocSenderEnabled { get; set; }
        public string MocFileResponce { get; set; }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public abstract class Sender
{
    [YamlIgnore]

    protected Metrics.MetricHistogram metricUpTime;
    [YamlIgnore]

    private Metrics.MetricHistogram metricUpTimeError;
    public virtual void Init(Pipeline owner)
    {
        metricUpTimeError = new Metrics.MetricHistogram("iu_outbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });

        metricUpTime = new Metrics.MetricHistogram("iu_outbound_request_duration_msec", "handle performance receiver");
        metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
    }


    public override string ToString()
    {
        return $"Sender:{this.GetType().Name} Step:{owner?.IDStep}";
    }
    public virtual  string getExample()
    {
        return "";
    }

    public virtual string getTemplate(string key)
    {
        return "";
    }

    public virtual void setTemplate(string key,string body)
    {
    }



    [YamlIgnore]
    public bool MocMode = false;
    public string MocFile;
    public string MocBody="";

    [YamlIgnore]
    public Step owner;
    public enum TypeContent { internal_list,json};
    [YamlIgnore]
    public abstract TypeContent typeContent
    {
        get;
    }
    //  public string IDResponsedReceiverStep = "";
    string MocContent = "";
    object syncro= new object();
    public async Task<string> send(AbstrParser.UniEl root)
    {
        DateTime time1 = DateTime.Now;
        string ans;
        try
        {
            if (!MocMode)
            {
                if (typeContent == TypeContent.internal_list)
                    ans = await sendInternal(root);
                else
                    ans = await send(root.toJSON());
            }
            else
            {
                // await Task.Delay(10);
                if ((MocBody ?? "") != "")
                    ans = MocBody;
                else
                {
                    if (MocContent == "")
                    {
                        lock (syncro)
                        {
                            if (MocContent == "")
                            {

                                // string ans;
                                using (StreamReader sr = new StreamReader(MocFile))
                                {
                                    MocContent = sr.ReadToEnd();
                                    //       return ans;
                                }
                            }
                        }
                    }
                    ans = MocContent;
                }
            }
            if (owner.debugMode)
                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information, this, root.toJSON(), ans);
            metricUpTime.Add(time1);
        }
        catch (Exception ex)
        {
            metricUpTimeError.Add(time1);
            throw;
        }
        return ans;
    }
    public async virtual Task<string> sendInternal(AbstrParser.UniEl root)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }
    public async virtual Task<string> send(string JsonBody)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static ParserLibrary.Step;

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
        metricUpTimeError.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });

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
    protected Activity sendActivity;
    public async Task<string> send(AbstrParser.UniEl root, ContextItem context)
    {
        DateTime time1 = DateTime.Now;
        string ans;
        sendActivity = owner.owner.GetActivity($"Send{this.GetType().Name}", context?.mainActivity);
        sendActivity?.AddTag("typeSender", this.GetType().Name);
        try
        {
            if (!MocMode)
            {
                if (typeContent == TypeContent.internal_list)
                    ans = await sendInternal(root,context);
                else
                    ans = await send(root.toJSON(),context);
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
            sendActivity?.AddTag("errorSend", "true");
            sendActivity?.Dispose();
            metricUpTimeError.Add(time1);
            throw;
        }
        sendActivity?.Dispose();
        return ans;
    }
    public async virtual Task<string> sendInternal(AbstrParser.UniEl root,ContextItem context)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }
    public async virtual Task<string> send(string JsonBody,ContextItem context)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }
}
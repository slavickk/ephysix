using System;
using System.Threading.Tasks;

using Confluent.Kafka;
using ParserLibrary;
using PluginBase;
using Serilog.Events;
using UniElLib;

namespace Plugins.Kafka;

public class KafkaSender : Sender, IDisposable, ISelfTested, ISender
{
    public override TypeContent typeContent =>  TypeContent.internal_list;
    public string BootstrapServers;

    /// <summary>
    /// Kafka topic to send messages to
    /// </summary>
    public string Topic;    
    private ISenderHost _host;
    private TypeContent _typeContent;
    private IProducer<Null, string>? _producer;

    ISenderHost ISender.host
    {
        get => _host;
        set => _host = value;
    }

    override  public  Task<string> sendInternal(AbstrParser.UniEl root, ContextItem context)
    {
        // TODO: do we need to use context here?
        return sendMessage(root.toJSON());
    }


    public async Task<string> sendMessage(string message)
    {
        if (_producer == null)
            throw new InvalidOperationException("KafkaSender is not initialized");
        
        var deliveryResult = await _producer.ProduceAsync(this.Topic, new Message<Null, string> { Value = message });
        Logger.log($"Sent message to partition {deliveryResult.Partition} with offset {deliveryResult.Offset}. Status is {deliveryResult.Status}", LogEventLevel.Debug);

        // DEBUG: this was to test Serilog enrichment with exception details - should be removed eventually
        // throw new KafkaException("Test exception without any error", deliveryResult);

        return deliveryResult.Status switch
        {
            PersistenceStatus.NotPersisted => throw new KafkaException("Message not delivered", deliveryResult: deliveryResult),
            PersistenceStatus.PossiblyPersisted => throw new KafkaException("Message may have been lost", deliveryResult: deliveryResult),
            // TODO: reconsider the return value. What should we return after successfully sending a message to Kafka?
            PersistenceStatus.Persisted => deliveryResult.Status.ToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override string getTemplate(string key)
    {
        return "";
        //throw new NotImplementedException();
    }

    public override void setTemplate(string key, string body)
    {
        throw new NotImplementedException();
    }

    //TypeContent ISender.typeContent => _typeContent;
    
    public override void Init(Pipeline owner)
    {
        base.Init(owner);
        ((ISender)this).Init();
    }

    void ISender.Init()
    {
        if (_producer != null)
            _producer.Dispose();
        
        _producer = new ProducerBuilder<Null, string>(
            new ProducerConfig
            {
                BootstrapServers = this.BootstrapServers
            }).Build();
    }

    public void Dispose()
    {
        _producer?.Dispose();
        _producer = null;
    }

    public async Task<(bool, string, Exception)> isOK()
    {
        string details;
        bool isSuccess = true;
        Exception exc = null;
        details = "Make kafka selfTest";
        try
        {
           // ((ISender)this).Init();

            DateTime time1 = DateTime.Now;
            var deliveryResult = await _producer.ProduceAsync(this.Topic, new Message<Null, string> { Value = "{}" });
            Logger.log($"Sent message to partition {deliveryResult.Partition} with offset {deliveryResult.Offset}. Status is {deliveryResult.Status}", LogEventLevel.Debug);
            if (deliveryResult.Status != PersistenceStatus.Persisted && deliveryResult.Status != PersistenceStatus.PossiblyPersisted)
                isSuccess = false;
        }
        catch (Exception e77)
        {
            isSuccess = false;
            exc = e77;
        }
        //            if(ans)
        return (isSuccess, details, exc);
    }

}
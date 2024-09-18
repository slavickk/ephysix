using System;
using System.Threading.Tasks;

using Confluent.Kafka;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using ParserLibrary;
using PluginBase;
using Serilog.Events;
using UniElLib;

namespace Plugins.Kafka;

public class IKafkaSender : ISender, IDisposable
{
    public string BootstrapServers { get; set; }
    
    /// <summary>
    /// Kafka topic to send messages to
    /// </summary>
    public string Topic { get; set; }
    
    private ISenderHost _host;
    private TypeContent _typeContent;
    private IProducer<Null, string>? _producer;

    ISenderHost ISender.host
    {
        get => _host;
        set => _host = value;
    }

    Task<string> ISender.send(AbstrParser.UniEl root, ContextItem context)
    {
        // TODO: do we need to use context here?
        return sendMessage(root.toJSON());
    }

    Task<string> ISender.send(string JsonBody, ContextItem context)
    {
        // TODO: do we need to use context here?
        return sendMessage(JsonBody);
    }
    public async Task<(bool, string, Exception)> isOK()
    {
        string details;
        bool isSuccess = true;
        Exception exc = null;
        details = "Make kafka selfTest" ;
        try
        {
            DateTime time1 = DateTime.Now;
            var deliveryResult = await _producer.ProduceAsync(this.Topic, new Message<Null, string> { Value = "{}" });
            Logger.log($"Sent message to partition {deliveryResult.Partition} with offset {deliveryResult.Offset}. Status is {deliveryResult.Status}", LogEventLevel.Debug);
            if(deliveryResult.Status != PersistenceStatus.Persisted && deliveryResult.Status != PersistenceStatus.PossiblyPersisted)
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

    private async Task<string> sendMessage(string message)
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

    string ISender.getTemplate(string key)
    {
        throw new NotImplementedException();
    }
    
    void ISender.setTemplate(string key, string body)
    {
        throw new NotImplementedException();
    }

    TypeContent ISender.typeContent => _typeContent;

    void ISender.Init()
    {
        if (_producer != null)
            throw new InvalidOperationException("The KafkaSender is already initialized");
        
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
}
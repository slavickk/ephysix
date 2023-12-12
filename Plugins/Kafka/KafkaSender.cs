using Confluent.Kafka;
using ParserLibrary;
using PluginBase;
using Serilog.Events;
using UniElLib;

namespace Plugins.Kafka;

public class KafkaSender : ISender, IDisposable
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

    private async Task<string> sendMessage(string message)
    {
        if (_producer == null)
            throw new InvalidOperationException("KafkaSender is not initialized");
        
        var deliveryReport = await _producer.ProduceAsync(this.Topic, new Message<Null, string> { Value = message });
        Logger.log($"Sent message to partition {deliveryReport.Partition} with offset {deliveryReport.Offset}. Status is {deliveryReport.Status}", LogEventLevel.Debug);

        return deliveryReport.Status switch
        {
            // TODO: more specific error?
            PersistenceStatus.NotPersisted => throw new Exception("Message not delivered"),
            // TODO: what should we do in this case?
            PersistenceStatus.PossiblyPersisted => throw new Exception("Message may have been lost"),
            // TODO: reconsider the return value. What should we return after successfully sending a message to Kafka?
            PersistenceStatus.Persisted => deliveryReport.Status.ToString(),
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
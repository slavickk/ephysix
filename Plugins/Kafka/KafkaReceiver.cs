using Confluent.Kafka;
using ParserLibrary;
using PluginBase;

namespace Plugins.Kafka;

public class KafkaReceiver : IReceiver, IDisposable
{
    public KafkaReceiver()
    {
        Console.WriteLine("constructor");
    }
    
    public string BootstrapServers { get; set; }
    
    /// <summary>
    /// Kafka topic to receive messages from
    /// </summary>
    public string Topic { get; set; }
    
    /// <summary>
    /// Kafka consumer group name
    /// </summary>
    public string ConsumerGroup { get; set; } = "default-group";
    
    private IConsumer<Ignore, string>? _consumer;
    private CancellationTokenSource cts;
    
    private IReceiverHost _host;
    private bool _cantTryParse;
    private bool _debugMode;
    
    Task IReceiver.start()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = this.BootstrapServers,
            GroupId = this.ConsumerGroup,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(Topic);
    
        cts = new CancellationTokenSource();
        
        var tcs = new TaskCompletionSource();
        
        var consumerTask = new Thread(() => receiveLoop(tcs));
        consumerTask.Start();
        
        return tcs.Task;
    }
    
    private async void receiveLoop(TaskCompletionSource tcs)
    {
        if (_consumer == null)
            throw new InvalidOperationException("_consumer is null");

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var message = _consumer.Consume(TimeSpan.FromSeconds(5));

                if (message != null)
                {
                    Console.WriteLine($"Received message: {message.Message.Value}");
                    
                    // TODO: what should we respond to Kafka?
                    
                    await _host.signal(message.Message.Value, "hz-context");
                    
                    // TODO: and how?
                }
                else
                {
                    Console.WriteLine("Nothing has been received from Kafka in 5 seconds.");
                }
            }
        }
        catch (OperationCanceledException e)
        {
            // Expected when the cancellation token is triggered.
            Console.WriteLine($"Operation cancelled: {e.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
        finally
        {
            _consumer.Close();
            tcs.SetResult();
        }
    }
    
    
    IReceiverHost IReceiver.host
    {
        get => _host;
        set => _host = value;
    }
    
    Task IReceiver.sendResponse(string response, object context)
    {
        // Does it make sense to send a response back to Kafka?
        // TODO: figure out how do we mark the message as processed in Kafka? Should this be here?
        Logger.log($"KafkaReceiver.IReceiver.sendResponse: {response}");
        return Task.CompletedTask;
    }
    
    bool IReceiver.cantTryParse => _cantTryParse;
    
    bool IReceiver.debugMode
    {
        get => _debugMode;
        set => _debugMode = value;
    }
    
    public void Dispose()
    {
        _consumer?.Dispose();
        _consumer = null;
    }
 
}
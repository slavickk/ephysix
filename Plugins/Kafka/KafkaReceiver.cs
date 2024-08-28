using Confluent.Kafka;
using ParserLibrary;
using PluginBase;
using Serilog.Events;
using UniElLib;

namespace Plugins.Kafka;

// TODO: move Kafka plugins to the main project, ParserLibrary
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
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
    
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(Topic);
    
        cts = new CancellationTokenSource();
        
        var tcs = new TaskCompletionSource();
        
        var consumerTask = new Thread(() => receiveLoop(tcs));
        consumerTask.Start();
        
        return tcs.Task;
    }
    
    public Task stop()
    {
        cts.Cancel();
        return Task.CompletedTask;
    }

    private record KafkaReceiverContext(ConsumeResult<Ignore, string> Message);
    
    private async void receiveLoop(TaskCompletionSource tcs)
    {
        if (_consumer == null)
            throw new InvalidOperationException("_consumer is null");

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var message = _consumer.Consume(TimeSpan.FromSeconds(2));

                if (message == null)
                {
                    // yield control to other tasks
                    await Task.Delay(100);
                    continue;
                }
                
                Console.WriteLine($"Received message: {message.Message.Value}");
                await _host.signal(message.Message.Value, new KafkaReceiverContext(message));
                _consumer.Commit(message);
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
        if (_consumer == null)
            throw new InvalidOperationException("_consumer is null");
        
        if (context is not ContextItem ctx)
            throw new ArgumentException(
                $"Expected context of type {nameof(ContextItem)} but got {context?.GetType().Name ?? "null"}");

        if (ctx.context is not KafkaReceiverContext krctx)
            throw new ArgumentException(
                $"Expected ContextItem.context of type {nameof(KafkaReceiverContext)} but got {ctx.context?.GetType().Name ?? "null"}");
        
        // Assume that "sending a response back to Kafka" means "committing the message".
        // Actual response content is ignored.
        Logger.log($"KafkaReceiver.IReceiver.sendResponse: committing the message with key {krctx.Message.Message.Key}", LogEventLevel.Debug);
        _consumer.Commit(krctx.Message);
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
 
    /// <summary>
    /// Body of the mock response to return.
    /// If set, the receiver will return this body instead of waiting for requests.
    /// </summary>
    string IReceiver.MocBody { 
        get
        {
            return mockBody; 
        }
        set
        {
            mockBody = value;
        }
    }
    public string mockBody;

    /// <summary>
    /// File containing the mock response to return.
    /// If set, the receiver will return the contents of this file instead of waiting for requests.
    /// </summary>
    public string mockFile;

    string IReceiver.MocFile
    {
        get => mockFile;
        set => mockFile = value;
    }

    bool IReceiver.MocMode { get; set; }=false;
}
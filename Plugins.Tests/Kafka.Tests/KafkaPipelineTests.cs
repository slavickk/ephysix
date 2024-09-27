using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Plugins.Kafka;
using Serilog;
using Serilog.Exceptions;
using Testcontainers.Kafka;

namespace ParserLibrary.Tests;

/// <summary>
/// These tests verify that the pipeline can consume messages from a Kafka topic (pre-arranged by the test)
/// and send them to another Kafka topic.
/// Kafka is provided by Testcontainers.
/// The pipeline definition is constructed dynamically to include address of the temporary Kafka instance.
/// After the test is executed, assertions are run to check that the destination topic contains the expected messages.
/// </summary>
[TestFixture]
public class KafkaPipelineTests
{
    private IProducer<Null, string>? _producer;
    private IConsumer<Null, string>? _consumer;
    
    private KafkaContainer _container;
    private Pipeline _pipeline;

    [OneTimeSetUp]
    public async Task InitAsync()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        _container = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.6.1")
            .Build();
        
        Console.WriteLine("Starting Kafka container...");

        await _container.StartAsync();
        
        Console.WriteLine("Creating topics...");
        
        var config = new AdminClientConfig { BootstrapServers = _container.GetBootstrapAddress() };
        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            await adminClient.CreateTopicsAsync(new TopicSpecification[]
            {
                new() { Name = "topic1", NumPartitions = 1 },
                new() { Name = "topic2", NumPartitions = 1 }
            });
            Console.WriteLine("Topics created successfully.");
        }
        
        Console.WriteLine("Creating producer and consumer...");
        _producer = new ProducerBuilder<Null, string>(
            new ProducerConfig
            {
                BootstrapServers = _container.GetBootstrapAddress(),
                Acks = Acks.All
            }).Build();
        
        _consumer = new ConsumerBuilder<Null, string>(
            new ConsumerConfig
            {
                BootstrapServers = _container.GetBootstrapAddress(),
                GroupId = "g1",
                ClientId = "Test Client ID",
                AutoOffsetReset = AutoOffsetReset.Earliest
            }).Build();
        _consumer.Subscribe("topic1");
        
        Console.WriteLine("Kafka setup completed.");
        
        Console.WriteLine("Creating pipeline...");
        // Create a pipeline that consumes (from topic1) and produces (to topic2) messages from and to the above Kafka instance
        // The pipeline definition is constructed dynamically to include the address of the temporary Kafka instance
        var pipelineDefinition = $@"
pipelineDescription: Pipeline example
steps:
- isBridge: true
  isHandleSenderError: true
  saveErrorSendDirectory: Data/Restore
  iDStep: Step_0
  iDPreviousStep: ''
  iDResponsedReceiverStep: ''
  description: Check registration extract
  ireceiver: !Plugins.Kafka.KafkaReceiver
      topic: topic1
      bootstrapServers: {_container.GetBootstrapAddress()}
  isender: !Plugins.Kafka.KafkaSender
      topic: topic2
      bootstrapServers: {_container.GetBootstrapAddress()}
";
        
        _pipeline = Pipeline.loadFromString(pipelineDefinition, typeof(KafkaReceiver).Assembly);
        
        var builder = new ConfigurationBuilder().AddEnvironmentVariables(prefix: "");
        var configuration = builder.Build();
        Pipeline.configuration = configuration;
    }
    
    [OneTimeTearDown]
    public async Task TeardownAsync()
    {
        Console.WriteLine($"Stopping the producer... {DateTime.Now}");
        _producer?.Dispose();
        _producer = null;
        
        Console.WriteLine($"Stopping the consumer... {DateTime.Now}");
        _consumer?.Dispose();
        _consumer = null;

        Console.WriteLine($"Stopping the Kafka container... {DateTime.Now}");
        // TODO: figure out why this takes 15 seconds
        await _container.StopAsync();
        
        Console.WriteLine($"Kafka container stopped. Teardown completed. {DateTime.Now}");
    }
    
    /// <summary>
    /// Add exception logging to the task. If the task fails, the exception will be logged using Log.Error.
    /// </summary>
    /// <param name="task"></param>
    private static void AddExceptionLogging(Task task)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && t.Exception != null)
                Log.Error(t.Exception, "Pipeline execution failed");
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    [Test]
    public async Task TestPipelineExecution()
    {
        // Arrange
        // Produce messages to the source topic
        Console.WriteLine("Producing messages to topic1...");
        await _producer.ProduceAsync("topic1", new Message<Null, string> { Value = "message1" });
        await _producer.ProduceAsync("topic1", new Message<Null, string> { Value = "message2" });
        
        var kr = (KafkaReceiver)_pipeline.steps[0].ireceiver;
        
        // Act

        // deliberately don't await
        Console.WriteLine("Running the pipeline...");
        AddExceptionLogging(_pipeline.run());

        // Consume messages from the destination topic `topic2`
        var result1 = _consumer.Consume(TimeSpan.FromSeconds(10));
        var result2 = _consumer.Consume(TimeSpan.FromSeconds(10));
            
        Console.WriteLine("Stopping the receiver...");
        await kr.stop();
        Console.WriteLine("Receiver stopped.");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result1);
            Assert.That(result1?.Message.Value, Is.EqualTo("message1"));
            Assert.NotNull(result2);
            Assert.That(result2?.Message.Value, Is.EqualTo("message2"));
        });
    }
}

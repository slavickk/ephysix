using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Testcontainers.Kafka;

namespace ParserLibrary.Tests;

[TestFixture]
public class KafkaProducerConsumerTests
{
    private IProducer<Null, string>? _producer;
    private IConsumer<Null, string>? _consumer;
    
    private KafkaContainer _container;

    [OneTimeSetUp]
    public async Task InitAsync()
    {
        _container = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.6.1")
            .Build();

        await _container.StartAsync();
        
        var config = new AdminClientConfig { BootstrapServers = _container.GetBootstrapAddress() };
        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            const string topicName = "topic1";
            await adminClient.CreateTopicsAsync(new TopicSpecification[]
            {
                new() { Name = topicName, NumPartitions = 4 }
            });
            Console.WriteLine("Topic created successfully.");
        }
        
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
    }
    
    [OneTimeTearDown]
    public async Task Teardown()
    {
        _producer?.Dispose();
        _producer = null;
        
        _consumer?.Dispose();
        _consumer = null;
        
        await _container.DisposeAsync();
    }

    [Test]
    public async Task TestProducingAndConsumingMessageAsync()
    {
        var message = $"Message created at {DateTime.Now}";
        var deliveryReport = await _producer.ProduceAsync(
            "topic1",
            new Message<Null, string>()
            {
                Value = message
            });
        Assert.NotNull(deliveryReport);
        
        var result = _consumer.Consume(TimeSpan.FromSeconds(5));
        Assert.NotNull(result);
        Assert.AreEqual(message, result.Message.Value);
    }
    
    [Test]
    public void TestProducingAndConsumingMultipleMessagesAsync()
    {
        var message1 = $"Message 1 created at {DateTime.Now}";
        var message2 = $"Message 2 created at {DateTime.Now}";
        
        var deliveryReport1 = _producer.ProduceAsync(
            "topic1",
            new Message<Null, string>()
            {
                Value = message1
            });
        Assert.NotNull(deliveryReport1);
        
        var deliveryReport2 = _producer.ProduceAsync(
            "topic1",
            new Message<Null, string>()
            {
                Value = message2
            });
        Assert.NotNull(deliveryReport2);
        
        var result1 = _consumer.Consume(TimeSpan.FromSeconds(5));
        Assert.NotNull(result1);
        Assert.AreEqual(message1, result1.Message.Value);
        
        var result2 = _consumer.Consume(TimeSpan.FromSeconds(5));
        Assert.NotNull(result2);
        Assert.AreEqual(message2, result2.Message.Value);
    }
}
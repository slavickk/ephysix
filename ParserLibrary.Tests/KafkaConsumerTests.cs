using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;

namespace ParserLibrary.Tests;

[TestFixture]
public class KafkaConsumerTests
{
    private IConsumer<Null, string>? _consumer;

    [OneTimeSetUp]
    public void Init()
    {
        _consumer = new ConsumerBuilder<Null, string>(
            new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "g1",
            }).Build();
        _consumer.Subscribe("topic1");
    }

    [Test]
    public void CheckConnection()
    {
        Assert.NotNull(_consumer);
    }

    [Test]
    public void TestConsumingMessageAsync()
    {
        var result = _consumer.Consume(TimeSpan.FromSeconds(5));
        Assert.NotNull(result);
    }
    
    [OneTimeTearDown]
    public void Teardown()
    {
        _consumer.Dispose();
        _consumer = null;
    }
}

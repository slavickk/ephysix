using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;

namespace ParserLibrary.Tests;

[TestFixture]
public class KafkaProducerTests
{
    private IProducer<Null, string>? _producer;

    [OneTimeSetUp]
    public void Init()
    {
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build();
    }

    [Test]
    public void CheckConnection()
    {
        Assert.NotNull(_producer);
    }

    [Test]
    public async Task TestProducingMessageAsync()
    {
        var deliveryReport = await _producer.ProduceAsync(
            "topic1",
            new Message<Null, string>()
            {
                Value = $"Message created at {DateTime.Now}"
            });
        
        Assert.NotNull(deliveryReport);
    }
    
    [OneTimeTearDown]
    public void Teardown()
    {
        _producer.Dispose();
        _producer = null;
    }
}

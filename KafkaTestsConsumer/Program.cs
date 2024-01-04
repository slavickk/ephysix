// ConsumerApp Program.cs

using Confluent.Kafka;

var consumerConfig = new ConsumerConfig
{
    GroupId = "test-group",
    BootstrapServers = "localhost:9092",
    EnableAutoCommit = false,
    SessionTimeoutMs = 10000
};

Console.WriteLine($"Consumer started. Arguments: {string.Join(", ", args)}");

// The consumer deliberately is not disposed to keep the connection open.
// This is to simulate the situation when the consumer crashes (in our case, it's killed by the parent process).
var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

Console.WriteLine("Consumer created");

consumer.Subscribe("test-topic");

Console.WriteLine("Consumer subscribed");

ConsumeResult<Ignore, string>? consumeResult = null;

// consumeResult = consumer.Consume(TimeSpan.FromSeconds(40));
for (var i = 0; i < 20; i++)
{
    Console.WriteLine($"Waiting for a message. Iteration {i}...");
    consumeResult = consumer.Consume(TimeSpan.FromSeconds(40));
    if (consumeResult != null)
    {
        Console.WriteLine($"Message received: {consumeResult.Message.Value}");
        Console.WriteLine(consumeResult.Message.Value);
        break;
    }

    Console.WriteLine($"Consume() returned null after iteration {i}");
}

if (consumeResult == null)
{
    Console.WriteLine("No message received after 20 iterations");
    throw new Exception("No message received");
}

// Don't commit the message to Kafka
Console.WriteLine("Sleeping and waiting for the parent process to kill us...");
Thread.Sleep(TimeSpan.FromMinutes(1));  // Give the parent process enough time to kill us
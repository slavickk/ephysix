/******************************************************************
 * File: KafkaTests.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System.Diagnostics;
using Confluent.Kafka;

namespace KafkaTests;

[TestFixture]
public class KafkaTests
{
    private const string BootstrapServers = "localhost:9092";
    private const string Topic = "test-topic";
    private const string GroupId = "test-group";
    
    /// <summary>
    /// This test checks that failed consumers don't commit messages to Kafka,
    /// and those messages remain in the topic and can be consumed by other consumers.
    /// The test assumes that the ConsumerApp is built and available in the KafkaTestsConsumer folder,
    /// and the Kafka broker is running on localhost:9092.
    /// </summary>
    [Test]
    public void TestUncommittedMessagesRemainInTopic()
    {
        // Run a simple consumer loop to ensure that the topic contains no old messages,
        // which may have been produced by previous test runs.
        var consumerConfig = new ConsumerConfig
        {
            GroupId = GroupId,
            BootstrapServers = BootstrapServers,
            EnableAutoCommit = false
        };
        
        using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
        {
            consumer.Subscribe(Topic);

            // Run the loop. There may be multiple messages.
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(10));
            while (consumeResult != null)
            {
                Console.WriteLine($"Skipped old message: {consumeResult.Message.Value}");
                consumer.Commit(consumeResult);
                consumeResult = consumer.Consume(TimeSpan.FromSeconds(10));
            }
        }

        var config = new ProducerConfig { BootstrapServers = BootstrapServers };
        var testMessage = $"Test message {Guid.NewGuid().ToString()} created at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            producer.Produce(Topic, new Message<Null, string> { Value = testMessage });
            var queueLength = producer.Flush(TimeSpan.FromSeconds(10));
            Console.WriteLine($"Out queue length after producing the message: {queueLength}");
        }
        
        // Start the ConsumerApp as a separate process
        var consumerAppProcess = Process.Start(
            new ProcessStartInfo
            {
                FileName = "KafkaTestsConsumer",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        );
        
        if (consumerAppProcess == null)
            throw new Exception("Failed to start the ConsumerApp");
        
        consumerAppProcess.Start();

        // Wait for the ConsumerApp to start and consume the message
        // Thread.Sleep(TimeSpan.FromSeconds(20)); // sometimes it takes a while to receive the message

        var consumerLogOutput = new List<string>();
        var consumerReachedSleep = false;

        var t0 = DateTime.Now;
        while ((DateTime.Now - t0) < TimeSpan.FromSeconds(120))
        {
            var line = consumerAppProcess.StandardOutput.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Got null line from the ConsumerApp. Waiting for one second.");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            else
            {
                consumerLogOutput.Add(line);
                Console.WriteLine($"ConsumerApp output: {line}");
                
                // Once we get "Sleeping and waiting for the parent process to kill us...", we can stop and kill the ConsumerApp
                if (line.StartsWith("Sleeping and waiting for the parent process to kill us..."))
                {
                    consumerReachedSleep = true;
                    break;
                }

                if (line.StartsWith("No message received"))
                    Assert.Fail("ConsumerApp didn't receive the message");
            }
        }
        if (!consumerReachedSleep) Assert.Fail("ConsumerApp didn't reach the sleep state");
        
        consumerAppProcess.Kill();
        consumerAppProcess.WaitForExit();
        var t1 = DateTime.Now;
        Console.WriteLine($"ConsumerApp exited after {(t1 - t0).TotalSeconds} seconds");
        
        var consumedMessage = consumerLogOutput
            .First(l => l.StartsWith("Message received: "))["Message received: ".Length..]
            .Trim();
        
        Assert.That(consumedMessage, Is.EqualTo(testMessage));
        
        // Now we need to wait for the session.timeout.ms to expire and the ConsumerApp to be kicked out of the consumer group
        // by the Kafka broker. This is to make sure that the ConsumerApp doesn't commit the message.
        Thread.Sleep(TimeSpan.FromSeconds(11));
        
        using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
        {
            consumer.Subscribe(Topic);
            t0 = DateTime.Now;
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(20));
            t1 = DateTime.Now;
            Console.WriteLine($"Consumed message after {(t1 - t0).TotalSeconds} seconds");
            
            Assert.That(consumeResult, Is.Not.Null);
            consumedMessage = consumeResult.Message.Value;
            
            t0 = DateTime.Now;
            consumer.Commit(consumeResult);
            t1 = DateTime.Now;
            Console.WriteLine($"Committed message after {(t1 - t0).TotalSeconds} seconds");
        }

        Assert.That(consumedMessage, Is.EqualTo(testMessage));
    }
}
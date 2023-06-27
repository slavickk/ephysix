using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Threading;


namespace ParserLibrary
{

    public class KafkaHelper
    {
        private IConsumer<Ignore, string> consumer;
        private CancellationTokenSource cts;

        public string BootstrapServers { get; set; }
        public string Topic { get; set; }

        public KafkaHelper(string bootstrapServers, string topic)
        {
            BootstrapServers = bootstrapServers;
            Topic = topic;
        }

        public void StartReceiving()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = "my-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(Topic);

            cts = new CancellationTokenSource();

            var consumerTask = new Thread(() =>
            {
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        var message = consumer.Consume(cts.Token);
                        Console.WriteLine($"Received message: {message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when the cancellation token is triggered.
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                }
                finally
                {
                    consumer.Close();
                }
            });

            consumerTask.Start();
        }

        public void StopReceiving()
        {
            cts?.Cancel();
        }

        public void SendMessage(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = BootstrapServers }).Build())
            {
                try
                {
                    var deliveryReport = producer.ProduceAsync(Topic, new Message<Null, string> { Value = message }).GetAwaiter().GetResult();
                    Console.WriteLine($"Sent message to partition {deliveryReport.Partition} with offset {deliveryReport.Offset}");
                }
                catch (ProduceException<Null, string> ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                }
            }
        }
    }
}

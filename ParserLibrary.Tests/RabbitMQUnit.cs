using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Tests
{
 
        [TestFixture]
        public class RabbitMQReceiverPerformanceTests
        {
            private const string Hostname = "localhost";
            private const int Port = 5672;
            private const string Username = "guest";
            private const string Password = "guest";
            private const string QueueName = "test_queue";

            private RabbitMQHelper receiver;
            private ConnectionFactory factory;
            private IConnection connection;
            private IModel channel;
            private EventingBasicConsumer consumer;

            [SetUp]
            public void Setup()
            {
                receiver = new RabbitMQHelper(Hostname, Port, Username, Password, QueueName);
                factory = new ConnectionFactory
                {
                    HostName = Hostname,
                    Port = Port,
                    UserName = Username,
                    Password = Password
                };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }

            [TearDown]
            public void TearDown()
            {
                receiver.StopReceiving();
                channel.Close();
                connection.Close();
            }

            [Test]
            public async Task PerformanceTest_MessageProcessingWithinThreshold()
            {
                // Arrange
                const int messageCount = 10000;
                const string message = "Test message";
                int receivedCount = 0;

                receiver.StartReceiving();

                consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var receivedMessage = Encoding.UTF8.GetString(body);

                // Simulate message processing
                ProcessMessage(receivedMessage);

                receivedCount++;
            };

            // Act
            for (int i = 0; i < messageCount; i++)
                {
                    receiver.SendMessage(message);
                }

                await Task.Delay(55000); // Wait for the messages to be processed

                // Assert
                Assert.AreEqual(messageCount, receivedCount);
            }

            private void ProcessMessage(string message)
            {
                // Simulate message processing time
                // Replace with your actual message processing logic
                // For example, you could write the message to a log or database.
                Task.Delay(1).Wait();
            }
        }

}

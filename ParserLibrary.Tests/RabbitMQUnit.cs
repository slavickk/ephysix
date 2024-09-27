/******************************************************************
 * File: RabbitMQUnit.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.RabbitMq;

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
            private RabbitMqContainer _container;

            private RabbitMQHelper _rabbitMqHelper;
            
            [OneTimeSetUp]
            public async Task GlobalSetup()
            {
                _container = new RabbitMqBuilder()
                    .WithImage("rabbitmq:3.11")
                    .WithUsername(Username)
                    .WithPassword(Password)
                    .WithPortBinding(5672)
                    .Build();
                await _container.StartAsync();
            }

            [OneTimeTearDown]
            public async Task GlobalTeardown()
            {
                await _container.DisposeAsync();
            }
            
            [SetUp]
            public void Setup()
            {
                _rabbitMqHelper = new RabbitMQHelper(Hostname, Port, Username, Password, QueueName)
                {
                    PrintReceivedNotification = false,
                    PrintSentNotification = false
                };
            }

            [TearDown]
            public void TearDown()
            {
                _rabbitMqHelper.StopReceiving();
            }

            [Test]
            public async Task PerformanceTest_MessageProcessingWithinThreshold()
            {
                // Arrange
                const int messageCount = 100;
                const string message = "Test message";
                int receivedCount = 0;
                int matchingMessages = 0;

                _rabbitMqHelper.StartReceiving();

                _rabbitMqHelper.Received += (_, e) =>
                {
                    var body = e.Body.ToArray();
                    var receivedMessage = Encoding.UTF8.GetString(body);

                    // Simulate message processing
                    ProcessMessageAsync(receivedMessage);

                    receivedCount++;
                    if (receivedMessage == message) matchingMessages++;
                };

                // Act
                for (int i = 0; i < messageCount; i++) _rabbitMqHelper.SendMessage(message);
                
                await Task.Delay(300); // Wait for the messages to be processed

                // Assert
                Assert.AreEqual(messageCount, receivedCount);
                Assert.AreEqual(messageCount, matchingMessages);
            }

            private void ProcessMessageAsync(string _)
            {
                // Simulate message processing time
                // Replace with your actual message processing logic
                // For example, you could write the message to a log or database.
                Task.Delay(1).Wait();
            }
        }

}

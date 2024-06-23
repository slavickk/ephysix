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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
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

            private RabbitMQHelper receiver;
            private ConnectionFactory factory;
            private IConnection connection;
            private IModel channel;
            private EventingBasicConsumer consumer;

            private RabbitMqContainer _container;
            
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

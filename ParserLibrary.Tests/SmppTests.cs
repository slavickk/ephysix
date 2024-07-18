/******************************************************************
 * File: RabbitMQPerformance.cs
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

using System;
using JamaaTech.Smpp.Net.Client;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;


namespace ParserLibrary.Tests
{
    [TestFixture]
    public class SmppTests
    {
        private IContainer _container;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            // Create and start the Docker container for SMSC simulator
            _container = new ContainerBuilder()
                .WithImage("ukarim/smscsim")
                .WithPortBinding(17000, 2775)
                .WithPortBinding(12775, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2775))
                .Build();
            
            await _container.StartAsync();
        }
        
        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            await _container.DisposeAsync();
        }
        
        [Test]
        public async Task TestSmpp()
        {
            var client = new SmppClient();
            var properties = client.Properties;
            properties.SystemID = "other";
            properties.Password = "other";
            properties.Port = 17000; //IP port to use
            properties.Host = "127.0.0.1"; //SMSC host name or IP Address
            properties.SystemType = "mysystemtype";
            properties.DefaultServiceType = "mydefaultservicetype";

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            //Start smpp client
            client.Start();

            // wait for the connection for maximum of 3 seconds
            var cts = new CancellationTokenSource();
            client.ConnectionStateChanged += (sender, e) =>
            {
                if (e.CurrentState == SmppConnectionState.Connected)
                    cts.Cancel();
            };
            await Task.Delay(TimeSpan.FromSeconds(3), cts.Token).ContinueWith(task => { }, TaskContinuationOptions.OnlyOnCanceled);
            
            if (client.ConnectionState != SmppConnectionState.Connected)
                Assert.Fail("Failed to connect to the SMSC simulator");

            var msg = new TextMessage
            {
                DestinationAddress = "+79222354098", //Receipient number
                SourceAddress = "300", //Originating number
                Text = "Hello, this is my test message!",
                RegisterDeliveryNotification = true //I want delivery notification for this message
            };
            
            client.SendMessage(msg);

            // Wait for delivery receipt (max 5 seconds) and check the message
            
            string actualDestinationAddress = null;
            string actualSourceAddress = null;
            
            cts = new CancellationTokenSource();
            client.MessageDelivered += (sender, e) =>
            {
                actualDestinationAddress = e.ShortMessage.DestinationAddress;
                actualSourceAddress = e.ShortMessage.SourceAddress;
                cts.Cancel();
            };
            
            await Task.Delay(TimeSpan.FromSeconds(5), cts.Token).ContinueWith(task => { }, TaskContinuationOptions.OnlyOnCanceled);
            
            // TODO: figure out - for some reason the source and destination addresses are swapped in the message receipt
            Assert.Warn("Source and destination addresses are unexpectedly swapped");
            Assert.AreEqual("+79222354098", actualSourceAddress);
            Assert.AreEqual("300", actualDestinationAddress);
            
            // TODO: figure out how to check the message text
        }
    }
}

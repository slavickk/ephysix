/******************************************************************
 * File: KafkaProducerTests.cs
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

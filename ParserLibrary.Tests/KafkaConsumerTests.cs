/******************************************************************
 * File: KafkaConsumerTests.cs
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

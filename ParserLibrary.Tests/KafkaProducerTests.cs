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
    [Test]
    public async Task ProduceMessageAsync(/*string topic, string message*/)
    {
        string topic = "UFD.TOPIC.OPERATION_HISTORY";
        string message = @"{
  ""internalId"": ""5fa85f6457174562b3fc2c963f66afa7"",
  ""customerId"": ""10000014998"",

  ""deviceId"": ""9fa85f6457174562b3fc2c963f66afa6"",

  ""type"": ""c2cTransferSbp"",

  ""transferAmount"": 1000.00,

  ""currencyIsoCode"": ""RUB"",

  ""operationDate"": ""2024-08-09T12:06:11.708Z"",

  ""status"": ""EXECUTED"",

  ""parametersVersion"": 1,

  ""senderInfo"": {

    ""senderProductId"": ""10000014345"",

    ""senderProductNumber"": ""40817810700007524127"",

    ""senderPhone"": ""79618751315"",

    ""senderName"": ""œÂÚÓ‚ —Â„ÂÈ “""

  },

  ""recipientInfo"": {

    ""recipientProductNumber"": ""40817810700007524123"",

    ""recipientPhone"": ""79618751316"",

    ""recipientName"": ""»‚‡ÌÓ‚  ËËÎÎ œ""

  },

  ""absIds"": [

    ""018fa55e5448122eaf3e72a8bff963fe"",

    ""019fa55e5448122eaf3e72a8bff963fe""

  ],

  ""externalIds"": [

    ""A4144121256183100000000011250501"",

    ""20240523100015110000000069833308"",

    ""2a0ef99ce60147e29cf166de64be12b8"",

    ""69833420""

  ],

  ""operationParameters"": [

    {

      ""name"": ""recipientPhone"",

      ""value"": ""79618751316""

    },

    {

      ""name"": ""recipientBankId"",

      ""value"": ""100000000004""

    },

    {

      ""name"": ""accountId"",

      ""value"": ""10000014345""

    },

    {

      ""name"": ""transferAmount"",

      ""value"": ""1000.00""

    }

  ]

} 
";
  /*      SslSocketFactory socketFactory = new SslSocketFactory(new TrustAllCerts())
        {
            // Additional socket factory configuration
        };
*/
        var config = new ProducerConfig
        {
            
            BootstrapServers = "10.200.100.11:9093", // Your Kafka server
            SecurityProtocol = SecurityProtocol.Plaintext,
          //  SslCaLocation/*SslCertificateLocation*/ = @"C:\¡√—\KafkaSSL\CARoot.pem",
          //  SslCertificateLocation/*SslCaLocation*/ = @"C:\¡√—\KafkaSSL\certificate.pem",
            /*   SslTruststoreLocation = "path/to/truststore.p12", // Path to your truststore
               SslTruststorePassword = "1qazXSW@",               // Password for truststore
              */
         //   SslKeyLocation= @"C:\¡√—\KafkaSSL\key.pem",
         //   SslKeyPassword= "1qazXSW@",
           /* SslKeystoreLocation = "C:/¡√—/KafkaSSL/keystore.p12",     // Path to your keystore
            SslKeystorePassword = "1qazXSW@",*/
            SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None/*.Https*/,
            SaslMechanism = SaslMechanism.Plain,
            // EnableSslCertificateVerification = true,
            // Password for keystore
            // Set the SslEndpointIdentificationAlgorithm to "https" or null
            // SslEndpointIdentificationAlgorithm = "https"     // Set to null if not required
            // Additional configs can be added as needed
        };
        try
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Message sent to topic {result.Topic} partition {result.Partition} at offset {result.Offset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
        catch(Exception e77)
        {
            
        }
    }
    [OneTimeTearDown]
    public void Teardown()
    {
        _producer.Dispose();
        _producer = null;
    }
}

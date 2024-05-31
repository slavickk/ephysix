/******************************************************************
 * File: RabbitMQHelper.cs
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
using System.Collections.Generic;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{

    public class RabbitMQHelper
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;

        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }

        public RabbitMQHelper(string hostname, int port, string username, string password, string queueName)
        {
            Hostname = hostname;
            Port = port;
            Username = username;
            Password = password;
            QueueName = queueName;
        }

        public void StartReceiving()
        {
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

            consumer = new EventingBasicConsumer(channel);
//            consumer.Received += Consumer_Received;

            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            consumer.Received += Consumer_Received;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received message: " + message);
        }

        public void StopReceiving()
        {
            channel.Close();
            connection.Close();
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
            Console.WriteLine("Sent message: " + message);
        }
    }
}

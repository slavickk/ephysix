/******************************************************************
 * File: SmppSupport.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using JamaaTech.Smpp.Net.Client;

namespace ParserLibrary
{

    public class SmppClientExample
    {
  //      private readonly SmppClient _client;

        public SmppClientExample()
        {
            SmppClient client = new SmppClient();
            System.Threading.Thread.Sleep(3000);
            SmppConnectionProperties properties = client.Properties;
            properties.SystemID = "other";
            properties.Password = "other";
            properties.Port = 7000; //IP port to use
            properties.Host = "10.77.206.210"; //SMSC host name or IP Address
            properties.SystemType = "mysystemtype";
            properties.DefaultServiceType = "mydefaultservicetype";

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            //Start smpp client
            client.Start();
            //client.ConnectionStateChanged += client_ConnectionStateChanged;

            TextMessage msg = new TextMessage();

            msg.DestinationAddress = "number"; //Receipient number
            msg.SourceAddress = "number"; //Originating number
            msg.Text = "Hello, this is my test message!";
            msg.RegisterDeliveryNotification = true; //I want delivery notification for this message

            //SmppClient client = GetSmppClient();
            client.SendMessage(msg);
        }

        public static  void SendSms(string phoneNumber="", string message = "")
        {
            SmppClient client = new SmppClient();
            System.Threading.Thread.Sleep(3000);
            SmppConnectionProperties properties = client.Properties;
            properties.SystemID = "other";
            properties.Password = "other";
            properties.Port = 7000; //IP port to use
            properties.Host = "10.77.206.210"; //SMSC host name or IP Address
            properties.SystemType = "mysystemtype";
            properties.DefaultServiceType = "mydefaultservicetype";

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            //Start smpp client
            client.Start();
            //client.ConnectionStateChanged += client_ConnectionStateChanged;

            TextMessage msg = new TextMessage();

            msg.DestinationAddress = "number"; //Receipient number
            msg.SourceAddress = "number"; //Originating number
            msg.Text = "Hello, this is my test message!";
            msg.RegisterDeliveryNotification = true; //I want delivery notification for this message

            //SmppClient client = GetSmppClient();
            client.SendMessage(msg);
            /*        var dataCoding = DataCodings.Default;
                    var sourceAddress = new SmeAddress();
                    var destinationAddress = new SmeAddress(phoneNumber);
                    var submitSm = new SubmitSm
                    {
                        SourceAddress = sourceAddress,
                        DestinationAddress = destinationAddress,
                        DataCoding = dataCoding,

                        MessageText = message
                    };

                    var response = _client.Submit(submitSm);
                    if (response.Header.Status == CommandStatus.ESME_ROK)
                    {
                        Console.WriteLine($"Message sent successfully. Message ID: {response.MessageId}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send message. Status: {response.Header.Status}");
                    }*/
        }

        public void Disconnect()
        {
            //_client.Disconnect();
        }
    }

   
}

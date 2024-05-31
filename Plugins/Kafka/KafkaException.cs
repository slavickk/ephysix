/******************************************************************
 * File: KafkaException.cs
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

using System.Runtime.Serialization;
using Confluent.Kafka;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Plugins.Kafka;

public class KafkaException : Exception
{
    public KafkaException()
    {
    }

    protected KafkaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public KafkaException(string? message) : base(message)
    {
    }
    
    public DeliveryResult<Null, string>? DeliveryResult { get; }
    
    public KafkaException(string? message, DeliveryResult<Null, string> deliveryResult) : base(message)
    {
        this.DeliveryResult = deliveryResult;
    }

    public KafkaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
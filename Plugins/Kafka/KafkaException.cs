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
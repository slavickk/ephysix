#nullable enable
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CCFAProtocols.TIC.ISO8583;
using Serilog;

namespace CCFAProtocols.TIC
{
    /// <summary>
    /// TIC Message
    /// </summary>
    /// <remarks><see cref="TICHeader"/>|<see cref="MessageTypeIdentifier"/>|...|...</remarks>
    public class TICMessage
    {
        private static readonly char[] prefix = { 'A', '4', 'M' };

        private static JsonSerializerOptions JsonSerializerOptions =
            new() { IncludeFields = true, IgnoreNullValues = true };

        [JsonIgnore] public static TICMessage EchoRequest = new()
        {
            Header = new TICHeader()
            {
                ProtocolVersion = 19,
                RejectStatus = 0
            },
            MessageType = new MessageTypeIdentifier()
            {
                IsReject = false,
                TypeIdentifier = EnumMessageTypeIdentifier.NetworkManagementRequest
            },
            Fields = new ISO8583.ISO8583()
            {
                NetworkManagementInformationCode = NetworkManagementInformationCodes.EchoTest,
                SytemTraceAuditNumber = 123456,
                TransmissionGreenwichTime = "0126150335"
            }
        };

        [JsonIgnore] public static TICMessage EchoResponse = new()
        {
            Header = new TICHeader()
            {
                ProtocolVersion = 19,
                RejectStatus = 0
            },
            MessageType = new MessageTypeIdentifier()
            {
                IsReject = false,
                TypeIdentifier = EnumMessageTypeIdentifier.NetworkManagementRequestResponse
            },
            Fields = new ISO8583.ISO8583()
            {
                NetworkManagementInformationCode = NetworkManagementInformationCodes.EchoTest,
                SytemTraceAuditNumber = 123456,
                TransmissionGreenwichTime = "0126150335"
            }
        };

        public ISO8583.ISO8583 Fields;
        public TICHeader Header;
        public MessageTypeIdentifier MessageType;

        [JsonIgnore] public uint TraceAuditNumber => Fields.SytemTraceAuditNumber ?? GenerateTraceAudit();

        private uint GenerateTraceAudit()
        {
            var hashCode = GetHashCode().ToString();

            return uint.Parse(hashCode[..6]);
        }

        public static TICMessage Deserialize(BinaryReader reader)
        {
            var cur_prefix = reader.ReadChars(3);
            if (!prefix.SequenceEqual(cur_prefix))
                throw new InvalidDataException(
                    $"NOT TIC Message: A4M expected, recieve {string.Join(',', cur_prefix)}");

            var message = new TICMessage();
            message.Header = TICHeader.Deserialize(reader);
            message.MessageType = MessageTypeIdentifier.Deserialize(reader);
            message.Fields = ISO8583.ISO8583.Deserialize(reader);
            Log.Verbose("Deserialized:{Message}", message);
            return message;
        }

        public static TICMessage Deserialize(byte[] bytes)
        {
            using (BinaryReader reader = new(new MemoryStream(bytes)))
            {
                return Deserialize(reader);
            }
        }

        public static string DeserializeToJSON(BinaryReader reader)
        {
            return Deserialize(reader).ToJSON();
        }

        public static string DeserializeToJSON(byte[] bytes)
        {
            return Deserialize(bytes).ToJSON();
        }

        public string ToJSON()
        {
            return JsonSerializer.Serialize(this, JsonSerializerOptions);
        }

        public void Serialize(BinaryWriter writer)
        {
            Log.Verbose("Serializing {Message}", this);
            Fields.SytemTraceAuditNumber ??= GenerateTraceAudit();
            writer.Write(prefix);
            Header.Serialize(writer);
            MessageType.Serialize(writer);
            Fields.Serialize(writer);
        }

        public byte[] Serialize()
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            Serialize(writer);
            return stream.ToArray();
        }

        public static TICMessage? FromJSON(string ticmessage)
        {
            return JsonSerializer.Deserialize<TICMessage>(ticmessage, JsonSerializerOptions);
        }

        public static void SerializeFromJson(BinaryWriter writer, string ticmessage)
        {
            FromJSON(ticmessage)?.Serialize(writer);
        }

        public static byte[] SerializeFromJson(string ticmessage)
        {
            return FromJSON(ticmessage).Serialize();
        }

        public override string ToString()
        {
            return string.Join('\n', "TICMessage:", Header.ToString(), MessageType.ToString(), Fields.ToString());
        }
    }
}
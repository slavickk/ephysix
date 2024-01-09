// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using ParserLibrary;
using Serilog;
using UniElLib;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

byte[] bytes =
{
    65, 52, 77, 49, 57, 48, 48, 48, 48, 56, 48, 48, 56, 50, 50, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48,
    52, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 49, 50, 49, 57, 48, 57, 53, 49, 53, 49, 56, 51, 48, 50,
    57, 52, 51, 48, 49
};
Console.WriteLine(bytes.LongLength);
// // TICFrame frame = TICFrame.GetFrame(6);
//
// // NetworkStream stream = new NetworkStream(bytes);
// // stream.Seek(0, SeekOrigin.Begin);
// // string? json = await frame.DeserializeToJson(stream);
// var json = TICMessage.DeserializeToJSON(bytes);


// JsonNode? node = JsonNode.Parse(json);
// Console.WriteLine(node.ToJsonString(new JsonSerializerOptions(){WriteIndented = true}));

TICSender sender = new TICSender() { twfaHost = "192.168.75.173", ticFrame = 6, twfaPort = 20101 };

string json =
    "{\"Fields\":{\"NetworkManagementInformationCode\":301,\"SytemTraceAuditNumber\":830294,\"TransmissionGreenwichTime\":\"1219095151\"},\"Header\":{\"ProtocolVersion\":19,\"RejectStatus\":0},\"MessageType\":{\"IsReject\":false,\"TypeIdentifier\":800}}";
string s = await sender.send(json, new ContextItem() { context = null });
JsonNode? node = JsonNode.Parse(s);
Console.WriteLine(node.ToJsonString(new JsonSerializerOptions() { WriteIndented = true }));
using System.CommandLine;
using System.Net;
using System.Net.Sockets;
using CCFAProtocols.TIC;
using ParserLibrary.TIC.TICFrames;

RootCommand root = new RootCommand();
Option<string> host = new Option<string>("--host", () => "0.0.0.0");
Option<int> port = new Option<int>("--port", () => 0);
root.AddGlobalOption(host);
root.AddGlobalOption(port);
var server = new Command("server");
server.SetHandler(async context =>
{
    await StartServer(context.ParseResult.GetValueForOption(host), context.ParseResult.GetValueForOption(port),
        context.GetCancellationToken());
});
root.AddCommand(server);
var client = new Command("client");
client.SetHandler(async context =>
{
    await StartClient(context.ParseResult.GetValueForOption(host), context.ParseResult.GetValueForOption(port),
        context.GetCancellationToken());
});
root.AddCommand(client);
await root.InvokeAsync(args);


async Task StartServer(string host, int port, CancellationToken token)
{
    RECONNECT:
    TcpListener listener = new TcpListener(IPEndPoint.Parse($"{host}:{port}"));
    listener.Start();
    Console.WriteLine("Start listening: {0}", listener.LocalEndpoint);
    TcpClient client = await listener.AcceptTcpClientAsync(token);
    Console.WriteLine($"Client Connected:{client.Client.RemoteEndPoint}");
    var stream = client.GetStream();
    var frame = TICFrame.GetFrame(6);
    while (!token.IsCancellationRequested)
    {
        try
        {
            var ticMessage = await frame.Deserialize(stream, token);
            await frame.Serialize(stream, ticMessage, token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (IOException e)
        {
            Console.WriteLine("Client disconnected");
            listener.Stop();
            goto RECONNECT;
        }
    }

    client.Close();
    Console.WriteLine("Stop listening");
    listener.Stop();
}

async Task StartClient(string host, int port, CancellationToken token)
{
    TcpClient client = new TcpClient(host, port);
    Console.WriteLine($"Connected to {client.Client.RemoteEndPoint}");
    var start_time = DateTime.Now;
    var req_count = 0;
    var stream = client.GetStream();
    var frame = TICFrame.GetFrame(6);
    try
    {
        while (!token.IsCancellationRequested)
        {
            await frame.Serialize(stream, TICMessage.EchoRequest, token);
            await frame.Deserialize(stream, token);
            req_count++;
        }
    }
    catch (OperationCanceledException e)
    {
    }

    var end_time = DateTime.Now;
    Console.WriteLine(
        $"REQ:{req_count} TIME:{end_time - start_time} Req/s: {req_count / (end_time - start_time).TotalSeconds} Mean Time,s: {(end_time - start_time).TotalSeconds / req_count}");
}
// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Threading.Tasks;
using ParserLibrary.TIP;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();
TIPReceiver tipReciever = new()
    {WorkDir = @"C:\TestTip"/* "/home/ilya/Documents/projects/integration-utility/TIPConsoleTest/Test"*/, DelayTime = 10};
string json_directory = Path.Combine(tipReciever.WorkDir, "json");
Directory.CreateDirectory(json_directory);
tipReciever.stringReceived = async (s, o) =>
{
    string filename = o as string;
    await File.AppendAllTextAsync(Path.Combine(json_directory, filename), s);
};
Task start = tipReciever.start();
Console.ReadKey();
Log.Information("Close programm");
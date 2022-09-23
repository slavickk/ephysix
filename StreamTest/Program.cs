using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using CCFAProtocols.TIC;
using Serilog;
using Serilog.Formatting.Compact;

namespace StreamTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("module", "TIC")
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            Log.Information("{test}", "test");

            var originalStream = new MemoryStream();
            TICMessage msg;
            using (var FileStream =
                File.Open(
                    @"/home/ilya/Documents/projects/protocols/CCFAProtocols.Tests/MESS_1",
                    FileMode.Open))
            {
                FileStream.CopyTo(originalStream);
            }

            originalStream.Seek(0, SeekOrigin.Begin);

            msg = TICMessage.Deserialize(new BinaryReader(originalStream));
            Console.WriteLine(msg);
            originalStream.Seek(0, SeekOrigin.Begin);
            using var stream = new MemoryStream();
            msg.Serialize();

            stream.Seek(0, SeekOrigin.Begin);

            //TODO: Move to extenstion
            Debug.Assert(stream.Length == originalStream.Length,
                $"stream.Length:{stream.Length} orig.Length:{originalStream.Length}");
            for (var i = 0; i < stream.Length; i++)
            {
                var s = stream.ReadByte();
                var o = originalStream.ReadByte();
                Debug.Assert(s == o, $"Diff Byte {i}: stream {s} orig {o}");
            }
        }
    }
}
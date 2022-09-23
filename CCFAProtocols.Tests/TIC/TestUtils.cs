using System;
using System.IO;
using System.Text;
using CCFAProtocols.TIC.Instruments;
using NUnit.Framework;
using Serilog;
using Serilog.Formatting.Compact;

namespace CCFAProtocols.Tests.TIC
{
    [TestFixture]
    public class TestUtils
    {
        [OneTimeSetUp]
        public void SetEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("module", "TEST")
                .CreateLogger();
        }

        [Test]
        public void TestReadByte()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("module", "TIC")
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();
            var stream = new MemoryStream();
            stream.Write(new byte[] {33, 33, 33});
            stream.Position = 0;
            var reader = new BinaryReader(stream, Encoding.GetEncoding(866));
            var exception = Assert.Catch(() => reader.ReadByte(3));
            Assert.AreEqual(typeof(FormatException), exception.GetType(), "Exception is not FormatException");
        }
    }
}
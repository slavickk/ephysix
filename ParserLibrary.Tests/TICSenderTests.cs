using System.IO;
using System.Threading.Tasks;
using CCFAProtocols.TIC;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class TICSenderTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var ticReciever = new TICReceiver()
            {
                port = _port, ticFrame = _senderTicFrame
            };
            ticReciever.stringReceived = (s, o) => ticReciever.sendResponseInternal(s, o);
            ticReciever.startInternal();
        }

        private int _port = 5000;
        private int _senderTicFrame = 6;

        [Test]
        public async Task SenderTest()
        {
            TICSender sender = new();
            sender.ticFrame = _senderTicFrame;
            sender.twfaHost = "localhost";
            sender.twfaPort = _port;

            byte[] bytes = File.ReadAllBytes("TestData/test200.tic")[2..];

            string respJson = await sender.send(TICMessage.DeserializeToJSON(bytes));

            byte[] resp = TICMessage.SerializeFromJson(respJson);
            Assert.AreEqual(bytes, resp);
        }
    }
}
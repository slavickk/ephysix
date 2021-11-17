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
            var ticReciever = new TICReciever()
            {
                Port = _port, TicFrame = _senderTicFrame
            };
            ticReciever.stringReceived = (s, o) => ticReciever.sendResponse(s, o);
            ticReciever.start();
        }

        private int _port = 5000;
        private int _senderTicFrame = 6;

        [Test]
        public async Task SenderTest()
        {
            TICSender sender = new();
            sender.TicFrame = _senderTicFrame;
            sender.TWFAHost = "localhost";
            sender.TWFAPort = _port;

            byte[] bytes = File.ReadAllBytes("TestData/test200.tic")[2..];

            string respJson = await sender.send(TICMessage.DeserializeToJSON(bytes));

            byte[] resp = TICMessage.SerializeFromJson(respJson);
            Assert.AreEqual(bytes, resp);
        }
    }
}
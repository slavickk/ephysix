using System.IO;
using System.Threading.Tasks;
using DummySystem1Protocols.DummyProtocol1;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class DummyProtocol1SenderTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var dummyProtocol1Receiver = new DummyProtocol1Reciever()
            {
                Port = _port, DummyProtocol1Frame = _senderDummyProtocol1Frame
            };
            dummyProtocol1Receiver.stringReceived = (s, o) => dummyProtocol1Receiver.sendResponse(s, o);
            dummyProtocol1Receiver.start();
        }

        private int _port = 5000;
        private int _senderDummyProtocol1Frame = 6;

        [Test]
        public async Task SenderTest()
        {
            DummyProtocol1Sender sender = new();
            sender.DummyProtocol1Frame = _senderDummyProtocol1Frame;
            sender.DummySystem3Host = "localhost";
            sender.DummySystem3Port = _port;

            byte[] bytes = File.ReadAllBytes("TestData/test200.dummy1")[2..];

            string respJson = await sender.send(DummyProtocol1Message.DeserializeToJSON(bytes));

            byte[] resp = DummyProtocol1Message.SerializeFromJson(respJson);
            Assert.AreEqual(bytes, resp);
        }
    }
}
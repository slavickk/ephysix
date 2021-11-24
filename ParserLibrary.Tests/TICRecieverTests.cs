using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    public class DummyProtocol1RecieverTests
    {
        int port = 5001;

        [OneTimeSetUp]
        public void Init()
        {
            var dummyProtocol1Receiver = new DummyProtocol1Receiver()
            {
                port = port, dummyProtocol1Frame = 6
            };
            dummyProtocol1Receiver.stringReceived = (s, o) => dummyProtocol1Receiver.sendResponseInternal(s, o);
            dummyProtocol1Receiver.startInternal();
        }

        [Test]
        public async Task TestServer()
        {
            byte[] bytes = File.ReadAllBytes("TestData/test200.dummy1");
            var tcpClient = new TcpClient();
            tcpClient.Connect(new IPEndPoint(IPAddress.Any, port));
            Assert.True(tcpClient.Connected);
            NetworkStream clientStream = tcpClient.GetStream();
            clientStream.Write(bytes);

            byte[] rec_bytes = new byte[bytes.Length];
            int BytesRead = clientStream.Read(rec_bytes, 0, bytes.Length);
            Assert.AreEqual(bytes.Length, rec_bytes.Length);
            Assert.AreEqual(bytes, rec_bytes);
        }
    }
}
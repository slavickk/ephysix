using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using ParserLibrary.DummyProtocol2;
using PluginBase;

namespace ParserLibrary.Tests
{
    public class DummyProtocol2RecieverTests
    {
        int port = 5001;

        [OneTimeSetUp]
        public void Init()
        {
            var dummyProtocol2Receiver = new DummyProtocol2Receiver()
            {
                WorkDir = @"C:\TestTip"
            };


            dummyProtocol2Receiver.stringReceived = (s, o) => dummyProtocol2Receiver.sendResponse(s, new ContextItem() { context = o });
            dummyProtocol2Receiver.start();
        }

        [Test]
        public async Task TestServer()
        {
//            File.Copy()

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
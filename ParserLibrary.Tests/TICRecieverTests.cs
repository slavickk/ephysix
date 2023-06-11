using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using PluginBase;

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
            dummyProtocol1Receiver.stringReceived = (s, o) => dummyProtocol1Receiver.sendResponse(s,new ContextItem() { context = o });
            dummyProtocol1Receiver.start();
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
            
            // Avoid infinite loop in case a bug causes the receiver to return less data than we expect
            clientStream.ReadTimeout = 1000;

            byte[] rec_bytes = new byte[bytes.Length];
            var BytesRead = 0;
            while (BytesRead < rec_bytes.Length)
                BytesRead += clientStream.Read(rec_bytes, BytesRead, bytes.Length - BytesRead);
            Assert.AreEqual(bytes.Length, BytesRead);
            Assert.AreEqual(bytes, rec_bytes);
        }
    }
}
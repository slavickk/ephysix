using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ParserLibrary.Tests
{
    public class TICRecieverTests
    {
        int port = 5001;

        [OneTimeSetUp]
        public void Init()
        {
            var ticReciever = new TICReceiver()
            {
                port = port, ticFrame = 6
            };
            ticReciever.stringReceived = (s, o) => ticReciever.sendResponse(s,new Step.ContextItem() { context = o });
            ticReciever.start();
        }

        [Test]
        public async Task TestServer()
        {
            byte[] bytes = File.ReadAllBytes("TestData/test200.tic");
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
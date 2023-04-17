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

            byte[] rec_bytes = new byte[bytes.Length];
            int BytesRead = clientStream.Read(rec_bytes, 0, bytes.Length);
            Assert.AreEqual(bytes.Length, rec_bytes.Length);
            Assert.AreEqual(bytes, rec_bytes);
        }
    }
}
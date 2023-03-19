using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using ParserLibrary.TIP;

namespace ParserLibrary.Tests
{
    public class TIPRecieverTests
    {
        int port = 5001;

        [OneTimeSetUp]
        public void Init()
        {
            var tipReciever = new TIPReceiver()
            {
                WorkDir = @"C:\TestTip"
            };


            tipReciever.stringReceived = (s, o) => tipReciever.sendResponse(s, o);
            tipReciever.start();
        }

        [Test]
        public async Task TestServer()
        {
//            File.Copy()

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
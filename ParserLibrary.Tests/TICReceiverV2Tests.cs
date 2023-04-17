using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using PluginBase;

namespace ParserLibrary.Tests;

public class TICReceiverV2Tests
{
    int port = 5001;
    
    private class ReceiverHostMock : IReceiverHost
    {
        public ReceiverHostMock(IReceiver receiver)
        {
            this._receiver = receiver;
        }
        
        private readonly IReceiver _receiver;

        /// <summary>
        /// The signal method is called by the receiver when a new message is received.
        /// This dummy implementation just send the received message back to the sender.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        public async Task signal(string input, object context)
        {
            Console.WriteLine("Test signal");
            Console.WriteLine("Input: " + input);
            Console.WriteLine("Context: " + context);
            
            await _receiver.sendResponse(input, new Step.ContextItem() { context = context });
        }
        
        public string IDStep => "DummyStep";
    }

    [OneTimeSetUp]
    public void Init()
    {
        var ticReciever = new TICReceiverV2()
        {
            port = port, ticFrame = 6
        };
        // ticReciever.stringReceived = (s, o) => ticReciever.sendResponse(s,new Step.ContextItem() { context = o });
        
        // Create a receiver host that will handle messages from the receiver.
        var receiverHost = new ReceiverHostMock(ticReciever);
        ((IReceiver)ticReciever).host = receiverHost;
        
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
        var BytesRead = 0;
        
        // Avoid infinite loop in case a bug causes the receiver to return less data than we expect
        clientStream.ReadTimeout = 1000;
        
        while (BytesRead < rec_bytes.Length)
            BytesRead += clientStream.Read(rec_bytes, BytesRead, bytes.Length - BytesRead);
        Assert.AreEqual(bytes.Length, BytesRead);
        Assert.AreEqual(bytes, rec_bytes);
    }

}
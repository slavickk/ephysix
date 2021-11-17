using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.DummyProtocol1.DummyProtocol1Frames;

namespace ParserLibrary
{
    public class DummyProtocol1Sender : Sender
    {
        private DummyProtocol1Frame Frame;
        private TcpClient? dummySystem3Client;
        public string DummySystem3Host;
        public int DummySystem3Port;
        public override TypeContent typeContent => TypeContent.json;


        public int DummyProtocol1Frame
        {
            get => Frame.FrameNum;
            set => Frame = DummyProtocol1Frame.GetFrame(value);
        }

        public override async Task<string> send(string JsonBody)
        {
            if (dummySystem3Client is null || !dummySystem3Client.Connected)
            {
                dummySystem3Client = new TcpClient();
                await dummySystem3Client.ConnectAsync(DummySystem3Host, DummySystem3Port);
            }

            NetworkStream stream = dummySystem3Client.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            return await Frame.DeserializeToJson(stream);
        }
    }
}
using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.DummyProtocol1.DummyProtocol1Frames;

namespace ParserLibrary
{
    public class DummyProtocol1Sender : Sender
    {
        private DummyProtocol1Frame Frame;
        private TcpClient? dummySystem3Client;
        public string dummySystem3Host= "192.168.75.148";
        public int dummySystem3Port=5553;
        public override TypeContent typeContent => TypeContent.json;


        public int dummyProtocol1Frame = 6;
/*        {
            get => Frame.FrameNum;
            set => Frame = DummyProtocol1Frame.GetFrame(value);
        }*/

        public override async Task<string> send(string JsonBody)
        {
            if (dummySystem3Client is null || !dummySystem3Client.Connected)
            {
                Frame = DummyProtocol1Frame.GetFrame(dummyProtocol1Frame);
                dummySystem3Client = new TcpClient();
                await dummySystem3Client.ConnectAsync(dummySystem3Host, dummySystem3Port);
            }

            NetworkStream stream = dummySystem3Client.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            return await Frame.DeserializeToJson(stream);
        }
    }
}
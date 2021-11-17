using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.TIC.TICFrames;

namespace ParserLibrary
{
    public class TICSender : Sender
    {
        private TICFrame Frame;
        private TcpClient? twfaclient;
        public string TWFAHost;
        public int TWFAPort;
        public override TypeContent typeContent => TypeContent.json;


        public int TicFrame
        {
            get => Frame.FrameNum;
            set => Frame = TICFrame.GetFrame(value);
        }

        public override async Task<string> send(string JsonBody)
        {
            if (twfaclient is null || !twfaclient.Connected)
            {
                twfaclient = new TcpClient();
                await twfaclient.ConnectAsync(TWFAHost, TWFAPort);
            }

            NetworkStream stream = twfaclient.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            return await Frame.DeserializeToJson(stream);
        }
    }
}
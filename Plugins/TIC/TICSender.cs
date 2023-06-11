using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.TIC.TICFrames;
using PluginBase;

namespace ParserLibrary
{
    public class TICSender : Sender
    {
        private TICFrame Frame;
        private TcpClient? twfaclient;
        public string twfaHost= "192.168.75.148";
        public int twfaPort=5553;
        public override TypeContent typeContent => TypeContent.json;


        public int ticFrame = 6;
/*        {
            get => Frame.FrameNum;
            set => Frame = TICFrame.GetFrame(value);
        }*/

        public override async Task<string> send(string JsonBody, ContextItem context)
        {
            if (twfaclient is null || !twfaclient.Connected)
            {
                Frame = TICFrame.GetFrame(ticFrame);
                twfaclient = new TcpClient();
                await twfaclient.ConnectAsync(twfaHost, twfaPort);
            }

            NetworkStream stream = twfaclient.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            return await Frame.DeserializeToJson(stream);
        }
    }
}
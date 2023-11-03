using System.Net.Sockets;
using System.Threading.Tasks;
using ParserLibrary.TIC.TICFrames;
using PluginBase;
using UniElLib;


namespace Plugins
{
    public class TICSender : ISender
    {
        private TICFrame Frame;
        private TcpClient? twfaclient;
        public string twfaHost= "192.168.75.148";
        public int twfaPort=5553;
        public void setTemplate(string key, string body)
        {
            throw new NotImplementedException();
        }

        public TypeContent typeContent => TypeContent.json;
        public void Init()
        {
            throw new NotImplementedException();
        }


        public int ticFrame = 6;
/*        {
            get => Frame.FrameNum;
            set => Frame = TICFrame.GetFrame(value);
        }*/

        public ISenderHost host { get; set; }

        public Task<string> send(UniElLib.AbstrParser.UniEl root, ContextItem context)
        {
            return Task.FromResult(string.Empty);
        }

        public async Task<string> send(string JsonBody, ContextItem context)
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

        public string getTemplate(string key)
        {
            return string.Empty;
        }
    }
}
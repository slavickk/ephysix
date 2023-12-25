using System.Diagnostics;
using System.Net.Sockets;
using ParserLibrary.TIC.TICFrames;
using Serilog;
using UniElLib;

namespace ParserLibrary
{
    public class TICSender : Sender
    {
        private readonly ActivitySource _activitySource = new(nameof(TICSender));
        private TICFrame Frame;


        public int ticFrame = 6;
        private TcpClient? twfaclient;
        public string twfaHost = "192.168.75.148";
        public int twfaPort = 5553;

        public override TypeContent typeContent => TypeContent.json;
/*        {
            get => Frame.FrameNum;
            set => Frame = TICFrame.GetFrame(value);
        }*/

        public override async Task<string> send(string JsonBody, ContextItem context)
        {
            using var activity = _activitySource.StartActivity();
            if (twfaclient is null || !twfaclient.Connected)
            {
                activity.AddEvent(new ActivityEvent("Connect to twfa"));
                Frame = TICFrame.GetFrame(ticFrame);
                twfaclient = new TcpClient();
                await twfaclient.ConnectAsync(twfaHost, twfaPort);
            }

            Log.Debug("Request: {request}", JsonBody);
            NetworkStream stream = twfaclient.GetStream();
            await Frame.SerializeFromJson(stream, JsonBody);
            var resp = await Frame.DeserializeToJson(stream);
            Log.Debug("Response: {response}", resp);
            return resp;
        }
    }
}
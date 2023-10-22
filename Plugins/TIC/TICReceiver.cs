using System.Net;
using System.Net.Sockets;
using System.Text;
using ParserLibrary.TIC.TICFrames;
using Serilog;
using Serilog.Context;

namespace ParserLibrary
{
    public class TICReceiver : Receiver, IDisposable
    {
        public override ProtocolType protocolType => ProtocolType.tcp;

        private readonly IDisposable pushProperty;
        private IPEndPoint endpoint;
        private TICFrame Frame;

        public TICReceiver()
        {
            if (port == -1)
                port = 15001;
            cantTryParse = true;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            pushProperty = LogContext.PushProperty("reciever", "TIC");
        }

        public int ticFrame = 5; //; { get; set; }
/*                {
                    get => Frame.FrameNum;
                    set { Frame = TICFrame.GetFrame(value); }
                }*/

        //        [YamlMember(Alias = "Port")]
//        public int port = 15001; //{ get; set; }
/*              {
                  set => endpoint = new IPEndPoint(IPAddress.Any, value);
                  get => endpoint.Port;
              }*/

        public void Dispose()
        {
            pushProperty.Dispose();
        }

        protected override async Task sendResponseInternal(string response, object context)
        {
            NetworkStream networkStream = context as NetworkStream;
            await Frame.SerializeFromJson(networkStream, response);
        }

        protected override async Task startInternal()
        {
            Frame = TICFrame.GetFrame(ticFrame);
            endpoint = new IPEndPoint(IPAddress.Any, port);
            CancellationToken cancellationToken = CancellationToken.None;

            var taskFactory = new TaskFactory(cancellationToken,
                TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None, TaskScheduler.Current);

            TcpListener tcpListener = new TcpListener(endpoint);
            tcpListener.Start();
            Log.Information("Start listening {endpoint}", endpoint);
            cancellationToken.Register(tcpListener.Stop);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    taskFactory.StartNew(() => ClientHandler(tcpClient, cancellationToken), cancellationToken);
                    // Interlocked.Increment(ref CurrentTasks);
                }
            }
            catch (SocketException e) when (e.ErrorCode == 125)
            {
                // if (CurrentTasks > 0) await Task.Delay(1000);
            }
            finally
            {
                tcpListener.Stop();
                Log.Information("Stop Listening");
            }
        }

        /*  public override bool cantTryParse()
          {
              return true;
          }*/
        public async Task ClientHandler(TcpClient client, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("client", client.Client.RemoteEndPoint))
            {
                Log.Information("Accepting client");
                using var clientStream = client.GetStream();
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        string TICMessageJson;
                        try
                        {
                            TICMessageJson = await Frame.DeserializeToJson(clientStream, cancellationToken);
                            if (TICMessageJson is null) continue;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Exception in TIC");
                            continue;
                        }

                        await signal(TICMessageJson, clientStream);
                    }
                }
                catch (IOException e) when (e.InnerException.GetType().Equals(typeof(SocketException)))
                {
                    throw;
                }
                catch (OperationCanceledException e)
                {
                }
                catch (Exception e)
                {
                    Log.Error(e, "Exception in TICReciever");
                    throw;
                }
                finally
                {
                    Log.Information("Disconnect client");
                    client.Close();
                    // Interlocked.Decrement(ref CurrentTasks);
                }
            }
        }
    }
}